using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Asteroids
{
    public class Asteroid : Square
    {
        public float speed;
        Random random = new Random();

        public float dirX, dirY;
        public int type = 0;

        public Asteroid(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor, 
            int type) : base(position, rotation, size, color, isActive, fillColor)
        {
            this.type = type;

            speed = random.Next(1, 3) * 1.75f;
            Vector2 dir = MathUtils.GetAngle2D(rotation.Z);

            dirX = dir.X; dirY = dir.Y;
        }

        public override void Update()
        {
            position.X += dirX * speed;
            position.Y += dirY * speed;
        }

        public bool Collision(float x, float y)
        {
            float dx = x - position.X;
            float dy = y - position.Y;

            Vector2 rads = MathUtils.GetAngle2D(rotation.Z);



            return false;
        }

        public void SetSpeed(float speed) => this.speed = speed;
    }
}
