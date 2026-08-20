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
            float cosZ = (float)Math.Cos(radZ), sinZ = (float)Math.Sin(radZ);

            // 1 Rotar Z
            float x1 = point.X * cosZ - point.Y * sinZ;
            float y1 = point.X * sinZ + point.Y * cosZ;
            float z1 = point.Z;

            // 2 Rotar X

            // 3 Rotar Y

            return new Vector3(x1, y1, z1);
        }
    }
}
