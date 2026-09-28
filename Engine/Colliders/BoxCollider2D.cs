using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Colliders
{
    internal class BoxCollider2D : Collider
    {
        public BoxCollider2D(GameObject p) : base(p) {}

        public override bool isColliding(Collider other)
        {
            if (other is PointCollider point) return CollisionManager.ShapeToPoint(this, point);
            else return CollisionManager.ShapeToShape(this, other);
        }
    }
}
