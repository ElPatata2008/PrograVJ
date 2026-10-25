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
    public class Enemy : Square
    {
        public int hp;
        public int maxHp;
        public int dmg;
        public Vector3 playerPos;
        protected FSM fsm;
        public Enemy(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
        }

        public override void Update(float dt)
        {
            fsm = controller.brain as FSM;
            controller.brain.Update(dt);
        }
        public void UpdatePlayerPos(Vector3 pos) => playerPos = pos;
        public void SetStats(int hp, int dmg) { this.hp = hp; maxHp = hp; this.dmg = dmg; }
    }
}
