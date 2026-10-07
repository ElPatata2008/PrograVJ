using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Objects
{
    public class Terrain : Square
    {
        public Terrain(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture, mode, repeatTileX, repeatTileY)
        {
        }

        public override void OnCollisionEnter(GameObject c)
        {
            bool inBoundsX = c.position.X + c.size.X >= position.X - size.X && c.position.X - c.size.X <= position.X + size.X;
            if (c is Player p)
            {
                if (inBoundsX)
                {
                    if (p.position.Y > position.Y) { 
                        p.isGrounded = true;
                        p.body.gravityScale = 0;
                        p.body.velocity = new Vector3(p.body.velocity.X, 0, p.body.velocity.Z);
                        p.position.Y = position.Y + size.Y / 2 + p.size.Y / 2;
                    }
                    else if (p.position.Y < position.Y)
                    {
                        p.isGrounded = false;
                        p.body.velocity = new Vector3(p.body.velocity.X, -100, p.body.velocity.Z);
                        p.position.Y = position.Y - size.Y / 2 - p.size.Y / 2;
                    }
                }
            }
            if (c is Enemy e)
            {
                if (inBoundsX)
                {
                    if (e.position.Y > position.Y)
                    {
                        e.isGrounded = true;
                        e.body.gravityScale = 0;
                        e.body.velocity.Y = 0;
                        e.position.Y = position.Y + size.Y / 2 + e.size.Y / 2;
                    }
                }
            }
        }

        public override void OnCollisionExit(GameObject c)
        {
            if (c is Player p)
            {
                if (p.position.Y > position.Y) p.isGrounded = false;
            }
            if (c is Enemy e)
            {
                if (e.position.Y > position.Y) e.isGrounded = false;
            }
        }
    }
}
