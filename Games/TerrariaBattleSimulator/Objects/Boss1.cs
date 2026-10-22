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
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Cuphead.Objects
{
    public class Phase1JumpState : FSMState
    {
        Stopwatch sw = new Stopwatch();
        float hSpeed = 50f;
        Random rnd = new Random();
        int dir = 1;
        public Phase1JumpState(AIController controller) : base(controller) { }

        public override void OnEnter() {  }

        public override void OnExit() { sw.Reset(); }

        public override void Update(float deltaTime)
        {
            if (controller.pawn is Boss1 pawn)
            {
                if (pawn.isGrounded)
                {
                    sw.Start();
                    if (sw.ElapsedMilliseconds / 1000 > 1)
                    {
                        sw.Reset();
                        pawn.body.velocity = Vector3.Zero;
                        pawn.body.type = RigidbodyType.Dynamic;
                        pawn.body.AddImpulse(0, (pawn.jumpCount % 3 == 2) ? 500f : 350f, 0);
                        pawn.isGrounded = false;
                        pawn.jumpCount++;
                        dir = pawn.facingRight ? 1 : -1;
                    }
                }
                else
                {
                    float jumpSpeed = rnd.NextDouble() < 0.33 ? hSpeed * 2f : hSpeed;
                    pawn.position.X += jumpSpeed * dir * deltaTime;
                }
            }
        }
    }

    public class Phase1AttackState : FSMState
    {
        Stopwatch sw = new Stopwatch();
        
        public Phase1AttackState(AIController controller) : base(controller) { }

        public override void OnEnter() { sw.Start(); (controller.pawn as Boss1).fillColor = Color.Orange; }

        public override void OnExit() { sw.Reset(); (controller.pawn as Boss1).fillColor = Color.White; }

        public override void Update(float deltaTime)
        {
            Boss1 pawn = controller.pawn as Boss1;
            if (sw.ElapsedMilliseconds / 1000 > 3)
            {

            }
        }
    }

    public class Boss1 : Square
    {
        public int hp;
        public int dmg;
        public bool isGrounded = false;
        public int jumpCount = 0;
        public bool facingRight = false;
        bool attacked = false;

        public Boss1(Vector3 position,
                     Vector3 rotation,
                     Vector3 size,
                     Color color,
                     Color fillColor,
                     Collider collider = null,
                     Rigidbody body = null,
                     AIController controller = null,
                     float borderWidth = 2,
                     Bitmap fillTexture = null,
                     WrapMode mode = WrapMode.Clamp,
                     int repeatTileX = 1,
                     int repeatTileY = 1,
                     AnimationController animationController = null
            ) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            switch (TerrariaBattleSimulator.config.difficulty) {
                case 0: { SetStats(2000, 40); } break;
                case 1: { SetStats(2800, 64); } break;
                case 2: { SetStats(3570, 96); } break;
            }
            Console.WriteLine($"HP: {hp}, DMG: {dmg}");
                
            if (controller.brain is FSM fsm)
            {
                fsm.AddState("Phase1Jump", new Phase1JumpState(controller));
                fsm.AddState("Phase1Attack", new Phase1AttackState(controller));
            }
        }
        private void SetStats(int hp, int dmg) { this.hp = hp; this.dmg = dmg; }

        public override void Update(float dt)
        {
            FSM fsm = controller.brain as FSM;
            controller.brain.Update(dt);
            if (position.X - size.X / 2 < -400) facingRight = true;
            if (position.X + size.X / 2 >  400) facingRight = false;

            //if (jumpCount == 3) fsm.ChangeState("Phase1Jump");
            //if ()

            if (position.Y - size.Y / 2 < -200)
            {
                position.Y = -200 + size.Y / 2;
                body.type = RigidbodyType.Static;
                isGrounded = true;
            }
        }
    }
}
