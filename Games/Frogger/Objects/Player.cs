using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Frogger.Objects
{
    public class Player : Square
    {
        public bool isMoving = false;
        private float targetX;
        private float targetY;
        public int speed { get; set; }
        public int hp { get; set; }

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, 
            
            Collider collider = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, borderWidth, fillTexture)
        {
            targetX = position.X;
            targetY = position.Y;
        }

        public override void Update() {
            if (isMoving)
            {
                var remainX = targetX - position.X;
                var stepX = Math.Min(speed, Math.Abs(remainX));
                if (Math.Abs(remainX) <= speed) position.X = targetX;
                else position.X += Math.Sign(remainX) * stepX;

                var remainY = targetY - position.Y;
                var stepY = Math.Min(speed, Math.Abs(remainY));
                if (Math.Abs(remainY) <= speed) position.Y = targetY;
                else position.Y += Math.Sign(remainY) * stepY;

                if (position.Y == targetY && position.X == targetX) isMoving = false;
            }
        }

        public void Up()
        {
            if (!isMoving && position.Y < 850)
            {
                AudioManager.PlaySFX("move");
                targetX = position.X;
                targetY = ((float)Math.Floor(position.Y / Frogger.tileSize) + 1) * Frogger.tileSize;
                rotation.Z = 0;
                isMoving = true;
            }
        }
        public void Down()
        {
            if (!isMoving && position.Y > -250)
            {
                AudioManager.PlaySFX("move");
                targetX = position.X;
                targetY = ((float)Math.Ceiling(position.Y / Frogger.tileSize) - 1) * Frogger.tileSize;
                rotation.Z = 180;
                isMoving = true;
            }
        }
        public void Left()
        {
            if (!isMoving && position.X > -350)
            {
                AudioManager.PlaySFX("move");
                targetY = position.Y;
                targetX = ((float)Math.Ceiling(position.X / Frogger.tileSize) - 1) * Frogger.tileSize;
                rotation.Z = 90;
                isMoving = true;
            }
        }
        public void Right()
        {
            if (!isMoving && position.X < 350)
            {
                AudioManager.PlaySFX("move");
                targetY = position.Y;
                targetX = ((float)Math.Floor(position.X / Frogger.tileSize) + 1) * Frogger.tileSize;
                rotation.Z = 270;
                isMoving = true;
            }
        }

        public void RemoveLife() => hp--;
    }
}
