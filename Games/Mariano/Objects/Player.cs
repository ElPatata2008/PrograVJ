using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Objects
{
    public class Player : Square
    {
        public bool isGrounded = false;
        public int hp = 3;
        public int score = 0;
        private float initGrav;
        private float speed;
        private float maxSpeed;
        private Stopwatch sw = new Stopwatch();
        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor,
            float speed, float maxSpeed,
            Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
            initGrav = body.gravityScale;
            this.speed = speed;
            this.maxSpeed = maxSpeed;
            sw.Start();
        }

        public override void Update(float dt)
        {
            body.gravityScale = isGrounded ? 0 : initGrav;
            if (InputManager.GetInput("left")) body.AddForce(-speed, 0, 0);
            if (InputManager.GetInput("right")) body.AddForce(speed, 0, 0);
            if (isGrounded && InputManager.JustPressedInput("up"))
            {
                AudioManager.PlaySFX("jump");
                body.AddImpulse(0, 650, 0);
            }

            if (body.velocity.X < -maxSpeed) body.velocity.X = -maxSpeed;
            if (body.velocity.X > maxSpeed) body.velocity.X = maxSpeed;

            //Console.WriteLine(body.velocity.X);

            bool isMoving = InputManager.GetInput("left") || InputManager.GetInput("right");
            if (!isMoving)
            {
                if (body.velocity.X > 0) body.velocity.X -= speed;
                if (body.velocity.X < 0) body.velocity.X += speed;
            }

            if (CanTakeDamage())
            {
                fillColor = Color.White;
            }
            else
            {
                fillColor = Color.Red;
            }
        }

        public void TakeDamage()
        {
            AudioManager.PlaySFX("damage");
            sw.Restart();
            hp--;
        }

        public bool CanTakeDamage() => sw.ElapsedMilliseconds / 1000 > 4;
    }
}
