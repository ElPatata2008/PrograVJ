using PrograVJ.Engine;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Arkanoid
{
    public class Block : GameObject
    {
        public int hp, initHp;
        public bool bouncedX, bouncedY;

        public Block(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive,
            int hp) : base(position, rotation, size, color, isActive)
        {
            this.hp = hp;
            initHp = hp;
            bouncedX = false;
            bouncedY = false;
        }

        public override void Draw(Graphics g, Camera c)
        {
            if (isActive) g.FillRectangle(new SolidBrush(color), position.X, position.Y, size.X, size.Y);
        }

        public override void Update()
        {
            switch(hp)
            {
                case 3: color = Color.Red; break;
                case 2: color = Color.Orange; break;
                case 1: color = Color.Yellow; break;
                case 0: isActive = false; break;
            }
        }

        public void CanBounce(Ball ball, int score) {
            
        }
    
        public void Restore()
        {
            hp = initHp;
            isActive = true;
        }
    }
}
