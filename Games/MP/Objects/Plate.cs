using PrograVJ.Engine.Colliders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.MP.Objects
{
    public class Plate : Enemy
    {
        
        public bool shot { get; set; }
        public int type { get; set; } // 0 = left, 1 = right, 2 = above

        public Plate(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, borderWidth, fillTexture)
        {
            hp = 1;
        }

        public override void Update()
        {
            double rad = rotation.Z * (Math.PI / 180);

            position.X += speed * (float)Math.Cos(rad);
            position.Y += speed * (float)Math.Sin(rad);
        }

        public void SetRotation(float rot) => rotation.Z = rot;
        public void Rotate(float dir) => rotation.Z += dir;

        public Bullet Attack(float x, float y, float width, float height)
        {
            return new Bullet(
                position,
                Vector3.Zero,
                new Vector3(width, height, 10),
                Color.Yellow, Color.Red
            )
            { xSpeed = x, ySpeed = y, enemy = true }; ;
        }
    }
}
