using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Colliders
{
    public class CircleCollider : Collider
    {
        public float radius;
        public CircleCollider(GameObject p) : base(p)
        {
        }

        public override void Update()
        {
            base.Update();
            radius = parent.size.X;
        }
        public override bool isColliding(Collider other)
        {
            if (other is PointCollider) return CollisionManager.ShapeToShape(this, other);
            else return CollisionManager.ShapeToShape(this, other);
        }
    }
}
