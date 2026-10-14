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
        public Rigidbody body;

        private List<GameObject> collidingObjects = new List<GameObject>();
        private List<GameObject> prevCollidingObejcts = new List<GameObject>();

        public GameObject(Vector3 position, Vector3 rotation, Vector3 size, Color color, 
            Collider collider = null, Rigidbody body = null)
        {
            this.position = position;
            this.rotation = rotation;
            this.size = size;
            this.color = color;
            this.collider = collider;
            if (this.collider != null) this.collider.parent = this;
            this.body = body;
            if (this.body != null) this.body.parent = this;
        }

        public abstract void Update(float dt);
        public abstract void Draw(Graphics g, Camera c);

        public bool isColliding(GameObject other) { return collider.isColliding(other.collider); }
        public abstract void OnColiisionStay(GameObject c);
        public abstract void OnCollisionEnter(GameObject c);
        public abstract void OnCollisionExit(GameObject c);
        public void PhysicsUpdate(float dt, List<GameObject> candidates)
        {
            collidingObjects = new List<GameObject>();

            foreach (var c in candidates)
            {
                if (c == this) continue;
                if (isColliding(c))
                {
                    collidingObjects.Add(c);
                    if (prevCollidingObejcts.Contains(c)) OnColiisionStay(c);
                    else OnCollisionEnter(c);
                }
            }

            foreach(var po in prevCollidingObejcts)
            {
                if (!collidingObjects.Contains(po)) OnCollisionExit(po);
            } 
            prevCollidingObejcts = new List<GameObject>(collidingObjects);
        }
    }
}
