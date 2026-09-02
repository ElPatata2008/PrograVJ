using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Asteroids
{
    public class Bullet : Square
    {
        float speed;

        public Bullet(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor,
            float speed) : base(position, rotation, size, color, isActive, fillColor)
        {
            this.speed = speed;
        }

        public override void Update()
        {
            Vector2 dir = MathUtils.GetAngle2D(rotation.Z);

            position.X += dir.X * speed;
            position.Y += dir.Y * speed;
        }
    }
}
