using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.PunchOut.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.PunchOut.Objects
{
    public class Enemy : Square
    {
        int hp = 100;
        Stopwatch sw = new Stopwatch();

        int state = 0;

        public EnemyPunch punch;

        public Enemy(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            SpriteSheet idle = new SpriteSheet(TextureManager.Get("gjIdle"), 33, 113, 0.15f, 10);
            this.animationController.AddAnimation("idle", idle);

            SpriteSheet attack = new SpriteSheet(TextureManager.Get("gjAttack"), 33, 113, 0.5f, 5);
            attack.AddEvent(2, () => punch.active = true );
            attack.AddEvent(3, () => punch.active = false);
            attack.AddEvent(4, () =>
            {
                this.animationController.Play("idle");
                state = 0;
                sw.Restart();
            });
            this.animationController.AddAnimation("attack", attack);

            SpriteSheet block = new SpriteSheet(TextureManager.Get("gjBlock"), 41, 113, 1.5f, 5);
            this.animationController.AddAnimation("block", block);

            this.animationController.Play("idle");
            sw.Start();
        }

        public override void Update(float dt)
        {
            animationController.Update(dt);
            punch.Update(dt);
            switch (state)
            {
                case 0: { 
                        if (sw.ElapsedMilliseconds / 1000 > 3)
                        {
                            Console.WriteLine("Attack");
                            state = 1;
                            animationController.Play("attack");
                            sw.Restart();
                        }
                    } break;
                case 1: {

                    } break;
            }
        }

        public override void Draw(Graphics g, Camera c)
        {
            base.Draw(g, c);

            if (punch.active) punch.Draw(g, c);
        }
    }
}
