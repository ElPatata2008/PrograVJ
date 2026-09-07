using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
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
        PointF[] localUVs;
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
            
            Brush b; Pen p;

            float halfX = size.X / 2;
            float halfY = size.Y / 2;
            localVertices = new Vector3[4]
            {
                new Vector3(-halfX, -halfY, 0f),
                new Vector3( halfX, -halfY, 0f),
                new Vector3( halfX,  halfY, 0f),
                new Vector3(-halfX,  halfY, 0f),
            };

            

            PointF[] screenPoints = new PointF[4];
            for (int i = 0; i < localVertices.Length; i++) {
                // S * R * T
                Vector3 scalePoint = MathUtils.Scale(localVertices[i], size);
                Vector3 rotationPoint = MathUtils.Rotate(scalePoint, rotation);
                Vector3 worldPoint = MathUtils.Translate(rotationPoint, position);

                Vector3 viewPoint = c.TransformPoint(worldPoint);
                screenPoints[i] = c.ProjectPoint(viewPoint, Program.resolution);
            }

            //for (int i = 0; i < localVertices.Length; i++)
            //{
            //    Vector3 scale = MathUtils.Scale(localVertices[i], size);
            //    Vector3 rotation = MathUtils.Rotate(scale, this.rotation);
            //    Vector3 world = MathUtils.Translate(rotation, position);

            //    viewPoints.Add(new Vertex3D
            //    {
            //        position = c.TransformPoint(world),
            //        UV = localUVs[i]
            //    });
            //}

            //List<Vertex3D> clippedPoints = ClipPolygon(viewPoints, c.nearZ);

            //for (int i = 0; i < clippedPoints.Count(); i++)
            //{
            //    polygonPoints[i] = new Vertex(c.ProjectPoint(clippedPoints[i].position, Program.resolution), clippedPoints[i].UV);
            //}

            //for (int i = 1; i < polygonPoints.Length - 1; i++)
            //{
            //    var vertices = polygonPoints.Position;
            //    var v0 = vertices[0], v1 = vertices[1], v2 = vertices[2];
            //    PointF[] triangle = { v0, v1, v2 };
            //    float area = triangleArea(v0, v1, v2);
            //    if (area < 0.001f) continue;

            //    float minU, minV, maxU, maxV;
            //    ObtainMinMaxPoints(screenPoints, out minU, out maxU, out minV, out maxV);
            //    float width = (maxU - minU) * fillTexture.Width;
            //    float height = (maxV - minV) * fillTexture.Height;
            //    RectangleF source = new RectangleF(
            //        minU * fillTexture.Width,
            //        minV * fillTexture.Height,
            //        width,
            //        height
            //    );
            //}

            if (fillTexture == null) b = new SolidBrush(fillColor);
            else
            {
                TextureBrush tb = new TextureBrush(fillTexture);
                tb.WrapMode = WrapMode.Clamp;
                tb.ResetTransform();

                //Matrix uvMatrix = new Matrix(source, triangle);
                //tb.Transform = uvMatrix;

                float minX, maxX, minY, maxY;
                ObtainMinMaxPoints(screenPoints, out minX, out maxX, out minY, out maxY);
                float width = maxX - minX, height = maxY - minY;
                tb.TranslateTransform(minX, minY);

                if (width > 0 && height > 0) {
                    float scaleX = width / fillTexture.Width;
                    float scaleY = height / fillTexture.Height;
                    tb.ScaleTransform(scaleX, scaleY);
                }

                b = tb;
            }
            p = new Pen(b, borderWidth);

            g.FillPolygon(b, screenPoints);
            g.DrawPolygon(p, screenPoints);

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
