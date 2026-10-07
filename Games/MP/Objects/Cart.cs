using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.MP.Objects
{
    public class Cart : Enemy
    {
        Stopwatch sw = new Stopwatch();

        public Cart(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
            hp = 15;
            sw.Start();
        }

        public override void Update(float dt)
        {
            position.X += speed * dt;
        }

        public bool CanAttack() => sw.ElapsedMilliseconds / 1000 > 5;

        public Bullet Attack()
        {
            sw.Restart();
            return new Bullet(
            position,
            Vector3.Zero,
            new Vector3(15, 10, 0),
            Color.Red, Color.Red
            ) { xSpeed = -4f, enemy = true };
        }
    }
}
