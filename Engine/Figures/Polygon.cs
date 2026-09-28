using NAudio.CoreAudioApi;
using PrograVJ.Engine.Colliders;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Figures
{
    //public struct Vertex3D
    //{
    //    public Vector3 position;
    //    public PointF UV;
    //}
    public class Polygon : GameObject
    {
        //PointF? p00 = null, p10 = null, p01 = null;
        //Vector3[] localVertices;
        List<Vector3> localVertices = new List<Vector3>();
        //PointF[] localUVs = new PointF[] {
        //    new PointF(0, 1),
        //    new PointF(1, 1),
        //    new PointF(1, 0),
        //    new PointF(0, 0)
        //};
        List<PointF> localUVs = new List<PointF>();
        List<Vertex3D> viewPoints = new List<Vertex3D>();
        float borderWidth;
        //float radius;
       
        Color fillColor;
        Bitmap fillTexture;
        public Polygon(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, 
            int vertices = 24, float border = 2f, Bitmap fillTexture = null,
            Collider collider = null) : base(position, rotation, size, color, collider)
        {
            //radius = size.X / 2;
            //radius = size.X * (float)Math.Sqrt(2) / 2;
            borderWidth = border;
            this.fillColor = fillColor;
            this.fillTexture = fillTexture;

            var step = (2 * Math.PI) / vertices;

            for (int i = 0; i < vertices; i++)
            {
                var angle = i * step;
                localVertices.Add(new Vector3(0.5f * (float)Math.Cos(angle), 0.5f * (float)Math.Sin(angle), 0f));
                localUVs.Add(new PointF(0.5f + 0.5f * (float)Math.Cos(angle), 0.5f + 0.5f * (float)Math.Sin(angle)));
                Console.WriteLine($"{i} | Local Vértice: {localVertices[i]} | Local UV: {localUVs[i]}");
            }
        }

        public override void Draw(Graphics g, Camera c)
        {
            //localVertices = new Vector3[4]
            //{
            //    new Vector3(-0.5f, -0.5f, 0f),
            //    new Vector3( 0.5f, -0.5f, 0f),
            //    new Vector3( 0.5f,  0.5f, 0f),
            //    new Vector3(-0.5f,  0.5f, 0f),
            //};

            List<Vertex3D> viewSpace = TransformToViewSpace(c);
            List<Vertex3D> clippedPoints = ClipPolygon(viewPoints, c.nearZ);

            if (clippedPoints.Count < 3) return;

            Vertex3D[] screenPoly = ProjectToScreen(clippedPoints, c);
            PointF[] screenPoints = screenPoly.Select(v => new PointF(v.position.X, v.position.Y)).ToArray();

            //Brush b = fillTexture == null
            //    ? new SolidBrush(fillColor)
            //    : BuildTextureBrush(screenPoly);
            Brush b = new SolidBrush(fillColor);

            Pen p = new Pen(fillTexture != null ? b : new SolidBrush(color), borderWidth);

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
            for (int i = 0; i < localVertices.Count; i++)
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

        //private Brush BuildTextureBrush(Vertex3D[] screenPoly)
        //{
        //    //PointF? p00 = null, p10 = null, p01 = null;
        //    foreach (var v in screenPoly)
        //    {
        //        if (v.UV.X == 0 && v.UV.Y == 0) p00 = new PointF(v.position.X, v.position.Y);
        //        else if (v.UV.X == 1 && v.UV.Y == 0) p10 = new PointF(v.position.X, v.position.Y);
        //        else if (v.UV.X == 0 && v.UV.Y == 1) p01 = new PointF(v.position.X, v.position.Y);
        //    }

        //    TextureBrush tb = new TextureBrush(fillTexture)
        //    {
        //        WrapMode = WrapMode.Clamp
        //    };

        //    if (p00.HasValue && p10.HasValue && p01.HasValue)
        //    {
        //        RectangleF sourceRect = new RectangleF(0, 0, fillTexture.Width, fillTexture.Height);
        //        PointF[] destPoints = { p00.Value, p10.Value, p01.Value };
        //        tb.Transform = new Matrix(sourceRect, destPoints);
        //    }
        //    else
        //    {
        //        tb.ResetTransform();

        //        float minX, maxX, minY, maxY;
        //        PointF[] positions = screenPoly.Select(v => new PointF(v.position.X, v.position.Y)).ToArray();
        //        ObtainMinMaxPoints(positions, out minX, out maxX, out minY, out maxY);
        //        float width = maxX - minX, height = maxY - minY;
        //        tb.TranslateTransform(minX, minY);

        //        if (width > 0 && height > 0)
        //        {
        //            float scaleX = width / fillTexture.Width;
        //            float scaleY = height / fillTexture.Height;
        //            tb.ScaleTransform(scaleX, scaleY);
        //        }
        //    }


        //    return tb;
        //}

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

        public override void Update() { }
    }
}
