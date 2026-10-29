using NAudio.Wave;
using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class JumpState : FSMState
    {
        float jumpTimer = 0;
        float hSpeed = 100f;
        Random rnd = new Random();
        public JumpState(AIController controller) : base(controller) { }

        public override void OnEnter() {  }

        public override void OnExit() { jumpTimer = 0; }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is Boss1 pawn)
            {
                if (pawn.isGrounded)
                {
                    jumpTimer += deltaTime;
                    float percent = (float)pawn.hp / pawn.maxHp;
                    float cd = MathUtils.Lerp(0.1f, 1f, percent);
                    if (jumpTimer > cd)
                    {
                        jumpTimer = 0;
                        pawn.body.gravityScale = 1;
                        pawn.isGrounded = false;
                        float jumpStrenghtX = rnd.NextDouble() < 0.33 ? hSpeed * 2f : hSpeed;
                        float finalJumpX = pawn.facingRight ? jumpStrenghtX : -jumpStrenghtX;
                        float clampedPercent = MathUtils.Clamp(percent, 0.75f, 1f);
                        float jumpStrenghtY = ((pawn.jumpCount % 3 == 2) ? 500f : 350f) * clampedPercent;
                        pawn.body.AddImpulse(finalJumpX, jumpStrenghtY, 0);
                        pawn.jumpCount++;
                        pawn.animationController.Play("jump");
                    }
                }
            }
        }
    }

    public class AttackState : FSMState
    {
        float timer = 0;
        bool attacked = false;
        
        public AttackState(AIController controller) : base(controller) { }

        public override void OnEnter() { (controller.pawn as Boss1).animationController.Play("attackCharge"); }

        public override void OnExit() { timer = 0; attacked = false;
            (controller.pawn as Boss1).release = false; 
        }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is Boss1 pawn)
            {
                timer += deltaTime;
                if (!attacked && timer > 2) {
                    attacked = true;
                    pawn.attack = true;
                    pawn.animationController.Play("attackRelease");
                }
                if (pawn.release) (controller.brain as FSM).ChangeState("Jump");
            }
        }
    }

    public class Boss1 : Enemy
    {
        public bool isGrounded = false;
        public int jumpCount = 0;
        public bool facingRight = false;
        public float spawnChance = 0.05f;
        public bool spawnEnemy = false;
        public bool attack = false;
        Random rnd = new Random();
        Vector3 initSize;
        Vector3 minSize;
        private int width = 48, height = 32;
        public bool release = false;
        public Square crown;

        public Boss1(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null
            ) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {

            initSize = size;
            minSize = size / 1.5f;
                
            if (controller.brain is FSM fsm) {
                fsm.AddState("Jump", new JumpState(controller));
                fsm.AddState("Attack", new AttackState(controller));
            }

            SpriteSheet idle = new SpriteSheet(TextureManager.Get("boss1idle"), width, height, 0.15f, 5);
            animationController.AddAnimation("idle", idle);

            SpriteSheet jump = new SpriteSheet(TextureManager.Get("boss1jump"), width, height, 0.15f, 2);
            animationController.AddAnimation("jump", jump);

            SpriteSheet attackCharge = new SpriteSheet(TextureManager.Get("boss1attackCharge"), width, height, 0.15f, 4);
            animationController.AddAnimation("attackCharge", attackCharge);

            SpriteSheet attackRelease = new SpriteSheet(TextureManager.Get("boss1attackRelease"), width, height, 0.15f, 9);
            attackRelease.AddEvent(7, () => release = true);
            animationController.AddAnimation("attackRelease", attackRelease);

            animationController.Play("idle");

            crown = new Square(
                position - new Vector3(0, size.Y / 2 - 100, 0),
                Vector3.Zero,
                new Vector3(100, 50, 0),
                Color.Black, Color.Gold,
                fillTexture: TextureManager.Get("crown")
            );
        }

        public override void InitStats()
        {
            switch (TBS.config.difficulty)
            {
                case 0: { SetStats(2000, 40); } break;
                case 1: { SetStats(2800, 64); } break;
                case 2: { SetStats(3570, 96); } break;
            }
        }

        public override void Draw(Graphics g, Camera c)
        {
            base.Draw(g, c);
            crown.Draw(g, c);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            animationController.Update(dt);

            crown.position = position + new Vector3(0, size.Y / 5 + crown.size.Y / 2 + (isGrounded ? 0 : 20), 0);

            spawnChance = getChance(0.05f, 0.25f);
            size = ChangeSize();

            facingRight = playerPos.X > position.X;

            if (isGrounded && jumpCount == 6) {
                fsm.ChangeState("Attack");
                jumpCount = 0;
            }

            if (position.Y - size.Y / 2 < -200)
            {
                position.Y = -200 + size.Y / 2;
                body.velocity = Vector3.Zero;
                isGrounded = true;
                body.gravityScale = 0;
                animationController.Play("idle");
            }
        }

        private float getChance(float pMin, float pHalf)
        {
            if (hp <= maxHp / 2) return pHalf;
            float slope = 2 * (pHalf - pMin);
            float intersection = pMin + slope;
            return intersection - (slope * ((float)hp / maxHp));
        }

        private Vector3 ChangeSize()
        {
            float hpPercent = (float)hp / maxHp;
            hpPercent = MathUtils.Clamp(hpPercent, 0, 1);
            Vector3 newSize = MathUtils.LerpVector3(minSize, initSize, hpPercent);
            if (isGrounded) position.Y = -200 + newSize.Y / 2 + 1;
            return newSize;
        }

        public MinionBoss1 SpawnMinion(Vector2 dir, bool facing)
        {
            bool spiked = rnd.NextDouble() < 0.25;
            MinionBoss1 minion = new MinionBoss1(
                position, rotation,
                Vector3.One * 32,
                Color.Black, spiked ? Color.CadetBlue : Color.Blue, spiked,
                body: new Rigidbody(RigidbodyType.Dynamic, 50, 1),
                controller: new AIController(controller.bb, new FSM()),
                animationController: new AnimationController()
            );

            minion.InitStats();
            minion.SetCollision();
            if (facing) dir.X *= facingRight ? 1 : -1;
            minion.body.AddImpulse(dir.X, dir.Y, 0);
            return minion;
        }

        public List<Boss1Spike> Attack()
        {
            List<Boss1Spike> spikes = new List<Boss1Spike>();
            for (int i = -6; i < 6; i++)
            {
                Boss1Spike s = new Boss1Spike(
                    position,
                    new Vector3(0, 0, 18 * (i + 5)),
                    new Vector3(24, 12, 0),
                    Color.Black, Color.BlueViolet,
                    dmg * 2, 0, 250f * (1 + Math.Abs(i) / 10),
                    fillTexture: TextureManager.Get("boss1Spike")
                )
                { rotSpeed = 45f / Math.Abs(i) };
                s.SetCol();
                spikes.Add(s);
            }
            Console.WriteLine("Attacked");
            return spikes;
        }
    }
}
