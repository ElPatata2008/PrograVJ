using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
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
    public class Player : Square
    {
        public bool isGrounded = false;
        public bool jumped = false;
        public bool doubleJumped = false;
        float speed = 20f;
        float maxSpeed = 150f;
        float knockback = 400f;
        bool facingRight = false;

        Stopwatch iFrames = new Stopwatch();
        public Stopwatch attackDelay = new Stopwatch();

        

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            iFrames.Start();
            attackDelay.Start();
        }

        public override void Update(float dt)
        {
            if (InputManager.JustPressedInput("w")) Jump();
            bool a = InputManager.GetInput("a");
            bool d = InputManager.GetInput("d");
            if (a && !d) MoveLeft(dt);
            if (!a && d) MoveRight(dt);

            if (!a && !d)
            {
                if (body.velocity.X > 0) body.velocity.X -= speed;
                if (body.velocity.X < 0) body.velocity.X += speed;
            }

            body.gravityScale = isGrounded ? 0 : 1;

            if (position.Y - size.Y / 2 < -200)
            {
                position.Y = -200 + size.Y / 2;
                body.velocity.Y = 0;
                isGrounded = true;
                jumped = false;
                doubleJumped = false;
            }
        }

        public void Jump() {
            if (isGrounded) {
                if (!jumped) {
                    body.velocity.Y = 0;
                    body.AddImpulse(0, 300f, 0);
                    isGrounded = false;
                    jumped = true;
                }
            } else {
                if (!doubleJumped) {
                    body.velocity.Y = 0;
                    body.AddImpulse(0, 300f, 0);
                    isGrounded = false;
                    doubleJumped = true;
                }
            }
        }

        public void MoveRight(float dt)
        {
            facingRight = true;
            body.AddImpulse(speed, 0, 0);
            if (body.velocity.X > maxSpeed) body.velocity.X = maxSpeed;
        }

        public void MoveLeft(float dt) {
            facingRight = false;
            body.AddImpulse(-speed, 0, 0);
            if (body.velocity.X < -maxSpeed) body.velocity.X = -maxSpeed;
        }

        public PlayerBullet Shoot(Camera c)
        {
            float dx = InputManager.MouseLocation.X + c.position.X - position.X;
            float dy = InputManager.MouseLocation.Y + c.position.Y + position.Y;
            double rad = Math.Atan2(dy, dx);
            double deg = rad * (180.0 / Math.PI);
            if (deg < 0) deg += 360.0;
            attackDelay.Restart();
            Console.WriteLine("Bullet Shot");
            PlayerBullet b = new PlayerBullet(
                position,
                new Vector3(0, 0, (float)deg),
                new Vector3(10, 2, 0),
                Color.Red, Color.Orange
            );
            b.SetCol();
            return b;
        }

        public override void OnColiisionStay(GameObject c) {
            if (c is Boss1 boss)
            {
                if (iFrames.ElapsedMilliseconds / 1000 > 1) {
                    body.velocity = Vector3.Zero;
                    isGrounded = false;
                    body.AddImpulse(position.X > boss.position.X ? knockback : -knockback, knockback * 0.5f,0);
                    iFrames.Restart();
                }
            }
        }
    }
}
