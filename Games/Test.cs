using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;
using PrograVJ.Engine.Manager;

namespace PrograVJ.Games
{
    public class Test : Game
    {
        Square square;
        float speed;
        float rotationSpeed;

        bool up, down, left, right;
        bool a, d, w, s;
        bool q, e;
        public Test(int w, int h, float fps) : base(w, h, fps)
        {
            speed = 1f;
            rotationSpeed = 2f;

            square = new Square(
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 1f, 1f),
                new Vector3(10f, 10f, 10f),
                Color.Black,
                true,
                Color.Blue
            );
            Instantiate(square);
        }

        protected override void ProcessInput()
        {
            up = InputManager.IsKeyPressed(Keys.Up);
            down = InputManager.IsKeyPressed(Keys.Down);
            left = InputManager.IsKeyPressed(Keys.Left);
            right = InputManager.IsKeyPressed(Keys.Right);

            a = InputManager.IsKeyPressed(Keys.A);
            d = InputManager.IsKeyPressed(Keys.D);
            w = InputManager.IsKeyPressed(Keys.W);
            s = InputManager.IsKeyPressed(Keys.S);

            q = InputManager.IsKeyPressed(Keys.Q);
            e = InputManager.IsKeyPressed(Keys.E);
        }

        protected override void Update()
        {
            if (w) square.position.Z += speed;
            if (s) square.position.Z -= speed;
            if (a) square.position.X -= speed;
            if (d) square.position.X += speed;
            if (up) square.position.Y -= speed;
            if (down) square.position.Y += speed;

            if (q) square.rotation.Z -= rotationSpeed;
            if (e) square.rotation.Z += rotationSpeed;

            if (left)
            {
                square.size.X -= 0.1f;
                square.size.Y -= 0.1f;
            }
            if (right)
            {
                square.size.X += 0.1f;
                square.size.Y += 0.1f;
            }

            Console.WriteLine($"Square: {square.position}, Camera: {c.position}, w: {w}");
        }

        protected override void Render(Graphics g)
        {
            g.Clear(Color.White);

            square.Draw(g, c);
        }
    }
}
