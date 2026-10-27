using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class Player : Square
    {
        public int hp = 400;
        public bool isGrounded = false;
        public bool jumped = false;
        public bool doubleJumped = false;
        public float speed = 20f;
        public float maxSpeed = 150f;
        float knockback = 400f;
        bool facingRight = false;

        bool dash = false;
        float dashCD = 0;
        bool dashRWindowActive = false;
        float dashRtimer = 0;
        bool dashLWindowActive = false;
        float dashLtimer = 0;
        float dashSpeed = 500f;

        float iFrames = 0;
        public float maxiFrames = 0.67f;
        public bool panic = false;
        public float panicTimer = 0;
        public float panicSpeed = 20f;
        public float panicMaxSpeed = 150f;
        public float attackDelay = 0;

        public int regen = 1;
        float regenTimer = 0;

        public int defense = 0;

        public int gunDamage = 0;
        float inaccuracy = 0;
        public float firerate = 0;
        public float critChance = 0.04f;
        Random rnd = new Random();

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
        }

        public void SetGunStats(int dmg, float inac, float fr) {
            gunDamage = dmg; inaccuracy = inac; firerate = fr;
        }

        public override void Update(float dt)
        {
            attackDelay += dt;
            iFrames += dt;

            bool jump = InputManager.JustPressedInput("space") || InputManager.JustPressedInput("up");
            if (jump) Jump();
            bool a = InputManager.GetInput("a");
            bool d = InputManager.GetInput("d");
            if (a && !d) MoveLeft(dt);
            if (!a && d) MoveRight(dt);

            if (regen > 0) {
                regenTimer += dt;
                if (regenTimer >= 1f) {
                    regenTimer = 0;
                    if (hp < 400) hp += regen;
                    if (hp > 400) hp = 400;
                    Console.WriteLine($"HP | {hp}");
                }
            }

            if (dash) {
                dashCD += dt;
                if (dashCD >= 0.5f) {
                    dash = false;
                    dashCD = 0;
                    dashRWindowActive = false;
                    dashRtimer = 0;
                }
            } else {
                body.velocity.X = MathUtils.Clamp(body.velocity.X, -maxSpeed, maxSpeed);
                if (dashRWindowActive) dashRtimer += dt;
                if (dashRtimer >= 0.5f) {
                    dashRWindowActive = false;
                    dashRtimer = 0;
                }

                if (dashLWindowActive) dashLtimer += dt;
                if (dashLtimer >= 0.5f)
                {
                    dashLWindowActive = false;
                    dashLtimer = 0;
                }
            }

            if (panic)
            {
                panicTimer += dt;
                speed = panicSpeed;
                maxSpeed = panicMaxSpeed;
                if (panicTimer >= 5)
                {
                    panic = false;
                    panicTimer = 0;
                    speed /= 2;
                    maxSpeed /= 2;
                }
            }

            if (!a && !d)
            {
                if (Math.Abs(body.velocity.X) <= speed) body.velocity.X = 0; 
                else {
                    float breakSpeed = isGrounded ? speed : speed / 4;
                    body.velocity.X -= Math.Sign(body.velocity.X) * breakSpeed;
                }
            }

            if (position.Y - size.Y / 2 > -200) isGrounded = false;

            if (position.Y - size.Y / 2 < -200)
            {
                position.Y = -200 + size.Y / 2;
                body.velocity.Y = 0;
                isGrounded = true;
                body.gravityScale = 0;
                jumped = false;
                doubleJumped = false;
            }
        }

        public void Jump() {
            if (isGrounded) {
                if (!jumped) {
                    body.gravityScale = 1;
                    body.velocity.Y = 0;
                    body.AddImpulse(0, 300f, 0);
                    isGrounded = false;
                    jumped = true;
                }
            } else {
                if (TBS.config.equipment[1])
                {
                    if (!doubleJumped) {
                        body.gravityScale = 1;
                        body.velocity.Y = 0;
                        body.AddImpulse(0, 300f, 0);
                        isGrounded = false;
                        doubleJumped = true;
                    }
                }
            }
        }

        public void MoveRight(float dt)
        {
            
            facingRight = true;
            float moveSpeed = isGrounded ? speed : speed / 2;
            if (!dash) body.AddImpulse(moveSpeed, 0, 0);
            if (TBS.config.equipment[0])
            {
                if (!dash)
                {
                    if (InputManager.JustPressedInput("d"))
                    {
                        if (dashRWindowActive)
                        {
                            body.AddImpulse(dashSpeed, 0, 0);
                            dash = true;
                            dashCD = 0;
                            dashRWindowActive = false;
                            dashRtimer = 0;
                            Console.WriteLine("Dashed");
                        }
                        else
                        {
                            dashRWindowActive = true;
                            dashRtimer = 0;
                            dashLWindowActive = false;
                            dashLtimer = 0;
                            Console.WriteLine("Dash Window Active");
                        }
                    }
                }
            }
        }

        public void MoveLeft(float dt) {
            facingRight = false;
            float moveSpeed = isGrounded ? speed : speed / 2;
            if (!dash) body.AddImpulse(-moveSpeed, 0, 0);
            if (TBS.config.equipment[0])
            {
                if (!dash)
                {
                    if (InputManager.JustPressedInput("a"))
                    {
                        if (dashLWindowActive)
                        {
                            body.AddImpulse(-dashSpeed, 0, 0);
                            dash = true;
                            dashCD = 0;
                            dashLWindowActive = false;
                            dashLtimer = 0;
                            Console.WriteLine("Dashed");
                        }
                        else
                        {
                            dashRWindowActive = false;
                            dashRtimer = 0;
                            dashLWindowActive = true;
                            dashLtimer = 0;
                        }
                    }
                }
            }
        }

        public PlayerBullet Shoot(Camera c)
        {
            Vector3 m = c.GetCursorOnWorld();
            float angle = MathUtils.GetAngleFrom2Points(position, m);
            angle += (float)rnd.NextDouble() * -inaccuracy;

            PlayerBullet b = new PlayerBullet(
                position,
                new Vector3(0, 0, angle),
                new Vector3(10, 2, 0),
                Color.Red, Color.Orange,
                gunDamage, critChance, 400f
            );
            b.SetCol();
            attackDelay = 0f;
            if (TBS.config.weapon == 3) AudioManager.PlaySFX("vortex");
            else AudioManager.PlaySFX("gunshot");
            return b;
        }

        public override void OnCollisionStay(GameObject c) {
            if (!(c is PlayerBullet))
            {
                if (iFrames > maxiFrames) {
                    body.gravityScale = 1;
                    isGrounded = false;
                    body.velocity = Vector3.Zero;
                    body.AddImpulse(position.X > c.position.X ? knockback : -knockback, knockback * 0.5f,0);
                    if (c is Enemy e) hp -= e.dmg - defense;
                    if (c is Proyectile p) hp -= p.dmg - defense;
                    Console.WriteLine($"Player HP: {hp}");
                    iFrames = 0;
                    panic = true;
                    panicTimer = 0;
                }
            }
        }
    }
}
