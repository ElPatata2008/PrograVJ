using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization.Advanced;

namespace PrograVJ.Games.MP.Objects
{
    public class Parallax
    {
        List<Square> elements = new List<Square>();
        float limits = 0;
        public Parallax(Bitmap image, Vector3 pos, Vector3 size, float limit) {
            for (int i = -1; i < 2; i++) {
                Square s = new Square(
                    new Vector3(pos.X - i * size.X, pos.Y, 0),
                    Vector3.Zero,
                    size,
                    Color.White, fillColor: Color.White, fillTexture: image
                );

                elements.Add(s);
            }
            limits = limit;
        }

        public void UpdateParallax(float speed)
        {
            foreach (var s in elements) s.position.X -= speed;

            foreach (var s in elements)
            {
                if (s.position.X + s.size.X / 2 < limits)
                {
                    float maxX = elements.Max(e => e.position.X);
                    s.position.X = maxX + s.size.X;
                }
            }
        }

        public void RenderParallax(Graphics g, Camera c) { foreach (var s in elements) s.Draw(g, c); }

    }
}
