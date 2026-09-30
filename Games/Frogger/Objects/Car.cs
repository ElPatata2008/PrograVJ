using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Frogger.Objects
{
    public class Car : Square
    {
        public int speed { get; set; }
        public Car(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, borderWidth, fillTexture)
        {

        }

        public override void Update()
        {
            var rad = rotation.Z * (Math.PI / 180);
            var dir = Math.Cos(rad);
            position.X += speed * (float)dir;
        }

        public void CreateCollider() => collider = new BoxCollider2D(this);
    }
}
