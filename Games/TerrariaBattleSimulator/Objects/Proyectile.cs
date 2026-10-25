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
    public class Proyectile : Square
    {
        public float speed = 400f;
        public int dmg = 0;
        public float critChance = 0;
        public float expire = 0;
        public bool landed = false;
        public Proyectile(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor,
            int dmg, float critChance, float speed,
            Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            this.dmg = dmg;
            this.critChance = critChance;
            this.speed = speed;
        }
        public void SetCol() => collider = new BoxCollider2D(this);

        public override void Update(float dt)
        {
            Vector2 dir = MathUtils.GetDirectionFromAngle(rotation.Z);
            position += new Vector3(dir.X, dir.Y, 0) * speed * dt;
            expire += dt;
        }
    }
}
