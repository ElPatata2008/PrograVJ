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
    public class Triangle : GameObject
    {
        Color fillColor;

        public Triangle(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null) : base(position, rotation, size, color, collider)
        {
            this.fillColor = fillColor;
        }

        public override void Draw(Graphics g, Camera c)
        {
            Vector3[] localVertices = new Vector3[3]
            {
                new Vector3(-0.5f, -0.5f, 0f),
                new Vector3( 0.5f, 0, 0f),
                new Vector3(-0.5f, 0.5f, 0f),
            };

            PointF[] screenPoints = new PointF[3];
            for (int i = 0; i < localVertices.Length; i++)
            {
                Vector3 scalePoint = MathUtils.Scale(localVertices[i], size);
                Vector3 rotationPoint = MathUtils.Rotate(scalePoint, rotation);
                Vector3 worldPoint = MathUtils.Translate(rotationPoint, position);

                Vector3 viewPoint = c.TransformPoint(worldPoint);
                screenPoints[i] = c.ProjectPoint(viewPoint, Program.resolution);
            }

            g.FillPolygon(new SolidBrush(fillColor), screenPoints);
            g.DrawPolygon(new Pen(new SolidBrush(color), 2f), screenPoints);
        }

        public override void OnColiisionStay(GameObject c) { }

        public override void OnCollisionEnter(GameObject c) { }

        public override void OnCollisionExit(GameObject c) { }

        public override void Update(float dt) { }
    }
}
