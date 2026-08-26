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
    public class Pad : GameObject
    {
        GameObject target;
        float speed;
        float hLimits;

        public bool bounced;

        bool left, right;

        public Pad(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, 
            float speed, float vLimits) : base(position, rotation, size, color, isActive)
        {
            this.speed = speed;
            this.hLimits = vLimits;

            bounced = false;
            left = false;
            right = false;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), position.X, position.Y, size.X, size.Y);
        }

        public override void Update()
        {
            if (left && position.X > 0) position.X -= speed;
            if (right && position.X + size.X < hLimits) position.X += speed;
        }

        public void SetTarget(GameObject obj) => target = obj;

        public bool CanBounce()
        {
            bool inBounds = target.position.X + target.size.X > position.X && target.position.X < position.X + size.X;

            return target.position.Y + target.size.Y > position.Y && inBounds && !bounced;
        }

        public void Left(bool keyPressed) => left = keyPressed;
        public void Right(bool keyPressed) => right = keyPressed;
    }
}
