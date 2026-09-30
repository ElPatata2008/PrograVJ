using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Frogger.Objects
{
    public class Player : Square
    {
        public bool isMoving = false;
        private int speedX = 0;
        private int speedY = 0;
        public int speed { get; set; }
        public int hp { get; set; }

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, 
            
            Collider collider = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, borderWidth, fillTexture)
        {

        }

        public override void Update() {
            if (isMoving)
            {
                position.X += speedX;
                position.Y += speedY;
            }
            if (position.X % Frogger.tileSize == 0 && speedX != 0 ) { speedX = 0; isMoving = false; }
            if (position.Y % Frogger.tileSize == 0 && speedY != 0 ) { speedY = 0; isMoving = false; }
        }

        public void Up()
        {
            if (!isMoving && position.Y < 850)
            {
                isMoving = true;
                speedY = speed;
            }
        }
        public void Down()
        {
            if (!isMoving && position.Y > -250)
            {
                isMoving = true;
                speedY = -speed;
            }
        }
        public void Left()
        {
            if (!isMoving && position.X > -350)
            {
                isMoving = true;
                speedX = -speed;
            }
        }
        public void Right()
        {
            if (!isMoving && position.X < 350)
            {
                isMoving = true;
                speedX = speed;
            }
        }

        public void RemoveLife() => hp--;
    }
}
