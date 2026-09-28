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
        public bool enemy { get; set; }
        public float xSpeed { get; set; }
        public float ySpeed { get; set; }

        public Bullet(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, isActive, fillColor, borderWidth, fillTexture)
        {
        }

        public override void Update()
        {
            position.X += xSpeed;
            position.Y += ySpeed;
        }
    }
}
