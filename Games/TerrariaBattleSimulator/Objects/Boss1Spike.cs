using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class Boss1Spike : Proyectile
    {
        public float rotSpeed { get; set; } = 45f;
        public Boss1Spike(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, int dmg, float critChance, float speed, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, dmg, critChance, speed, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        { expire *= 3; }

        public override void Update(float dt)
        {
            if (expire > 0.5f) {
                float diff = 270 - rotation.Z;
                float step = rotSpeed * dt;
                diff = diff % 360;
                if (diff >  180) diff -= 360;
                if (diff < -180) diff += 360;
                if (Math.Abs(diff) <= step) rotation.Z = 270;
                else rotation.Z += Math.Sign(diff) * step;
            }
            base.Update(dt);
        }
    }
}
