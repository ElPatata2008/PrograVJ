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

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects.Components
{
    public abstract class SelectionButton : Square
    {
        public Square img;
        public int idx = 0;
        public bool pressed = false;
        public float delayTimer = 0;

        public SelectionButton(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor,
            Bitmap acc, int idx,
            Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
            img = new Square(
                position,
                rotation,
                size - Vector3.One * 10,
                Color.White, Color.White,
                fillTexture: acc
            );
            this.idx = idx;
        }

        public override void Draw(Graphics g, Camera c)
        {
            base.Draw(g, c);
            img.Draw(g, c);
        }

        public override void Update(float dt) => delayTimer += dt;

        public override void OnCollisionStay(GameObject c)
        {
            if (InputManager.MBL)
            {
                if (!pressed)
                {
                    if (delayTimer > 0.25f)
                    {
                        pressed = true;
                        DoSomething();
                        delayTimer = 0f;
                    }
                }
            }
            else pressed = false;
        }

        public abstract void DoSomething();
    }
}
