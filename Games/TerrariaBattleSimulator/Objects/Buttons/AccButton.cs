using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects.Components;
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
    public class AccButton : SelectionButton
    {
        public AccButton(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Bitmap acc, int idx, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, acc, idx, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            fillColor = TBS.config.equipment[idx] ? Color.Green : Color.White;
        }

        public override void DoSomething()
        {
            bool isAtive = TBS.config.equipment[idx];
            if (isAtive) TBS.config.equipment[idx] = false;
            else {
                int count = TBS.config.equipment.Where(a => a == true).Count();
                if (count < 4) TBS.config.equipment[idx] = true;
            }
        }
    }
}
