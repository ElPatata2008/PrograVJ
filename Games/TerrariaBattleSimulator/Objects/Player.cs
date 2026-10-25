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
        float speed = 20f;
        float maxSpeed = 150f;
        float knockback = 400f;
        bool facingRight = false;

        float iFrames = 0;
        public float attackDelay = 0;

        int gunDamage = 0;
        float inaccuracy = 0;
        public float firerate = 0;
        float critChance = 0.04f;
        Random rnd = new Random();

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            switch (TerrariaBattleSimulator.config.weapon)
            {
                case 0: SetGunStats(26, 2, 0.33f); break; // Undertaker
                case 1: SetGunStats(38, 0, 0.53f); critChance += 0.08f; break; // Musket
                case 2: SetGunStats(13, 4, 0.13f); break; // Minishark
                case 3: SetGunStats(57, 4, 0.083f); break;
            }
            Console.WriteLine($"Fire Rate: {firerate}");
        }

        private void SetGunStats(int dmg, float inac, float fr) {
            gunDamage = dmg; inaccuracy = inac; firerate = fr;
        }

        public override void Update(float dt)
        {
            attackDelay += dt;
            iFrames += dt;

            if (InputManager.JustPressedInput("w")) Jump();
            bool a = InputManager.GetInput("a");
            bool d = InputManager.GetInput("d");
            if (a && !d) MoveLeft(dt);
            if (!a && d) MoveRight(dt);

            body.velocity.X = MathUtils.Clamp(body.velocity.X, -maxSpeed, maxSpeed);

            if (!a && !d)
            {
                if (Math.Abs(body.velocity.X) <= speed) body.velocity.X = 0; 
                else {
                    float breakSpeed = isGrounded ? speed : speed / 4;
                    body.velocity.X -= Math.Sign(body.velocity.X) * breakSpeed;
                }
            }

            isGrounded = (position.Y - size.Y / 2 == -200) ? true : false;
            body.gravityScale = isGrounded ? 0 : 1;

            if (position.Y - size.Y / 2 < -200)
            {
                position.Y = -200 + size.Y / 2;
                body.velocity.Y = 0;
                jumped = false;
                doubleJumped = false;
            }
        }

        public void Jump() {
            if (isGrounded) {
                if (!jumped) {
                    body.velocity.Y = 0;
                    body.AddImpulse(0, 300f, 0);
                    //isGrounded = false;
                    jumped = true;
                }
            } else {
                if (!doubleJumped) {
                    body.velocity.Y = 0;
                    body.AddImpulse(0, 300f, 0);
                    //isGrounded = false;
                    doubleJumped = true;
                }
            }
        }

        public void MoveRight(float dt)
        {
            facingRight = true;
            float moveSpeed = isGrounded ? speed : speed / 2;
            body.AddImpulse(moveSpeed, 0, 0);
        }

        public void MoveLeft(float dt) {
            facingRight = false;
            float moveSpeed = isGrounded ? speed : speed / 2;
            body.AddImpulse(-moveSpeed, 0, 0);
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
            if (TerrariaBattleSimulator.config.weapon == 3) AudioManager.PlaySFX("vortex");
            else AudioManager.PlaySFX("gunshot");
            return b;
        }

        public override void OnColiisionStay(GameObject c) {
            if (!(c is PlayerBullet))
            {
                if (iFrames > 0.5f) {
                    body.velocity = Vector3.Zero;
                    body.AddImpulse(position.X > c.position.X ? knockback : -knockback, knockback * 0.5f,0);
                    if (c is Enemy e) hp -= e.dmg;
                    if (c is Proyectile p) hp -= p.dmg;
                    Console.WriteLine($"Player HP: {hp}");
                    iFrames = 0;
                }
            }
        }
    }
}
