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
        float rotSpeed = 1f;
        float accel;

        bool move;

        public Ship(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive,
            float accel, float maxSpeed, float rotSpeed) : base(position, rotation, size, color, isActive)
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

            position.X += dir.X * speed;
            position.Y += dir.Y * speed;
        }

        public void StartMoving(bool keyPressed) => move = keyPressed;
        public void RotateShipLeft() => rotation.Z += rotSpeed;
        public void RotateShipRight() => rotation.Z -= rotSpeed;
    }
}
