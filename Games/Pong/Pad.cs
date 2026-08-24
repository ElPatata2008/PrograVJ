using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PrograVJ.Games
{
    public class Pad : GameObject
    {
        int playerNumber;
        float speed = 0;
        float accel;
        float maxSpeed;
        GameObject target;
        float vLimits;
        int score = 0;

        public bool bounced;

        bool up, down;

        public Pad(Vector3 position, Vector3 rotation, Vector3 size, Color color, 
            int playerNumber, float accel, float maxSpeed, float vlimit) : base(position, rotation, size, color)
        {
            this.playerNumber = playerNumber;
            this.accel = accel;
            this.maxSpeed = maxSpeed;
            this.vLimits = vlimit;

            bounced = false;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(new SolidBrush(color)), position.X, position.Y, size.X, size.Y);
        }

        public override void Update()
        {
            switch (playerNumber)
            {
                case 1:
                    if (up && position.Y > 0) speed -= accel;
                    else if (speed < 0 && !up && !down) speed += accel;

                    if (down && position.Y + size.Y < vLimits) speed += accel;
                    else if (speed > 0 && !up && !down) speed -= accel;
                    break;

                case 2:
                    if (target.position.Y + target.size.Y / 2 < position.Y + size.Y / 2)
                    {
                        if (position.Y > 0) speed -= accel;
                        else speed = 0;
                    }
                    else
                    {
                        if (position.Y + size.Y < vLimits) speed += accel;
                        else speed = 0;
                    }
                    break;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
                speed = 0;
            }
            
            if (position.Y + size.Y > vLimits)
            {
                position.Y = vLimits - size.Y;
                speed = 0;
            }
            
            if (speed < -maxSpeed) speed = -maxSpeed;
            if (speed > maxSpeed) speed = maxSpeed;

            position.Y += speed;
            
        }

        public void SetTarget(GameObject obj) => target = obj;
        public bool CanBounce()
        {
            bool inBounds = target.position.Y + target.size.Y > position.Y && target.position.Y < position.Y + size.Y;

            switch (playerNumber)
            {
                case 1: return target.position.X < position.X + size.X && inBounds && !bounced;
                case 2: return target.position.X + target.size.X > position.X && inBounds && !bounced;
                default: return false;
            }
        }

        public void Up(bool keyPressed) => up = keyPressed; 

        public void Down(bool KeyPressed) => down = KeyPressed;

        public void AddScore() => score++;
        public int GetScore() => score;
    }
}
