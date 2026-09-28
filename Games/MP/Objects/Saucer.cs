using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.MP.Objects
{
    public class Saucer : Enemy
    {
        public bool shot = false;
        public float hLimits { get; set; }

        public Saucer(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, isActive, fillColor, borderWidth, fillTexture)
        {
            hp = 10;
        }

        public override void Update()
        {
            position.X += speed;

            if (position.X > hLimits && speed > 0) speed *= -1;
            else if (position.X < -hLimits && speed < 0) speed *= -1;
        }
        public Bullet Attack()
        {
            return new Bullet(
                new Vector3(position.X, position.Y - 50, position.Z),
                Vector3.Zero,
                new Vector3(5, 75, 10),
                Color.Red, true, Color.Red
            )
            { ySpeed = -8, enemy = true }; ;
        }
    }
}
