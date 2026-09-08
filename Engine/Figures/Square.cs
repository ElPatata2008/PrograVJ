using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Engine.Figures
{
    public struct Vertex3D
    {
        public Vector3 position;
        public PointF UV;
    }

    public class Square : GameObject
    {
        Vector3[] localVertices;
        PointF[] localUVs = new PointF[] {
            new PointF(0, 1), 
            new PointF(1, 1),
            new PointF(1, 0), 
            new PointF(0, 0)
        };

        List<Vertex3D> viewPoints = new List<Vertex3D>();
        PointF[] polygonPoints;

        Color fillColor;
        Bitmap fillTexture;
        float borderWidth;

        public Square(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive,
            Color fillColor, float borderWidth = 2f, Bitmap fillTexture = null) : base(position, rotation, size, color, isActive)
        {
            this.fillColor = fillColor;
            this.borderWidth = borderWidth;
            this.fillTexture = fillTexture;
        }

        public override void Draw(Graphics g, Camera c)
        {
            
            //Brush b; Pen p;

            float halfX = size.X / 2;
            float halfY = size.Y / 2;
            localVertices = new Vector3[4]
            {
                new Vector3(-halfX, -halfY, 0f),
                new Vector3( halfX, -halfY, 0f),
                new Vector3( halfX,  halfY, 0f),
                new Vector3(-halfX,  halfY, 0f),
            };

            //PointF[] screenPoints = new PointF[4];
            //for (int i = 0; i < localVertices.Length; i++) {
            //    // S * R * T
            //    Vector3 scalePoint = MathUtils.Scale(localVertices[i], size);
            //    Vector3 rotationPoint = MathUtils.Rotate(scalePoint, rotation);
            //    Vector3 worldPoint = MathUtils.Translate(rotationPoint, position);

            //    Vector3 viewPoint = c.TransformPoint(worldPoint);
            //    screenPoints[i] = c.ProjectPoint(viewPoint, Program.resolution);
            //}

            List<Vertex3D> viewSpace = TransformToViewSpace(c);
            List<Vertex3D> clippedPoints = ClipPolygon(viewPoints, c.nearZ);

            if (clippedPoints.Count < 3) return;

            Vertex3D[] screenPoly = ProjectToScreen(clippedPoints, c);
            PointF[] screenPoints = screenPoly.Select(v => new PointF(v.position.X, v.position.Y)).ToArray();

            Brush b = fillTexture == null
                ? new SolidBrush(fillColor)
                : BuildTextureBrush(screenPoly);

            Pen p = new Pen(b, borderWidth);

            g.FillPolygon(b, screenPoints);
            g.DrawPolygon(p, screenPoints);

        }

        private List<Vertex3D> ClipPolygon(List<Vertex3D> vertices, float nearZ)
        {
            List<Vertex3D> output = new List<Vertex3D>();
            int n = vertices.Count();

            for (int i = 0; i < n; i++)
            {
                Vertex3D current = vertices[i];
                Vertex3D next = vertices[(i + 1) % n];

                bool currentInside = current.position.Z > nearZ;
                bool nextInside = next.position.Z > nearZ;

                if (currentInside) output.Add(current);

                if (currentInside != nextInside)
                {
                    float t = (nearZ - current.position.Z) / (next.position.Z - current.position.Z);

                    Vertex3D intersection = new Vertex3D
                    {
                        position = Vector3.Lerp(current.position, next.position, t),
                        UV = Lerp(current.UV, next.UV, t)
                    };
                    output.Add(intersection);
                }
            }

            return output;
        }

        private PointF Lerp(PointF a, PointF b, float t) => new PointF(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);

        private List<Vertex3D> TransformToViewSpace(Camera c)
        {
            viewPoints.Clear();
            for (int i = 0; i < localVertices.Length; i++)
            {
                Vector3 scale = MathUtils.Scale(localVertices[i], size);
                Vector3 rotation = MathUtils.Rotate(scale, this.rotation);
                Vector3 world = MathUtils.Translate(rotation, position);

                viewPoints.Add(new Vertex3D
                {
                    position = c.TransformPoint(world),
                    UV = localUVs[i]
                });
            }
            return viewPoints;
        }

        private Vertex3D[] ProjectToScreen(List<Vertex3D> clipped, Camera c)
        {
            Vertex3D[] result = new Vertex3D[clipped.Count];

            for (int i = 0; i < clipped.Count(); i++)
            {
                PointF newPos = c.ProjectPoint(clipped[i].position, Program.resolution);
                result[i] = new Vertex3D
                {
                    position = new Vector3(newPos.X, newPos.Y, clipped[i].position.Z),
                    UV = clipped[i].UV
                };
            }
            return result;
        }

        private Brush BuildTextureBrush(Vertex3D[] screenPoly)
        {
            PointF? p00 = null, p10 = null, p01 = null;
            foreach(var v in screenPoly)
            {
                if (v.UV.X == 0 && v.UV.Y == 0) p00 = new PointF(v.position.X, v.position.Y);
                else if (v.UV.X == 1 && v.UV.Y == 0) p10 = new PointF(v.position.X, v.position.Y);
                else if (v.UV.X == 0 && v.UV.Y == 1) p01 = new PointF(v.position.X, v.position.Y);
            }

            TextureBrush tb = new TextureBrush(fillTexture);
            tb.WrapMode = WrapMode.Clamp;

            if (p00.HasValue && p10.HasValue && p01.HasValue)
            {
                RectangleF sourceRect = new RectangleF(0, 0, fillTexture.Width, fillTexture.Height);
                PointF[] destPoints = { p00.Value, p10.Value, p01.Value };
                tb.Transform = new Matrix(sourceRect, destPoints);
            }
            else
            {
                tb.ResetTransform();

                float minX, maxX, minY, maxY;
                PointF[] positions = screenPoly.Select(v => new PointF(v.position.X, v.position.Y)).ToArray();
                ObtainMinMaxPoints(positions, out minX, out maxX, out minY, out maxY);
                float width = maxX - minX, height = maxY - minY;
                tb.TranslateTransform(minX, minY);

                if (width > 0 && height > 0)
                {
                    float scaleX = width / fillTexture.Width;
                    float scaleY = height / fillTexture.Height;
                    tb.ScaleTransform(scaleX, scaleY);
                }
            }

            return tb;
        }

        private void ObtainMinMaxPoints(PointF[] screenPoints, out float minX, out float maxX, out float minY, out float maxY)
        {
            if (screenPoints == null || screenPoints.Length == 0) 
            {
                minX = maxX = minY = maxY = 0; return;  
            }

            minX = maxX = screenPoints[0].X;
            minY = maxY = screenPoints[0].Y;

            for (int i = 1; i < screenPoints.Length; i++)
            {
                float x = screenPoints[i].X;
                float y = screenPoints[i].Y;

                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
            }

        }

        public override void Update()
        {
            
        }

        
    }
}
