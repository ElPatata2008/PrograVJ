using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
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
    public class Player : Square
    {
        float dodgeOffset = 50f;
        public int hp = 100;
        int state = 0;
        Stopwatch sw = new Stopwatch();
        public Stopwatch strongatk = new Stopwatch();

        public bool dl = false;
        public bool dr = false;
        public bool block = false;
        public bool atk1 = false;
        public bool atk2 = false;

        bool move = false;
        public bool blocking = false;

        public PlayerPunch punch;
        public bool gameover = false;

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            SpriteSheet idle = new SpriteSheet(TextureManager.Get("lmIdle"), 25, 89, 0.2f, 2);
            this.animationController.AddAnimation("idle", idle);

            SpriteSheet hurt = new SpriteSheet(TextureManager.Get("lmHurt"), 25, 89, 1f, 1);
            this.animationController.AddAnimation("hurt", hurt);

            SpriteSheet dodgeLeft = new SpriteSheet(TextureManager.Get("lmDL"), 25, 89, 1f, 2);
            this.animationController.AddAnimation("dodgeL", dodgeLeft);

            SpriteSheet dodgeRight = new SpriteSheet(TextureManager.Get("lmDR"), 25, 89, 1f, 2);
            this.animationController.AddAnimation("dodgeR", dodgeRight);

            SpriteSheet block = new SpriteSheet(TextureManager.Get("lmBlock"), 25, 89, 1f, 2);
            this.animationController.AddAnimation("block", block);

            SpriteSheet attackLight = new SpriteSheet(TextureManager.Get("lmAttackLight"), 33, 89, 0.05f, 5);
            attackLight.AddEvent(3, () =>
            {
                punch.active = true;
                punch.position.X = position.X + 10;
                punch.strong = false;
            });
            attackLight.AddEvent(4, () =>
            {
                punch.active = false;
                ChangeSize(50);
                state = 0;
                this.animationController.Play("idle");
            });
            this.animationController.AddAnimation("attackLight", attackLight);

            SpriteSheet attackHard = new SpriteSheet(TextureManager.Get("lmAttackHard"), 31, 89, 0.1f, 7);
            attackHard.AddEvent(5, () =>
            {
                punch.active = true;
                punch.position.X = position.X - 10;
                punch.strong = true;
            });
            attackHard.AddEvent(6, () =>
            {
                punch.active = false;
                ChangeSize(50);
                state = 0;
                strongatk.Restart();
                this.animationController.Play("idle");
            });
            this.animationController.AddAnimation("attackHard", attackHard);

            SpriteSheet defeat = new SpriteSheet(TextureManager.Get("lmDefeat"), 33, 89, 1f, 1);
            this.animationController.AddAnimation("defeat", defeat);

            SpriteSheet win = new SpriteSheet(TextureManager.Get("lmWin"), 25, 89, 0.3f, 2);
            this.animationController.AddAnimation("win", win);

            this.animationController.Play("idle");
        }

        public void ChangeSize(float X) => size.X = X;
        public override void Update(float dt)
        {
            animationController.Update(dt);
            punch.Update(dt);

            if (!gameover)
            {
                switch (state)
                {
                    case 0: { // Idle
                            if (dl) DodgeLeft();
                            if (dr) DodgeRight();
                            if (block) Block();
                            if (atk1) Attack1();
                            if (atk2) Attack2();
                        }
                        break;
                    case 1: { // Hurt
                            if (sw.ElapsedMilliseconds / 1000 > 0.5)
                            {
                                ChangeSize(50);
                                punch.active = false;
                                if (hp <= 0)
                                {
                                    state = 7;
                                    ChangeSize(66);
                                    gameover = true;
                                    AudioManager.PlaySFX("playerDefeat");
                                    animationController.Play("defeat");
                                }
                                else
                                {
                                    state = 0;
                                    animationController.Play("idle");
                                }
                                sw.Reset();
                            }
                        }
                        break;
                    case 2: 
                        { // Dodge Left
                            move = sw.ElapsedMilliseconds / 1000 < 1;
                            if (move) position.X = -dodgeOffset;
                            else position.X = 0;
                            if (!move)
                            {
                                state = 0;
                                blocking = false;
                                animationController.Play("idle");
                                sw.Reset();
                            }
                        }
                        break;
                    case 3:
                        { // DodgeRight
                            move = sw.ElapsedMilliseconds / 1000 < 1;
                            if (move) position.X = dodgeOffset;
                            else position.X = 0;
                            if (!move)
                            {
                                state = 0;
                                blocking = false;
                                animationController.Play("idle");
                                sw.Reset();
                            }
                        }
                        break;
                    case 4:
                        { // Block
                            if (sw.ElapsedMilliseconds / 1000 > 0.5f)
                            {
                                state = 0;
                                sw.Reset();
                                blocking = false;
                                animationController.Play("idle");
                            }
                        }
                        break;
                    case 5:
                        { // Attack1

                        }
                        break;
                    case 6:
                        { // Attack2

                        }
                        break;
                    case 7: { } break;
                }
            }
        }

        private void DodgeLeft()
        {
            if (state != 2)
            {
                state = 2;
                sw.Start();
                blocking = true;
                animationController.Play("dodgeL");
            }
        }

        private void DodgeRight()
        {
            if (state != 3)
            {
                state = 3;
                sw.Start();
                blocking = true;
                animationController.Play("dodgeR");
            }
        }

        private void Block()
        {
            if (state != 4)
            {
                state = 4;
                sw.Restart();
                blocking = true;
                animationController.Play("block");
            }
        }

        public void Hit(int dmg)
        {
            if (state != 1) 
            {
                state = 1;
                sw.Start();
                hp -= dmg ;
                AudioManager.PlaySFX("playerHurt");
                animationController.Play("hurt");
            }
        }

        private void Attack1()
        {
            if (state != 5)
            {
                state = 5;
                ChangeSize(66);
                animationController.Play("attackLight");
            }
        }

        private void Attack2()
        {
            if (strongatk.ElapsedMilliseconds / 1000 > 4)
            {
                if (state != 6)
                {
                    state = 6;
                    ChangeSize(62);
                    animationController.Play("attackHard");
                }
            }
        }

        public void StartTimer() => strongatk.Start();
        public override void Draw(Graphics g, Camera c)
        {
            if (punch.active) punch.Draw(g, c);
            base.Draw(g, c);
        }
    }
}
