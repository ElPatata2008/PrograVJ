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
        Vector3 size;
        public ShapeCollider(GameObject p) : base(p) { }

        public override void Update()
        {
            base.Update();
            size = parent.size;
        }

        public override bool isColliding(Collider other)
        {
            return false;
        }
    }
}
