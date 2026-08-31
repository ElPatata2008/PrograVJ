using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using PrograVJ.Engine.Manager;

namespace PrograVJ.Games
{
    public class Pong : Game
    {
        Font scoreFont;

        bool up, down;

        Ball ball;
        Pad P1, P2;

        public Pong(int w, int h, float fps) : base(w, h, fps)
        {
            scoreFont = new Font("Arial", 50, FontStyle.Bold);

            ball = new Ball(
                new Vector3(window.ClientSize.Width / 2 - 10f, window.ClientSize.Height / 2 - 10f, 0),
                new Vector3(1, 1, 1),
                new Vector3(20f, 20f, 1f),
                Color.White,
                true,
                6f
            );

            P1 = new Pad(
                new Vector3(20f, window.ClientSize.Height / 2 - 100f, 0),
                new Vector3(1, 1, 1),
                new Vector3(20f, 100f, 1f),
                Color.White,
                true,
                1,
                2.5f,
                10f,
                window.ClientSize.Height
            );

            P2 = new Pad(
                new Vector3(window.ClientSize.Width - 20f - 20f, window.ClientSize.Height / 2 - 50f, 0),
                new Vector3(1, 1, 1),
                new Vector3(20f, 100f, 1f),
                Color.White,
                true,
                2,
                0.2f,
                6f,
                window.ClientSize.Height
            );

            P1.SetTarget(ball);
            P2.SetTarget(ball);

            gameObjects.Add(ball);
            gameObjects.Add(P1);
            gameObjects.Add(P2);
        }

        protected override void ProcessInput()
        {
            bool esc = InputManager.IsKeyPressed(Keys.Escape);
            if (esc) loop = false;

            up = InputManager.IsKeyPressed(Keys.Up) || InputManager.IsKeyPressed(Keys.W);
            down = InputManager.IsKeyPressed(Keys.Down) || InputManager.IsKeyPressed(Keys.S);
        }

        protected override void Update()
        {

            if (ball.position.X + 5f > window.ClientSize.Width)
            {
                P1.AddScore();
                ball.SetInitPosition();
                ball.SetRandomDirection();
            }
            if (ball.position.X + ball.size.X + 5f < 0)
            {
                P2.AddScore();
                ball.SetInitPosition();
                ball.SetRandomDirection();
            }

            if (ball.position.Y + ball.size.Y > window.ClientSize.Height) ball.dirY = -1;
            if (ball.position.Y < 0) ball.dirY = 1;

            P1.Up(up);
            P1.Down(down);

            if (P1.CanBounce())
            {
                ball.dirX = 1; P1.bounced = true;
            }
            if (ball.position.X > P1.position.X + P1.size.X * 2) P1.bounced = false;

            if (P2.CanBounce())
            {
                ball.dirX = -1; P2.bounced = true;
            }
            if (ball.position.X + ball.size.X < P2.position.X - P2.size.X) P2.bounced = false;
        }


        protected override void Render(Graphics g)
        {
            g.Clear(Color.Black);

            ball.Draw(g, c);
            P1.Draw(g, c);
            P2.Draw(g, c);

            g.DrawString($"{P1.GetScore()}", scoreFont, new SolidBrush(Color.White), (window.ClientSize.Width / 3) - 50, 50);
            g.DrawString($"{P2.GetScore()}", scoreFont, new SolidBrush(Color.White), (window.ClientSize.Width / 3) * 2, 50);

            for (int i = 0; i < 20; i++)
            {
                g.DrawRectangle(new Pen(new SolidBrush(Color.White)), window.ClientSize.Width / 2 - 5, 9 + 29 * i, 10, 20);
            }
        }
    }
}
