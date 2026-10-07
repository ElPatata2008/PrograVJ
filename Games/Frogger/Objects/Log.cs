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

namespace PrograVJ.Games.Frogger.Objects
{
    public class Log : Square
    {
        private Random rand;
        public float speed = 5;

        public Log(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
        }

        public override void Update(float dt)
        {
            double rad = rotation.Z * (Math.PI / 180);
            position.X += speed * (float)Math.Cos(rad) * dt;
        }

        public void CreateCollider() => collider = new BoxCollider2D(this);
    }
}
