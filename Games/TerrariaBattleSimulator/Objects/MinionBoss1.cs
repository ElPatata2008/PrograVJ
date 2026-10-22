using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.Cuphead.Objects;
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
        Stopwatch sw = new Stopwatch();
        float hSpeed = 50f;
        Random rnd = new Random();
        public MoveState(AIController controller) : base(controller) { }

        public override void OnEnter() { }

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
                        pawn.body.AddImpulse(0, (pawn.jumpCount % 3 == 2) ? 50f : 35f, 0);
                        pawn.isGrounded = false;
                        pawn.jumpCount++;
                    }
                }
                else
                {
                    float jumpSpeed = rnd.NextDouble() < 0.33 ? hSpeed * 2f : hSpeed;
                    pawn.position.X += (pawn.facingRight ? jumpSpeed : -jumpSpeed) * deltaTime;
                }
            }
        }
    }
    public class MinionBoss1 : Square
    {
        public MinionBoss1(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
        }
    }
}
