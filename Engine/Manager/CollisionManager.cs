using PrograVJ.Engine.Colliders;
using PrograVJ.GameObjects;
using PrograVJ.Games.MP.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Manager
{
    public static class CollisionManager
    {
        #region Shape To Shape Collisions

        public static bool ShapeToShape(Collider a, Collider b)
        {
            // Same Collider
            if (a is BoxCollider2D && b is BoxCollider2D) return Box2DToBox2D(a as BoxCollider2D, b as BoxCollider2D);
            if (a is CircleCollider && b is CircleCollider) return CircleToCircle(a as CircleCollider, b as CircleCollider);

            // Different Collider
            if (a is BoxCollider2D && b is CircleCollider) return BoxAndCircle(a as BoxCollider2D, b as CircleCollider);
            if (b is BoxCollider2D && a is CircleCollider) return BoxAndCircle(b as BoxCollider2D, a as CircleCollider);

            return true;
        }


        private static bool Box2DToBox2D(BoxCollider2D a, BoxCollider2D b)
        {
            bool inBoundsX = a.position.X + a.size.X / 2 >= b.position.X - b.size.X / 2
                          && a.position.X - a.size.X / 2 <= b.position.X + b.size.X / 2;
            bool inBoundsY = a.position.Y + a.size.Y / 2 >= b.position.Y - b.size.Y / 2
                          && a.position.Y - a.size.Y / 2 <= b.position.Y + b.size.Y / 2;
            return inBoundsX && inBoundsY;
        }

        private static bool CircleToCircle(CircleCollider a, CircleCollider b)
        {
            float x = a.position.X - b.position.X;
            float y = a.position.Y - b.position.Y;
            float dist = x * x + y * y;
            return dist <= a.radius + b.radius;
        }

        private static bool BoxAndCircle(BoxCollider2D a, CircleCollider b)
        {
            float closestX = MathUtils.Clamp(b.position.X, a.position.X - a.size.X / 2, a.position.X + a.size.X / 2);
            float closestY = MathUtils.Clamp(b.position.Y, a.position.Y - a.size.Y / 2, a.position.Y + a.size.Y / 2);

            float dx = b.position.X - closestX;
            float dy = b.position.Y - closestY;
            float dist = dx * dx + dy * dy, rad = b.radius * b.radius;

            return dist >= rad;
        }

        #endregion

        public static bool ShapeToPoint(Collider c, PointCollider p)
        {
            if (c is BoxCollider2D box)
            {
                bool inBoundsX = p.position.X <= box.position.X + box.size.X / 2 && p.position.X >= box.position.X - box.size.X / 2;
                bool inBoundsY = p.position.Y <= box.position.Y + box.size.Y / 2 && p.position.Y >= box.position.Y - box.size.Y / 2;
                return inBoundsX && inBoundsY;
            }
            if (c is CircleCollider circle)
            {
                float x = circle.position.X - p.position.X;
                float y = circle.position.Y - p.position.Y;
                float dist = x * x + y * y;
                return dist <= circle.radius;
            }
            return false;
        }

        public static bool PointToPoint(PointCollider a, PointCollider b) => a.position == b.position; 
    }
}
