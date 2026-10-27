using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
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
    public class UIButton : Square
    {
        private bool pressed = false;
        private Action action;
        private Color originalColor;
        public float delayTimer = 0;
        public UIButton(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, 
            Action action,
            Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            this.action = action;
            originalColor = fillColor;
        }

        public override void Update(float dt) => delayTimer += dt;

        public override void OnCollisionEnter(GameObject c) => fillColor = Color.White;

        public override void OnCollisionStay(GameObject c)
        {
            if (InputManager.MBL) {
                if (!pressed) {
                    if (delayTimer > 1)
                    {
                        pressed = true;
                        action.Invoke();
                    }
                }
            }
            else pressed = false; 
        }

        public override void OnCollisionExit(GameObject c) => fillColor = originalColor;

    }
}
