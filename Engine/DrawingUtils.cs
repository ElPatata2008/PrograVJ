using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine
{
    public static class DrawingUtils
    {
        public static void DrawCenterText(Graphics g, string text, Font font, Color color, PointF pos)
        {
            SizeF sizeStart = g.MeasureString(text, font);
            g.DrawString(text, font, new SolidBrush(color), pos.X - sizeStart.Width / 2, pos.Y);
        }

    }
}
