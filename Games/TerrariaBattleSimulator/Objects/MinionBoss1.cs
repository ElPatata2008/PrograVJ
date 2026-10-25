using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
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

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class MoveState : FSMState
    {
        float jumpTimer = 0;
        float hSpeed = 75f;
        Random rnd = new Random();
        public MoveState(AIController controller) : base(controller) { }

        public override void OnEnter() { }

        public override void OnExit() { jumpTimer = 0; }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is MinionBoss1 pawn)
            {
                if (pawn.isGrounded)
                {
                    float dist = Vector3.Distance(pawn.position, pawn.playerPos);
                    if (dist <= pawn.shootRange && pawn.spiked) (controller.brain as FSM).ChangeState("Shoot");

                    jumpTimer += deltaTime;
                    if (jumpTimer > 0.5f)
                    {
                        jumpTimer = 0;
                        pawn.body.gravityScale = 1;
                        pawn.isGrounded = false;
                        float jumpStrenght = rnd.NextDouble() < 0.33 ? hSpeed * 2f : hSpeed;
                        float finalJump = pawn.facingRight ? jumpStrenght : -jumpStrenght;
                        pawn.body.AddImpulse(finalJump, (pawn.jumpCount % 3 == 2) ? 250f : 200f, 0);
                        pawn.jumpCount++;
                    }
                    
                }
            }
        }
    }

    public class ShootState : FSMState
    {
        float timer = 0;
        public ShootState(AIController controller) : base(controller) { }

        public override void OnEnter() { }

        public override void OnExit() { timer = 0; }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is MinionBoss1 pawn)
            {
                float dist = Vector3.Distance(pawn.position, pawn.playerPos);
                if (dist > pawn.shootRange) (controller.brain as FSM).ChangeState("Move");

                timer += deltaTime;
                if (timer > 1.5f)
                {
                    pawn.attack = true;
                    timer = 0;
                }
            }
        }
    }
    public class MinionBoss1 : Enemy {
        public bool isGrounded = false;
        public bool spiked = false;
        public int jumpCount = 0;
        public bool facingRight = false;
        public bool attack = false;
        public float shootRange = 200f;
        Random rnd = new Random();
        public MinionBoss1(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, 
            bool spiked,
            Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            this.spiked = spiked;
            switch(TerrariaBattleSimulator.config.difficulty)
            {
                case 0: SetStats(spiked ?  50 : 25, spiked ? 14 :  7); break;
                case 1: SetStats(spiked ? 100 : 50, spiked ? 28 : 14); break;
                case 2: SetStats(spiked ? 150 : 75, spiked ? 42 : 21); break;
            }
            if (controller.brain is FSM fsm)
            {
                fsm.AddState("Move", new MoveState(controller));
                fsm.AddState("Shoot", new ShootState(controller));
            }
        }
        public void SetCollision() => collider = new BoxCollider2D(this);

        public override void Update(float dt)
        {
            base.Update(dt);

            facingRight = playerPos.X > position.X;

            if (position.Y - size.Y / 2 < -200)
            {
                position.Y = -200 + size.Y / 2;
                body.velocity = Vector3.Zero;
                isGrounded = true;
                body.gravityScale = 0;
            }
        }

        public Boss1Spike Shoot()
        {
            float angle = MathUtils.GetAngleFrom2Points(position, playerPos);
            angle += (float)rnd.NextDouble() * -10;
            Boss1Spike s = new Boss1Spike(
                position,
                new Vector3(0, 0, angle),
                new Vector3(10, 5, 0),
                Color.Black, Color.BlueViolet,
                dmg * 2, 0, 175f
            );
            s.SetCol();
            return s;
        }
    }
}
