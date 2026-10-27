using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.TerrariaBattleSimulator.Objects.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects.Buttons
{
    public class DifButton : SelectionButton
    {
        public DifButton(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Bitmap acc, int idx, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, acc, idx, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
        }

        public override void Update(float dt) => fillColor = TBS.config.difficulty == idx ? Color.Green : Color.White;
        public override void DoSomething() => TBS.config.difficulty = idx;
    }
}
