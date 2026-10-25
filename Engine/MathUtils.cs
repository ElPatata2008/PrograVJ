using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Engine
{
    public static class MathUtils
    {
        public static float GetAngleFrom2Points(Vector3 a, Vector3 b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            double rad = Math.Atan2(dy, dx);
            double deg = rad * (180.0 / Math.PI);
            return (float)((deg < 0) ? deg += 360.0 : deg);
        }
        public static Vector2 GetDirectionFromAngle(float rotationZ)
        {
            double radians = rotationZ * (Math.PI / 180);

            double x = Math.Cos(radians);
            double y = Math.Sin(radians);

            return new Vector2((float)x, (float)y);
        }

        public static float GetAngleFromDirection(float x, float y)
        {
            double rad = Math.Atan2(y, x);
            double deg = rad * (180.0 / Math.PI);
            return (float)(deg < 0 ? deg += 360.0f : deg);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }

        public static PointF LerpPointF(PointF a, PointF b, float t) 
            => new PointF(
                a.X + (b.X - a.X) * t, 
                a.Y + (b.Y - a.Y) * t
            );
        public static Vector3 LerpVector3(Vector3 a, Vector3 b, float t) 
            => new Vector3(
                a.X + (b.X - a.X) * t, 
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t
            );
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
               

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vector3 Scale(Vector3 src, Vector3 scale)
        {
            float x = src.X * scale.X;
            float y = src.Y * scale.Y;
            float z = src.Z * scale.Z;
            return new Vector3(x, y, z);
        }

        public static Vector3 Rotate(Vector3 src, Vector3 rotation)
        {
            float radX = rotation.X * ((float)Math.PI / 180f);
            float radY = rotation.Y * ((float)Math.PI / 180f);
            float radZ = rotation.Z * ((float)Math.PI / 180f);
            float cosX = (float)Math.Cos(radX), sinX = (float)Math.Sin(radX);
            float cosY = (float)Math.Cos(radY), sinY = (float)Math.Sin(radY);
            float cosZ = (float)Math.Cos(radZ), sinZ = (float)Math.Sin(radZ);

            //// 1 Rotar Z
            float x1 = src.X * cosZ - src.Y * sinZ;
            float y1 = src.X * sinZ + src.Y * cosZ;
            float z1 = src.Z;

            // 2 Rotar X
            float x2 = x1;
            float y2 = y1 * cosX - z1 * sinX;
            float z2 = y1 * sinX + z1 * cosX;

            // 3 Rotar Y
            float x3 = x2 * cosY + z2 * sinY;
            float y3 = y2;
            float z3 = -x2 * sinY + z2 * cosY;

            return new Vector3(x3, y3, z3);
        }

        public static Vector3 Translate(Vector3 src, Vector3 translate) => src + translate;
    }
}
