using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using PrograVJ.Engine;

namespace PrograVJ.GameObjects
{
    public abstract class GameObject
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 size;
        public Color color;
        public bool isActive;

        public GameObject(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive)
        {
            this.position = position;
            this.rotation = rotation;
            this.size = size;
            this.color = color;
            this.isActive = isActive;
        }

        public abstract void Update();
        public abstract void Draw(Graphics g, Camera c);

    }
}
