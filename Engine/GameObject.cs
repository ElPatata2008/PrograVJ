using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;

namespace PrograVJ.GameObjects
{
    public abstract class GameObject
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 size;
        public Color color;
        public Collider collider;

        public GameObject(Vector3 position, Vector3 rotation, Vector3 size, Color color, Collider collider = null)
        {
            this.position = position;
            this.rotation = rotation;
            this.size = size;
            this.color = color;
            this.collider = collider;
            if (this.collider != null) this.collider.parent = this;
        }

        public abstract void Update();
        public abstract void Draw(Graphics g, Camera c);

        public bool isColliding(GameObject other)
        {
            return collider.isColliding(other.collider);
        }

    }
}
