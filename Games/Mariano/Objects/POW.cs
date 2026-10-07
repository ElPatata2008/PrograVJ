using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Objects
{
    public class POW : Square
    {
        public bool isActive = false;
        public bool gotActivated = false;
        Stopwatch sw = new Stopwatch();
        public POW(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture)
        {
            sw.Start();
        }

        public override void Update(float dt)
        {
            if (!isActive) {
                if (sw.ElapsedMilliseconds / 1000 > 10) {
                    isActive = true;
                    gotActivated = false;
                }
            }
        }

        public override void OnCollisionEnter(GameObject c)
        {
            if (c is Player p)
            {
                if (p.body.velocity.X > 0)
                {
                    if (p.position.Y < position.Y)
                    {
                        if (isActive)
                        {
                            sw.Restart();
                            isActive = false;
                            gotActivated = true;
                            p.body.velocity.Y = -100;
                        }
                    }
                }
            }
        }

    }
}
