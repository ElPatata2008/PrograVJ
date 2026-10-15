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
        public int hp = 100;
        Stopwatch sw = new Stopwatch();
        Stopwatch spatk = new Stopwatch();

        int state = 0;

        public EnemyPunch punch;
        bool hit = false;
        public bool gameover = false;

        public Enemy(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            SpriteSheet idle = new SpriteSheet(TextureManager.Get("gjIdle"), 33, 113, 0.15f, 10);
            this.animationController.AddAnimation("idle", idle);

            SpriteSheet attackStart = new SpriteSheet(TextureManager.Get("gjAttackStart"), 33, 113, 0.75f, 2);
            attackStart.AddEvent(1, () =>
            {
                if (spatk.ElapsedMilliseconds / 1000 > 10)
                {
                    ChangeSize(94);
                    this.animationController.Play("attackSpecial");
                }
                else { this.animationController.Play("attack"); punch.active = true; punch.strong = false; }

            });
            this.animationController.AddAnimation("attackStart", attackStart);

            SpriteSheet attack = new SpriteSheet(TextureManager.Get("gjAttack"), 33, 113, 0.5f, 3);
            attack.AddEvent(1, () => punch.active = false);
            attack.AddEvent(2, () =>
            {
                state = 0;
                sw.Restart();
                this.animationController.Play("idle");
            });
            this.animationController.AddAnimation("attack", attack);

            SpriteSheet attackSpecial = new SpriteSheet(TextureManager.Get("gjAttackSpecial"), 47, 113, 0.1f, 5);
            attackSpecial.AddEvent(1, () => { punch.active = true; punch.strong = true; });
            attackSpecial.AddEvent(2, () => punch.active = false);
            attackSpecial.AddEvent(4, () =>
            {
                state = 0;
                ChangeSize(66);
                sw.Restart();
                spatk.Restart();
                this.animationController.Play("idle");
            });
            this.animationController.AddAnimation("attackSpecial", attackSpecial);

            SpriteSheet block = new SpriteSheet(TextureManager.Get("gjBlock"), 41, 113, 0.5f, 3);
            block.AddEvent(2, () =>
            {
                state = 0;
                this.animationController.Play("idle");
            });
            this.animationController.AddAnimation("block", block);

            SpriteSheet hitLight = new SpriteSheet(TextureManager.Get("gjHitLight"), 33, 113, 1f, 1);
            this.animationController.AddAnimation("hitLight", hitLight);

            SpriteSheet hitHard = new SpriteSheet(TextureManager.Get("gjHitHard"), 36, 113, 1f, 1);
            this.animationController.AddAnimation("hitHard", hitHard);

            SpriteSheet defeat = new SpriteSheet(TextureManager.Get("gjDefeat"), 56, 113, 0.75f, 3);
            defeat.AddEvent(0, () => {
                ChangePosition(10, 20);
                AudioManager.PlaySFX("enemyDefeat");
            });
            defeat.AddEvent(1, () => ChangePosition(10, 20));
            defeat.AddEvent(2, () =>
            {
                ChangePosition(10, 20);
                AudioManager.PlaySFX("enemyDefeated");
                this.animationController.Play("defeated");
            });
            this.animationController.AddAnimation("defeat", defeat);

            SpriteSheet defeated = new SpriteSheet(TextureManager.Get("gjDefeated"), 56, 113, 1f, 1);
            this.animationController.AddAnimation("defeated", defeated);

            this.animationController.Play("idle");
            
            spatk.Start();
        }

        public void Hit(bool strong, bool block)
        {
            if (!hit)
            {
                hit = true;
                if (block)
                {
                    if (state != 3)
                    {
                        ChangeSize(82);
                        state = 3;
                        punch.active = false;
                        animationController.Play("block");
                    }
                }
                else
                {
                    if (state != 1 && state != 2)
                    {
                        state = 2;
                        if (strong)
                        {
                            ChangeSize(72);
                            hp -= 15;
                            punch.active = false;
                            AudioManager.PlaySFX("strongHit");
                            animationController.Play("hitHard");
                        }
                        else
                        {
                            ChangeSize(66);
                            hp -= 5;
                            punch.active = false;
                            AudioManager.PlaySFX("enemyHurt");
                            animationController.Play("hitLight");
                        }
                        //sw.Restart();
                        Console.WriteLine($"{hp}");
                    }
                }
            }
        }

        public void ChangeSize(float X) => size.X = X;
        public void ChangePosition(float X, float Y)
        {
            position.X = X;
            position.Y = Y;
        }
        public override void Update(float dt)
        {
            animationController.Update(dt);
            punch.Update(dt);
            if (!gameover)
            {
                switch (state)
                {
                    case 0:
                        { // Idle
                            ChangeSize(66);
                            hit = false;
                            if (sw.ElapsedMilliseconds / 1000 > 1)
                            {
                                Console.WriteLine("Attack");
                                state = 1;
                                animationController.Play("attackStart");
                                sw.Restart();
                            }
                        }
                        break;
                    case 1: { } break;
                    case 2:
                        { // Hit
                            if (sw.ElapsedMilliseconds / 1000 > 0.2)
                            {
                                state = 0;
                                ChangeSize(66);
                                punch.active = false;
                                if (hp <= 0)
                                {
                                    state = 4;
                                    ChangeSize(112);
                                    gameover = true;
                                    animationController.Play("defeat");
                                }
                                else
                                {
                                    state = 0;
                                    hit = false;
                                    animationController.Play("idle");
                                }
                                sw.Restart();
                            }
                        }
                        break;
                    case 3:
                        { // Block
                            if (sw.ElapsedMilliseconds / 1000 > 1)
                            {
                                state = 0;
                                ChangeSize(33);
                                sw.Restart();
                                hit = false;
                                animationController.Play("idle");
                            }
                        }
                        break;
                    case 4: { } break;
                }
            }
        }

        public void StartTime() => sw.Start();
        public override void Draw(Graphics g, Camera c)
        {
            base.Draw(g, c);

            if (punch.active) punch.Draw(g, c);
        }
    }
}
