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
        public Triangle(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive) : base(position, rotation, size, color, isActive)
        {
        }

        public override void Draw(Graphics g, Camera c)
        {
            // S * R * T

            // Scale
            float halfX = size.X / 2;
            float halfY = size.Y / 2;
            Vector3[] localVertices = new Vector3[3]
            {
                new Vector3(-halfX, -halfY, 0f),
                new Vector3(halfX, 0, 0f),
                new Vector3(-halfX, halfY, 0f),
                //new Vector3( halfX, -halfY, 0f),
                //new Vector3( halfX,  halfY, 0f),
                //new Vector3(-halfX,  halfY, 0f),
            };

            PointF[] screenPoints = new PointF[3];
            for (int i = 0; i < localVertices.Length; i++)
            {
                Vector3 scalePoint = MathUtils.Scale(localVertices[i], size);
                Vector3 rotationPoint = MathUtils.Rotate(scalePoint, rotation);
                Vector3 worldPoint = MathUtils.Translate(rotationPoint, position);

                //screenPoints[i] = new PointF(worldPoint.X, worldPoint.Y);

                Vector3 viewPoint = c.TransformPoint(worldPoint);
                screenPoints[i] = c.ProjectPoint(viewPoint, Program.resolution);
            }

            //g.FillPolygon(new SolidBrush(fillColor), screenPoints);
            g.DrawPolygon(new Pen(new SolidBrush(color), 2f), screenPoints);
        }

        public override void Update()
        {
            
        }
    }
}
