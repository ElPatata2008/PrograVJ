using PrograVJ.Engine;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Arkanoid
{
    public class Ball : GameObject
    {
        float speed, initSpeed;
        public float dirX, dirY;
        Vector3 initPos;

        float hLimit;

        public Ball(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, 
            float speed, float hLimit) : base(position, rotation, size, color, isActive)
        {
            this.speed = speed;
            initSpeed = speed;
            this.hLimit = hLimit;
            initPos = position;

            SetRandomDir();
        }

        public override void Draw(Graphics g, Camera c)
        {
            g.FillEllipse(new SolidBrush(color), position.X, position.Y, size.X, size.Y);
        }

        public override void Update()
        {
            if (position.Y < 0) dirY = 1;
            if (position.X < 0) dirX = 1;
            if (position.X + size.X > hLimit) dirX = -1;

            position.X += speed * dirX;
            position.Y += speed * dirY;
        }

        public void SetRandomDir()
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
        public void Stop() => speed = 0;

        public void Restart()
        {
            SetInitPosition();
            SetRandomDir();
            speed = initSpeed;

        }
    }
}
