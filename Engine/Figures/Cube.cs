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
    public class Cube : GameObject
    {
        Color fillColor;
        public Cube(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive,
            Color fillColor) : base(position, rotation, size, color, isActive)
        {
            this.fillColor = fillColor;
        }

        public override void Draw(Graphics g, Camera c)
        {
            float halfX = size.X / 2;
            float halfY = size.Y / 2;
            float halfZ = size.Z / 2;

            Vector3[] localVertices = new Vector3[8]
            {
                new Vector3(-halfX, -halfY, -halfZ),
                new Vector3( halfX, -halfY, -halfZ),
                new Vector3( halfX,  halfY, -halfZ),
                new Vector3(-halfX,  halfY, -halfZ),
                new Vector3(-halfX, -halfY,  halfZ),
                new Vector3( halfX, -halfY,  halfZ),
                new Vector3( halfX,  halfY,  halfZ),
                new Vector3(-halfX,  halfY,  halfZ),
            };

            int[][] faces = new int[][]
            {
                new int[] {4, 5, 6, 7},
                new int[] {1, 0, 3, 2},
                new int[] {0, 4, 7, 3},
                new int[] {5, 1, 2, 6},
                new int[] {0, 1, 5, 4},
                new int[] {7, 6, 2, 1}
            };

            PointF[] screenPoints = new PointF[8];
            Vector3[] worldPoints = new Vector3[8];
            for (int i = 0; i < localVertices.Length; i++)
            {
                Vector3 scalePoint = MathUtils.Scale(localVertices[i], size);
                Vector3 rotationPoint = MathUtils.Rotate(scalePoint, rotation);
                worldPoints = MathUtils.Translate(rotationPoint, position);
                Vector3 viewPoint = camera.TransformPoint(worldPoints);

                screenPoints[i] = camera.ProjectedPoint(viewPoint, );
                if (viewPoint.Z <= 0.1f) screenPoints
            }

            foreach (int[] face in faces)
            {
                Vector3 w0 = worldPoints[face[0]], w1 = worldPoints[face[1]], w2 = worldPoints[face[2]], w3 = worldPoints[face[3]];
                Vector3 v1 = worldPoints[face[1]] - worldPoints[face[0]];
                Vector3 v2 = worldPoints[face[2]] - worldPoints[face[0]];


                Vector3 faceCenter = (w0 + w1 + w2 + w3) * 0.2f;
                Vector3 normal = Vector3.Cross(v1, v2);
                Vector3 viewDir = camera.GetViewDir(faceCenter);

                if (!Vector3 Dot(normal, viewDir) >= 0)
                {
                    PointF p0 = screenPoints[face[0]], p1 = screenPoints[face[1]], p2 = screenPoints[face[2]];
                    if (p0.X == -9999f || p0.Y == -9999f || p0.Z == -9999f ||
                        p1.X == -9999f || p1.Y == -9999f || p1.Z == -9999f ||
                        p2.X == -9999f || p2.Y == -9999f || p2.Z == -9999f ||
                        p3.X == -9999f || p3.Y == -9999f || p3.Z == -9999f
                        ) continue;

                    PointF[] facePolygonPoints = new PointF[4]
                    {
                    screenPoints[face[0]],
                    screenPoints[face[1]],
                    screenPoints[face[2]],
                    screenPoints[face[3]],
                    };
                    g.FillPolygon(new SolidBrush(fillColor), facePolygonPoints);
                    g.DrawPolygon(new Pen(new SolidBrush(color), 2f), facePolygonPoints);
                }
            }

        }

        public override void Update()
        {
            
        }
    }
}
