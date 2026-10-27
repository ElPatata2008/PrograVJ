using NAudio.CoreAudioApi;
using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class HoverState : FSMState
    {
        float timer = 0;
        float laserTimer = 0;
        float maxSpeed = 125f;
        float spawnrate = 3f;

        public HoverState(AIController controller) : base(controller) { }

        public override void OnEnter()
        {
            if ((controller.pawn as Boss2).Phase2)
            {
                maxSpeed = 300f;
                spawnrate = 0.75f;
            }
        }

        public override void OnExit() { timer = 0; }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is Boss2 pawn)
            {
                timer += deltaTime;
                if (timer > spawnrate)
                {
                    pawn.spawnEnemy = true;
                    timer = 0;
                }

                if (!pawn.Phase2)
                {
                    laserTimer += deltaTime;
                    if (laserTimer > 1.5f)
                    {
                        pawn.spawnLaser = true;
                        laserTimer = 0;
                    }
                }

                pawn.rotation.Z = MathUtils.GetAngleFrom2Points(pawn.position, pawn.playerPos);

                float dx = pawn.playerPos.X - pawn.position.X;
                float dy = (pawn.playerPos.Y + 250) - pawn.position.Y;
                float xSpeed = dx * pawn.speed * deltaTime;
                float ySpeed = dy * pawn.speed * 0.5f * deltaTime;
                pawn.body.AddForce(xSpeed, ySpeed, 0);
                pawn.body.velocity.X = MathUtils.Clamp(pawn.body.velocity.X, -maxSpeed, maxSpeed);
                pawn.body.velocity.Y = MathUtils.Clamp(pawn.body.velocity.Y, -maxSpeed, maxSpeed);
            }
        }
    }

    public class ChargeState : FSMState
    {
        float timer = 0;
        float spawnTimer = 0;
        float dashCD = 2;
        float dashStrength = 250f;
        float breakSpeed = 2f;
        Vector2 dir;
        int dashCount = 0;
        int maxDashes = 3;
        bool dashed = false;
        public ChargeState(AIController controller) : base(controller) { }

        public override void OnEnter() {
            if (controller.pawn is Boss2 pawn)
            {
                pawn.rotation.Z = MathUtils.GetAngleFrom2Points(pawn.position, pawn.playerPos);
                dir = MathUtils.GetDirectionFromAngle(pawn.rotation.Z);
                pawn.body.velocity = Vector3.Zero;
                pawn.body.AddImpulse(dir.X * dashStrength, dir.Y * dashStrength, 0);
                maxDashes = pawn.Phase2 ? 6 : 3;
            }
            dashCD = 2;
            dashStrength = 250f;
            dashCount++;
            timer = 0;
            AudioManager.PlaySFX("roar1");
        }

        public override void OnExit() {
            (controller.pawn as Boss2).charge = false;
            timer = 0;
            dashed = false;
            dashStrength = 250f;
            dashCD = 2;
            dashCount = 0;
        }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is Boss2 pawn)
            {
                timer += deltaTime;
                if (dashCount == maxDashes)
                {
                    if (timer > dashCD / 2)
                    {
                        pawn.rotation.Z = MathUtils.GetAngleFrom2Points(pawn.position, pawn.playerPos);
                        dir = MathUtils.GetDirectionFromAngle(pawn.rotation.Z);
                    }
                    if (timer > dashCD * 2)
                    {
                        dashStrength = 250f;
                        (controller.brain as FSM).ChangeState("Hover");
                    }
                }
                else
                {
                    if (timer < dashCD)
                    {
                        if (timer > dashCD / 2)
                        {
                            pawn.rotation.Z = MathUtils.GetAngleFrom2Points(pawn.position, pawn.playerPos);
                            dir = MathUtils.GetDirectionFromAngle(pawn.rotation.Z);
                        }

                        if (pawn.Phase2 && dashCount < 3)
                        {
                            spawnTimer += deltaTime;
                            if (spawnTimer > 0.5f)
                            {
                                pawn.spawnEnemy = true;
                                spawnTimer = 0;
                            }
                        }
                    }
                    else
                    {
                        if (!dashed)
                        {
                            dashed = true;
                            dashCount++;
                            pawn.body.velocity = Vector3.Zero;
                            pawn.body.AddImpulse(dir.X * dashStrength, dir.Y * dashStrength, 0);
                            if (dashCount > 3) {
                                AudioManager.PlaySFX("roar2");
                                dashCD = 0.75f;
                            }
                            else AudioManager.PlaySFX("roar1");
                        }
                        else
                        {
                            
                            if (Math.Abs(pawn.body.velocity.X) <= breakSpeed) pawn.body.velocity.X = 0;
                            else pawn.body.velocity.X -= Math.Sign(pawn.body.velocity.X) * breakSpeed * deltaTime;
                            if (Math.Abs(pawn.body.velocity.Y) <= breakSpeed) pawn.body.velocity.Y = 0;
                            else pawn.body.velocity.Y -= Math.Sign(pawn.body.velocity.Y) * breakSpeed * deltaTime;



                            if (timer > dashCD) {
                                timer = 0;
                                dashed = false;
                                if (pawn.Phase2) {
                                    if (dashCount == 3) {
                                        dashStrength = 1000f;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
    }

    public class TransformState : FSMState
    {
        float breakSpeed = 2f;
        float rotSpeed = 0;
        bool ending = false;
        float duration = 0;
        float timer = 0;
        public TransformState(AIController controller) : base(controller) { }

        public override void OnEnter() { }

        public override void OnExit()
        {
            duration = 0;
        }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is Boss2 pawn)
            {
                duration += deltaTime;
                pawn.rotation.Z += rotSpeed * deltaTime;
                if (!ending)
                {
                    if (duration >= 1)
                    {
                        if (rotSpeed >= 1080) rotSpeed = 1080;
                        else rotSpeed += 1000 * deltaTime;
                        timer += deltaTime;
                        if (timer > 0.25f)
                        {
                            pawn.spawnEnemy = true;
                            timer = 0;
                        }
                    }
                    if (duration > 3)
                    {
                        timer = 0f;
                        ending = true;
                        AudioManager.PlaySFX("roar1");
                        pawn.animationController.Play("phase2");
                        duration = 0;
                    }
                }
                else
                {
                    if (rotSpeed <= 0) rotSpeed = 0;
                    else rotSpeed -= 1000 * deltaTime;
                    if (duration > 1)
                    {
                        (controller.brain as FSM).ChangeState("Hover");
                    }
                }

                if (Math.Abs(pawn.body.velocity.X) <= breakSpeed) pawn.body.velocity.X = 0;
                else pawn.body.velocity.X -= Math.Sign(pawn.body.velocity.X) * breakSpeed;
                if (Math.Abs(pawn.body.velocity.Y) <= breakSpeed) pawn.body.velocity.Y = 0;
                else pawn.body.velocity.Y -= Math.Sign(pawn.body.velocity.Y) * breakSpeed;
            }
        }

    }

    public class Boss2 : Enemy
    {
        public bool Phase2 = false;
        public bool transforming = false;
        public float phaseTimer = 0;
        public bool charge = false;
        public float speed = 3f;
        public bool spawnEnemy = false;
        public bool spawnLaser = false;
        Random rnd = new Random();
        public Boss2(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            if (controller.brain is FSM fsm)
            {
                fsm.AddState("Hover", new HoverState(controller));
                fsm.AddState("Charge", new ChargeState(controller));
                fsm.AddState("Transform", new TransformState(controller));
            }

            SpriteSheet phase1 = new SpriteSheet(TextureManager.Get("boss2phase1"), 32, 32, 1, 1);
            animationController.AddAnimation("phase1", phase1);

            SpriteSheet phase2 = new SpriteSheet(TextureManager.Get("boss2phase2"), 32, 32, 0.1f, 4);
            animationController.AddAnimation("phase2", phase2);
        }

        public override void InitStats()
        {
            switch (TBS.config.difficulty)
            {
                case 0: { SetStats(2800, 15); } break;
                case 1: { SetStats(3640, 30); } break;
                case 2: { SetStats(4641, 45); } break;
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            animationController.Update(dt);
            
            if (!charge) phaseTimer += dt;

            Phase2 = hp < maxHp / 2;

            if (Phase2 && !transforming)
            {
                speed *= 2.5f;
                fsm.ChangeState("Transform");
                transforming = true;
                phaseTimer = 0;
            }

            if (phaseTimer > (Phase2 ? 6 : 12))
            {
                fsm.ChangeState("Charge");
                charge = true;
                phaseTimer = 0;
            }

        }

        public MinionBoss2 SpawnMinion()
        {
            float rndAngle = rnd.Next(0, 360);
            MinionBoss2 minion = new MinionBoss2(
                position,
                new Vector3(0, 0, rndAngle),
                Vector3.One * 20,
                Color.Black, Color.White,
                Phase2,
                body: new Rigidbody(RigidbodyType.Dynamic, 50, 0),
                fillTexture: TextureManager.Get("minionBoss2")
            );
            minion.InitStats();
            minion.SetCollision();
            return minion;
        }

        public Boss2Laser Shoot()
        {
            float inaccurary = (float)rnd.NextDouble() * -4;
            Boss2Laser b = new Boss2Laser(
                position, 
                new Vector3(0, 0, rotation.Z + inaccurary),
                new Vector3(60, 2, 0),
                Color.Red, Color.Red,
                (int)(dmg * 1.5), 0, 700f
            );
            b.SetCol();
            AudioManager.PlaySFX("laser");
            return b;
        }
    }
}
