using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.MP.Objects
{
    public class Bullet : Square
    {
        public Bullet(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
        }

        public bool enemy { get; set; }
        public float xSpeed { get; set; }
        public float ySpeed { get; set; }


        
        public override void Update(float dt)
        {
            position.X += xSpeed * dt;
            position.Y += ySpeed * dt;
        }
    }
}
