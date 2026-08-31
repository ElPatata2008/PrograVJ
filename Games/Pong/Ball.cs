using PrograVJ.Engine;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games
{
    public class Ball : GameObject
    {
        public float dirX, dirY;
        float speed;
        Vector3 initPos;

        public Ball(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive,
            float speed) : base(position, rotation, size, color, isActive)
        {
            this.speed = speed;
            initPos = position;
            
            SetRandomDirection();
        }

        public override void Draw(Graphics g, Camera c)
        {
            g.DrawEllipse(new Pen(new SolidBrush(color)), position.X, position.Y, size.X, size.Y);
        }

        public override void Update()
        {
            position.X += speed * dirX;
            position.Y += speed * dirY;
        }

        public void SetRandomDirection()
        {
            Random rnd = new Random();
            switch (rnd.Next(3))
            {
                case 0: dirX = 1; dirY = 1; break;
                case 1: dirX = 1; dirY = -1; break;
                case 2: dirX = -1; dirY = -1; break;
                case 3: dirX = -1; dirY = 1; break;
            }
        }

        public void SetInitPosition() => position = initPos;
    }
}
