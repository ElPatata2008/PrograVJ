using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Xml;

namespace PrograVJ
{
    class Game
    {
        Window window;
        Graphics g;
        Font scoreFont;

        bool loop;
        bool up, down;
        float frameTime, sleepTime;
        float fps;

        float ballX, ballY;
        float ballDirX, ballDirY;
        float ballSpeed;
        float ballSize;

        float p1X, p1Y, p2X, p2Y;
        float p1Speed, p2Speed, p2Accel, p2MaxSpeed;
        float p1w, p1h, p2w, p2h;
        int score1, score2;

        public Game(int w, int h, float fps) {
            window = new Window(w, h, fps);
            g = window.CreateGraphics();
            scoreFont = new Font("Arial", 50, FontStyle.Bold);

            this.fps = fps;
            loop = true;

            ballSize = 20f;
            ballX = window.Width / 2 + ballSize / 2;
            ballY = window.Height / 2 + ballSize / 2;
            ballSpeed = 6f;
            Random rnd = new Random();
            switch (rnd.Next(3))
            {
                case 0: ballDirX = 1; ballDirY = 1; break;
                case 1: ballDirX = 1; ballDirY = -1; break;
                case 2: ballDirX = -1; ballDirY = -1; break;
                case 3: ballDirX = -1; ballDirY = 1; break;
            }

            p1w = 20f; p1h = 100f;
            p1X = 20f; p1Y = window.ClientSize.Height / 2 - p1h / 2;
            p1Speed = 10f;
            score1 = 0;

            p2w = 20f; p2h = 100f;
            p2X = window.ClientSize.Width - p2w -20f; p2Y = window.ClientSize.Height / 2 - p2h / 2;
            p2MaxSpeed = 7f;
            p2MaxSpeed = 6f;
            p2Speed = 0f;
            p2Accel = 0.2f;
            score2 = 0;
        }

        public void StartGame()
        {
            window.Show();
            Thread t = new Thread(Loop);
            t.Start();
        }

        private void Loop()
        {
            while (loop)
            {
                if (!loop) break;

                Stopwatch sw = new Stopwatch();
                sw.Start();
                ProcessInput();
                Update();
                RenderGraphics(g);
                //Render(g);
                sw.Stop();

                frameTime = sw.ElapsedMilliseconds;
                sleepTime = 1000/fps - frameTime;
                if (sleepTime < 0) sleepTime = 1;
                Thread.Sleep((int)sleepTime);
                sw.Reset();
            }

            Environment.Exit(0);
        }

        private void ProcessInput()
        {
            bool esc = window.IsPressedKey(Keys.Escape);
            if (esc) loop = false;

            up = window.IsPressedKey(Keys.Up) || window.IsPressedKey(Keys.W);
            down = window.IsPressedKey(Keys.Down) || window.IsPressedKey(Keys.S);
        }

        private void Update()
        {
            ballX += ballDirX * ballSpeed;
            ballY += ballDirY * ballSpeed;

            if (ballX + ballSize > window.ClientSize.Width)
            {
                //Console.WriteLine("Score P1!");
                //Console.WriteLine($"P1 [{score1} - {score2}] P2");
                score1++;

                ballX = window.Width / 2 + ballSize / 2;
                ballY = window.Height / 2 + ballSize / 2;

                Random rnd = new Random();
                switch (rnd.Next(3))
                {
                    case 0: ballDirX = 1; ballDirY = 1; break;
                    case 1: ballDirX = 1; ballDirY = -1; break;
                    case 2: ballDirX = -1; ballDirY = -1; break;
                    case 3: ballDirX = -1; ballDirY = 1; break;
                }
            }
            if (ballX < 0)
            {
                //Console.WriteLine("Score P2!");
                //Console.WriteLine($"P1 [{score1} - {score2}] P2");
                score2++;

                ballX = window.Width / 2 + ballSize / 2;
                ballY = window.Height / 2 + ballSize / 2;

                Random rnd = new Random();
                switch (rnd.Next(3))
                {
                    case 0: ballDirX = 1; ballDirY = 1; break;
                    case 1: ballDirX = 1; ballDirY = -1; break;
                    case 2: ballDirX = -1; ballDirY = -1; break;
                    case 3: ballDirX = -1; ballDirY = 1; break;
                }
            }

            if (ballY + ballSize > window.ClientSize.Height) ballDirY = -1;
            if (ballY < 0) ballDirY = 1;

            if (down && p1Y + p1h < window.ClientSize.Height) p1Y += p1Speed; 
            if (up && p1Y > 0) p1Y -= p1Speed;

            
            if (ballY + ballSize / 2 < p2Y + p2h / 2 ) 
            {
                if (p2Y > 0) p2Speed -= p2Accel;
                else p2Speed = 0;
            }
            else 
            {
                if (p2Y + p2h < window.ClientSize.Height) p2Speed += p2Accel; 
                else p2Speed = 0; 
            }

            if (p2Y < 0)
            {
                p2Y = 0;
                p2Speed = 0;
            }

            if (p2Y + p2h > window.ClientSize.Height)
            {
                p2Y = window.ClientSize.Height - p2h;
                p2Speed = 0;
            }

            p2Y += p2Speed;
            if (p2Speed < -p2MaxSpeed) p2Speed = -p2MaxSpeed;
            if (p2Speed > p2MaxSpeed) p2Speed = p2MaxSpeed;

            bool bouncedP1 = false;
            bool inBoundsP1 = ballY + ballSize > p1Y && ballY < p1Y + p1h;
            if (ballX < p1X + p1w && inBoundsP1 && !bouncedP1) {
                ballDirX = 1; bouncedP1 = true;
            }
            if (ballX > p1X + p1w * 2) bouncedP1 = false;

            bool bouncedP2 = false;
            bool inBoundsP2 = ballY + ballSize > p2Y && ballY < p2Y + p2h;
            if (ballX + ballSize > p2X && inBoundsP2 && !bouncedP2) {
                ballDirX = -1; bouncedP2 = true;
            }
            if (ballX + ballSize < p2X - p2w) bouncedP2 = false;
        }

        private void Render(Graphics g) {
            g.Clear(Color.Black);

            //g.FillEllipse(new SolidBrush(Color.Red), ballX, ballY, ballSize, ballSize);
            g.DrawEllipse(new Pen(new SolidBrush(Color.White)), ballX, ballY, ballSize, ballSize);
            g.DrawRectangle(new Pen(new SolidBrush(Color.White)), p1X, p1Y, p1w, p1h);
            g.DrawRectangle(new Pen(new SolidBrush(Color.White)), p2X, p2Y, p2w, p2h);

            g.DrawString($"{score1}", scoreFont, new SolidBrush(Color.White), (window.ClientSize.Width / 3) - 50, 50);
            g.DrawString($"{score2}", scoreFont, new SolidBrush(Color.White), (window.ClientSize.Width / 3) * 2, 50);

            for (int i = 0; i < 20; i++)
            {
                g.DrawRectangle(new Pen(new SolidBrush(Color.White)), window.ClientSize.Width / 2 - 5, 9 + 29 * i, 10, 20);
            }

        }

        private void RenderGraphics(Graphics g)
        {
            Render(window.GetGraphics());
            window.Render();
        }
    }
}
