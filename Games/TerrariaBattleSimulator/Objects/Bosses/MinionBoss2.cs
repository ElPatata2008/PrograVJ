using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class MinionBoss2 : Enemy
    {
        float rotSpeed;
        float moveSpeed;
        float maxSpeed; 
        public MinionBoss2(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, 
            bool phase2,
            Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            if (phase2) { rotSpeed = 135f; moveSpeed = 150f;   maxSpeed = 200f; }
            else        { rotSpeed = 90f; moveSpeed = 75f; maxSpeed = 150f; }
        }
        public void SetCollision() => collider = new CircleCollider2D(this);
        public override void InitStats()
        {
            switch (TBS.config.difficulty)
            {
                case 0: SetStats(8, 12); break;
                case 1: SetStats(12, 24); break;
                case 2: SetStats(15, 36); break;
            }
        }

        public override void Update(float dt)
        {
            float targetAngle = MathUtils.GetAngleFrom2Points(position, playerPos);
            float angleDiff = targetAngle - rotation.Z;
            while (angleDiff < -180) angleDiff += 360;
            while (angleDiff > 180) angleDiff -= 360;

            if (Math.Abs(angleDiff) <= 5) rotation.Z = targetAngle;
            else rotation.Z += Math.Sign(rotation.Z) * rotSpeed * dt;

            Vector2 dir = MathUtils.GetDirectionFromAngle(rotation.Z);
            body.velocity.X = dir.X * moveSpeed;
            body.velocity.Y = dir.Y * moveSpeed;
            
            body.velocity.X = MathUtils.Clamp(body.velocity.X, -maxSpeed, maxSpeed);
            body.velocity.Y = MathUtils.Clamp(body.velocity.Y, -maxSpeed, maxSpeed);
        }
    }
}
