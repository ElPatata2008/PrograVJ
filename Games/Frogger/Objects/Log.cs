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
        public Log(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, borderWidth, fillTexture)
        {
            rand = new Random();
            speed = rand.Next(4, 7);
        }

        public override void Update()
        {
            double rad = rotation.Z * (Math.PI / 180);
            position.X += speed * (float)Math.Cos(rad);
        }

        public void CreateCollider() => collider = new BoxCollider2D(this);
    }
}
