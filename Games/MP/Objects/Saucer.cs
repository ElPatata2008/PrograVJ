using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
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

        public Saucer(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
            hp = 10;

        }

        public float hLimits { get; set; }



        public override void Update(float dt)
        {
            position.X += speed * dt;

            if (position.X > hLimits && speed > 0) speed *= -1;
            else if (position.X < -hLimits && speed < 0) speed *= -1;
        }
        public Bullet Attack()
        {
            return new Bullet(
                new Vector3(position.X, position.Y - 50, position.Z),
                Vector3.Zero,
                new Vector3(5, 75, 10),
                Color.Red, Color.Red
            )
            { ySpeed = -8, enemy = true }; ;
        }
    }
}
