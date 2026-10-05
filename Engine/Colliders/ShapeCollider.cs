using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Colliders
{
    public class ShapeCollider : Collider
    {
        public ShapeCollider(GameObject p) : base(p) { }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public override bool isColliding(Collider other)
        {
            if (other is PointCollider point) return CollisionManager.ShapeToPoint(this, point);
            else return CollisionManager.ShapeToShape(this, other);
        }
    }
}
