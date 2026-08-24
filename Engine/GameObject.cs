using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace PrograVJ.GameObjects
{
    public abstract class GameObject
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 size;
        public Color color;
        public bool isActive;

        public GameObject(Vector3 position, Vector3 rotation, Vector3 size, Color color)
        {
            this.position = position;
            this.rotation = rotation;
            this.size = size;
            this.color = color;
        }

        public abstract void Update();
        public abstract void Draw(Graphics g);

        public Vector3 RotatePoint(Vector3 point)
        {
            float radZ = rotation.Z * ((float)Math.PI / 180f);
            float radX = rotation.X * ((float)Math.PI / 180f);
            float radY = rotation.Y * ((float)Math.PI / 180f);
            float cosZ = (float)Math.Cos(radZ), sinZ = (float)Math.Sin(radZ);
            float cosX = (float)Math.Cos(radX), sinX = (float)Math.Sin(radX);
            float cosY = (float)Math.Cos(radY), sinY = (float)Math.Sin(radY);

            // 1 Rotar Z
            float x1 = point.X * cosZ + point.Y * -sinZ;
            float y1 = point.X * sinZ + point.Y * cosZ;
            float z1 = point.Z;

            // 2 Rotar X
            float x2 = x1 * cosX + z1 * sinX;
            float y2 = y1;
            float z2 = z1 * -sinX + z1 * cosX;

            // 3 Rotar Y
            float x3 = x2;
            float y3 = y2 * cosY + y2 * -sinY;
            float z3 = z2 * sinY + z2 * cosY;

            return new Vector3(x3, y3, z3);
        }
    }
}
