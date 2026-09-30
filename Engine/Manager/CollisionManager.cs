using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.GameObjects;
using PrograVJ.Games.MP.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
            if (a is CircleCollider2D && b is CircleCollider2D) return CircleToCircle(a as CircleCollider2D, b as CircleCollider2D);

            // Box and Circle
            if (a is BoxCollider2D && b is CircleCollider2D) return BoxAndCircle(a as BoxCollider2D, b as CircleCollider2D);
            if (b is BoxCollider2D && a is CircleCollider2D) return BoxAndCircle(b as BoxCollider2D, a as CircleCollider2D);

            //

            return true;
        }


        private static bool Box2DToBox2D(BoxCollider2D a, BoxCollider2D b)
        {
            float rotA = a.rotation.Z * ((float)Math.PI / 180f), rotB = b.rotation.Z * ((float)Math.PI / 180f);
            List<Vector3> axis = new List<Vector3>()
            {
                new Vector3((float)Math.Cos(rotA), (float)Math.Sin(rotA), 0),  // AX
                new Vector3((float)-Math.Sin(rotA), (float)Math.Cos(rotA), 0), // AY
                new Vector3((float)Math.Cos(rotB), (float)Math.Sin(rotB), 0),  // BX
                new Vector3((float)-Math.Sin(rotB), (float)Math.Cos(rotB), 0)  // BY
            };
            
            Vector3 d = a.position - b.position;

            foreach(var L in axis)
            {
                float rA = (a.size.X / 2) * Math.Abs(MathUtils.Dot(axis[0], L)) + (a.size.Y / 2) * Math.Abs(MathUtils.Dot(axis[1], L));
                float rB = (b.size.X / 2) * Math.Abs(MathUtils.Dot(axis[2], L)) + (b.size.Y / 2) * Math.Abs(MathUtils.Dot(axis[3], L));
                if (Math.Abs(MathUtils.Dot(d, L)) > rA + rB) return false;
            }

            return true;
        }

        private static bool CircleToCircle(CircleCollider2D a, CircleCollider2D b)
        {
            float x = a.position.X - b.position.X;
            float y = a.position.Y - b.position.Y;
            float dist = x * x + y * y;
            return dist <= a.radius + b.radius;
        }

        private static bool BoxAndCircle(BoxCollider2D box, CircleCollider2D circle)
        {
            Vector3 localC = circle.position; //MathUtils.Rotate(circle.position - box.position, Vector3.Zero);

            float closestX = MathUtils.Clamp(localC.X, box.position.X - box.size.X / 2, box.position.X + box.size.X / 2);
            float closestY = MathUtils.Clamp(localC.Y, box.position.Y - box.size.Y / 2, box.position.Y + box.size.Y / 2);

            float dx = localC.X - closestX;
            float dy = localC.Y - closestY;
            float dist = dx * dx + dy * dy, rad = circle.radius * circle.radius;

            return dist < rad;
        }

        #endregion

        public static bool ShapeToPoint(Collider c, PointCollider p)
        {
            if (c is BoxCollider2D box)
            {
                Vector3 localP = MathUtils.Rotate(p.position - box.position, -box.rotation);

                bool inBoundsX = localP.X <= box.position.X + box.size.X / 2 && localP.X >= box.position.X - box.size.X / 2;
                bool inBoundsY = localP.Y <= box.position.Y + box.size.Y / 2 && localP.Y >= box.position.Y - box.size.Y / 2;
                return inBoundsX && inBoundsY;
            }
            if (c is CircleCollider2D circle)
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
