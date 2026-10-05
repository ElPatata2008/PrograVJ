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
    public class Polygon : GameObject
    {
        List<Vector3> localVertices = new List<Vector3>();

        List<PointF> localUVs = new List<PointF>();
        List<Vertex3D> viewPoints = new List<Vertex3D>();
        float borderWidth;
        //float radius;
       
        public Color fillColor;
        public Polygon(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, 
            int vertices = 24, float border = 2f,
            Collider collider = null) : base(position, rotation, size, color, collider)
        {
            borderWidth = border;
            this.fillColor = fillColor;

            var step = (2 * Math.PI) / vertices;

            for (int i = 0; i < vertices; i++)
            {
                var angle = i * step;
                localVertices.Add(new Vector3(0.5f * (float)Math.Cos(angle), 0.5f * (float)Math.Sin(angle), 0f));
                localUVs.Add(new PointF(0.5f + 0.5f * (float)Math.Cos(angle), 0.5f + 0.5f * (float)Math.Sin(angle)));
            }
        }

        public override void Draw(Graphics g, Camera c)
        {
            List<Vertex3D> viewSpace = TransformToViewSpace(c);
            List<Vertex3D> clippedPoints = ClipPolygon(viewPoints, c.nearZ);

            if (clippedPoints.Count < 3) return;

            Vertex3D[] screenPoly = ProjectToScreen(clippedPoints, c);
            PointF[] screenPoints = screenPoly.Select(v => new PointF(v.position.X, v.position.Y)).ToArray();

            Brush b = new SolidBrush(fillColor);

            Pen p = new Pen(new SolidBrush(color), borderWidth);

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


        public override void OnColiisionStay(GameObject c) { }

        public override void OnCollisionEnter(GameObject c) { }

        public override void OnCollisionExit(GameObject c) { }

        public override void Update(float dt) { }
    }
}
