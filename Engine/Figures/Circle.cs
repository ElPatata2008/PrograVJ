using PrograVJ.Engine.Colliders;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Figures
{
    public class Circle : GameObject
    {
        float radius;
        Color fillColor;
        public Circle(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null) : base(position, rotation, size, color, collider)
        {
            radius = size.X;
            this.fillColor = fillColor;
        }

        public override void Draw(Graphics g, Camera c)
        {
            RectangleF circle = new RectangleF(position.X, position.Y, radius, radius);
            g.FillEllipse(new SolidBrush(fillColor), circle);
            g.DrawEllipse(new Pen(color), circle);
        }

        public override void Update() { }
    }
}
