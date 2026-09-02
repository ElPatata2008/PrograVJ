using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrograVJ.Games.Asteroids
{
    public class Ship : Triangle
    {

        float speed = 0, maxSpeed = 5f;
        float accel;
        float rotSpeed = 1f;

        public float dirX, dirY;
        bool move;

        public Ship(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor,
            float accel, float maxSpeed, float rotSpeed) : base(position, rotation, size, color, isActive, fillColor)
        {
            this.accel = accel;
            this.maxSpeed = maxSpeed;
            this.rotSpeed = rotSpeed;
        }

        public override void Update() {
            if (move) { speed += accel; }
            else if (!move && speed > 0) { speed -= accel; }

            if (speed < 0) speed = 0; 
            if (speed > maxSpeed) speed = maxSpeed;

            Vector2 dir = MathUtils.GetAngle2D(rotation.Z);

            dirX = dir.X; dirY = dir.Y;

            position.X += dirX * speed;
            position.Y += dirY * speed;
        }

        public void StartMoving(bool keyPressed) => move = keyPressed;
        public void RotateShipLeft() => rotation.Z += rotSpeed;
        public void RotateShipRight() => rotation.Z -= rotSpeed;

        public void Restore()
        {
            position = Vector3.Zero;
            isActive = true;
            speed = 0;
        }
    }
}
