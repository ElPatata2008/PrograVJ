using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Figures
{
    class Square : GameObject
    {
        public Square(Vector3 position, Vector3 rotation, Vector3 size, Color color) : base(position, rotation, size, color)
        {
        }

        public override void Draw(Graphics g)
        {

            // S * R * T

            // Scale
            float halfX = size.X / 2;
            float halfY = size.Y / 2;
            Vector3[] localVertices = new Vector3[4]
            {
                new Vector3(-halfX, -halfY, 0),
                new Vector3( halfX, -halfY, 0),
                new Vector3( halfX,  halfY, 0),
                new Vector3(-halfX,  halfY, 0),
            };
            Vector3[] worldPoints = new Vector3[4];
            for (int i = 0; i < 4; i++) {
                // Rotation
                Vector3 rotated = RotatePoint(localVertices[i]);

                // Translation
                Vector3 worldPoint = position + rotated;
                worldPoints[i] = worldPoint;
            }

            PointF[] screenPoint = new PointF[4]
            {
                new PointF(worldPoints[0].X, worldPoints[0].Y),
                new PointF(worldPoints[1].X, worldPoints[1].Y),
                new PointF(worldPoints[2].X, worldPoints[2].Y),
                new PointF(worldPoints[3].X, worldPoints[3].Y)
            };

            g.DrawPolygon(new Pen(new SolidBrush(color)), screenPoint);



        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
