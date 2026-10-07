using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Objects
{
    public class Enemy : Square
    {
        public bool isGrounded = false;
        public int hp = 1;
        public int maxHp = 0;
        public float lastSpeed = 0;
        public Enemy(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor,
            float dir, int hp,
            Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
            this.hp = hp;
            maxHp = hp;
            body.velocity.X = dir == 0 ? 3000 : -3000;
            lastSpeed = body.velocity.X;
        }

        public override void Update(float dt)
        {
            body.gravityScale = isGrounded ? 0 : 20;
        }

        public void CreateCollision() => collider = new BoxCollider2D(this);

        public override void OnCollisionEnter(GameObject c)
        {
            if (c is Player p)
            {
                if (p.body.velocity.Y < 0) { 
                    hp--;
                    p.body.velocity.Y = 0;
                    p.body.AddForce(0, 2200, 0);
                    if (hp > 1) body.velocity.X *= 1.5f;
                }
                else
                {
                    if (p.CanTakeDamage())
                    {
                        if (p.position.X < position.X) p.body.AddForce(-1000, 100, 0);
                        else if (p.position.X > position.X) p.body.AddForce(1000, 100, 0);
                        p.TakeDamage();
                    }
                }
            }
        }

        public void StopMovement() { lastSpeed = body.velocity.X; body.velocity.X = 0; }
        public void ContinueMovement() => body.velocity.X = lastSpeed;
    }
}
