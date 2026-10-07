using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
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
        private int phase = 1;
        private int type = 0;
        public Enemy(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor,
            float dir, int hp,
            Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
            this.hp = hp;
            maxHp = hp;
            body.velocity.X = dir == 0 ? 50 : -50;
            lastSpeed = body.velocity.X;
            type = hp > 1 ? 1 : 2;
        }

        public override void Update(float dt)
        {
            body.gravityScale = isGrounded ? 0 : 20;
            if (type == 1) fillTexture = TextureManager.Get("enemy1_phase" + phase);
        }

        public void CreateCollision() => collider = new BoxCollider2D(this);

        public override void OnCollisionEnter(GameObject c)
        {
            if (c is Player p)
            {
                if (p.body.velocity.Y < 0) { 
                    hp--;
                    phase++;
                    p.body.velocity.Y = 0;
                    p.body.AddForce(0, 300, 0);
                    if (hp > 1) body.velocity.X *= 1.5f;
                    AudioManager.PlaySFX("stomp");
                }
                else
                {
                    if (p.CanTakeDamage())
                    {
                        p.TakeDamage();
                    }
                }
            }
        }

        public void StopMovement() { lastSpeed = body.velocity.X; body.velocity.X = 0; }
        public void ContinueMovement() => body.velocity.X = lastSpeed;
    }
}
