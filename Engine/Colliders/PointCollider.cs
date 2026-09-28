using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Colliders
{
    public class PointCollider : Collider
    {
        public PointCollider(GameObject p) : base(p)
        {
        }

        public override bool isColliding(Collider other)
        {
            if (other is PointCollider point) return CollisionManager.PointToPoint(this, point);
            else return CollisionManager.ShapeToPoint(other, this);
        }
    }
}
