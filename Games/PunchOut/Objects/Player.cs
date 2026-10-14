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
        int hp = 100;
        int state = 0;
        Stopwatch sw = new Stopwatch();

        public bool dl = false;
        public bool dr = false;
        public bool block = false;

        bool move = false;
        bool blocking = false;

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

            this.animationController.Play("idle");
        }

        public override void Update(float dt)
        {
            animationController.Update(dt);

            switch (state)
            {
                case 0: { // Idle
                        if (dl) DodgeLeft();
                        if (dr) DodgeRight();
                        if (block) Block();
                    } break;
                case 1: { // Hurt
                        if (sw.ElapsedMilliseconds / 1000 > 0.5) {
                            sw.Reset();
                            state = 0;
                            animationController.Play("idle");
                        }
                    } break;
                case 2: { // Dodge Left
                        move = sw.ElapsedMilliseconds / 1000 < 1;
                        if (move) position.X = -dodgeOffset;
                        else position.X = 0;
                        if (!move)
                        {
                            state = 0;
                            animationController.Play("idle");
                            sw.Reset();
                        }
                    } break;
                case 3: { // DodgeRight
                        move = sw.ElapsedMilliseconds / 1000 < 1;
                        if (move) position.X = dodgeOffset;
                        else position.X = 0;
                        if (!move)
                        {
                            state = 0;
                            animationController.Play("idle");
                            sw.Reset();
                        }
                    } break;
                case 4: {
                        blocking = sw.ElapsedMilliseconds / 1000 < 0.5f;
                        if (!blocking)
                        {
                            state = 0;
                            animationController.Play("idle");
                            sw.Reset();
                        }
                    } break;
            }
        }

        public override void OnColiisionStay(GameObject c)
        {
            if (c is EnemyPunch ep)
            {
                if (ep.active)
                {
                    if (!blocking)
                    {
                        Hit();
                    }
                }
            }
        }

        private void DodgeLeft()
        {
            if (state != 2)
            {
                state = 2;
                sw.Start();
                animationController.Play("dodgeL");
            }
        }

        private void DodgeRight()
        {
            if (state != 3)
            {
                state = 3;
                sw.Start();
                animationController.Play("dodgeR");
            }
        }

        private void Block()
        {
            if (state != 4)
            {
                state = 4;
                sw.Start();
                animationController.Play("block");
            }
        }

        private void Hit()
        {
            if (state != 1) 
            {
                state = 1;
                sw.Start();
                hp -= 10;
                Console.WriteLine($"HP: {hp}");
                animationController.Play("hurt");
            }
        }
    }
}
