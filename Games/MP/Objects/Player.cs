using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.MP.Objects
{
    public class Player : Square
    {
        public float speed = 0;
        private float vSpeed = 0;
        private bool jumped = false;
        private Stopwatch sw = new Stopwatch();
        private float initY;

        public float accel { get; set; }
        public float gravity { get; set; }
        public float maxSpeed { get; set; }
        public float jumpStrenght { get; set; }
        private Bitmap defaultTexture;
        public Bitmap invensibleTexture { get; set; }

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor, float borderWidth = 2, Bitmap fillTexture = null)
            : base(position, rotation, size, color, isActive, fillColor, borderWidth, fillTexture) {
            initY = position.Y;
            defaultTexture = fillTexture;
            sw.Start();
        }

        public override void Update()
        {
            if (speed > maxSpeed) speed = maxSpeed;
            if (speed < -maxSpeed) speed = -maxSpeed;

            position.X += speed;
            position.Y += vSpeed;


            if (jumped)
            {
                vSpeed -= gravity;
                if (position.Y < initY)
                {
                    position.Y = initY;
                    vSpeed = 0;
                    jumped = false;
                }
            }

            if (sw.ElapsedMilliseconds / 1000 > 5) fillTexture = defaultTexture; 
            else fillTexture = invensibleTexture;
            //if (sw.ElapsedMilliseconds / 1000 > 5) fillColor = Color.White; 
            //else fillColor = Color.Gray;
        }


        public void Right() => speed += accel * 1.25f;
        public void Left() => speed -= accel;
        public void Jump()
        {
            if (vSpeed == 0) { 
                jumped = true; 
                vSpeed = jumpStrenght;
            }
        }

        public bool CanBeDamaged() => sw.ElapsedMilliseconds / 1000 > 5;
        public void TookDamage() => sw.Restart();
        public void StopClock() => sw.Stop();

        public Bullet Attack(float width, float height, float xSpeed, float ySpeed) => new Bullet(
            position, 
            Vector3.Zero, 
            new Vector3(width, height, 0), 
            Color.Yellow, true, Color.Yellow)
            { xSpeed = xSpeed, ySpeed = ySpeed, enemy = false };
}
}
