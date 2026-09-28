using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Colliders
{
    public abstract class Collider
    {
        public Vector3 position;
        public GameObject parent;

        public Collider(GameObject p)
        {
            parent = p;
        }

        public virtual void Update()
        {
            position = parent.position;
        }

        public abstract bool isColliding(Collider other);


    }
}
