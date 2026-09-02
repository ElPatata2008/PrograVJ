using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;
using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using System.Diagnostics;

namespace PrograVJ.Games.Asteroids
{
    public class Asteroids : Game
    {

        Ship player;
        List<Bullet> bullets = new List<Bullet>();
        List<Asteroid> asteroids = new List<Asteroid>();

        bool a, d, w, mlb, clicked, space;
        Font font = new Font("Arial", 25, FontStyle.Bold);

        Random rnd = new Random();

        int score = 0, highscore = 0, lives = 3;
        int round = 1, roundTime = 10, timeLimit;
        Stopwatch sw = new Stopwatch();
        bool gameOver = false;

        public Asteroids(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            sw.Start();

            Instantiate(player = new Ship(
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 0f),
                new Vector3(5f, 5f, 5f),
                Color.White,
                true,
                0.1f, 5f, 5f
                //Color.Black
            ));

            InstantiateAsteroids();
        }

        protected override void ProcessInput()
        {
            a = InputManager.IsKeyPressed(Keys.A);
            d = InputManager.IsKeyPressed(Keys.D);
            w = InputManager.IsKeyPressed(Keys.W) || InputManager.IsKeyPressed(Keys.Up);
            mlb = InputManager.MBL;

            space = InputManager.IsKeyPressed(Keys.Space);
        }

        protected override void Update()
        {

            timeLimit = round * roundTime;
            if (sw.ElapsedMilliseconds / 1000 > round * roundTime)
            {
                gameOver = true;
                player.isActive = false;
            }

            if (lives == 0)
            {
                gameOver = true;
                player.isActive = false;
            }

            if (!gameOver)
            {
                if (a) player.RotateShipLeft();
                if (d) player.RotateShipRight();
                player.StartMoving(w);

                c.position = player.position;

                if (mlb && !clicked)
                {
                    clicked = true;

                    Bullet bullet = new Bullet(
                        player.position, player.rotation,
                        new Vector3(3f, 0.5f, 0f),
                        Color.White,
                        true,
                        Color.White,
                        15f
                    );
                    bullets.Add(bullet);
                    Instantiate(bullet);
                }
                else if (!mlb && clicked) clicked = false;
            }
            else
            {
                if (space)
                {
                    gameOver = false;
                    lives = 3;
                    if (highscore < score) highscore = score;
                    score = 0;
                    player.isActive = true;
                    round = 0;
                    asteroids.Clear(); InstantiateAsteroids();
                }
            }

                foreach (Bullet b in bullets)
                {
                    if (gameObjects.Contains(b))
                    {
                        #region Bullet Limits
                        if (c.position.X + c.size.X / 2 < b.position.X - b.size.X * 2)
                        {
                            Deinstantiate(b);
                            bullets.Remove(b);
                            break;
                        }
                        if (c.position.X - c.size.X / 2 > b.position.X + b.size.X)
                        {
                            Deinstantiate(b);
                            bullets.Remove(b);
                            break;
                        }
                        if (c.position.Y + c.size.Y / 2 < b.position.Y - b.size.Y * 2)
                        {
                            Deinstantiate(b);
                            bullets.Remove(b);
                            break;
                        }
                        if (c.position.Y - c.size.Y / 2 > b.position.Y + b.size.Y)
                        {
                            Deinstantiate(b);
                            bullets.Remove(b);
                            break;
                        }
                        #endregion
                    }
                }

            if (asteroids.Count == 0) {
                round++;
                InstantiateAsteroids();
                Console.WriteLine("Generating");
            }

            foreach (Asteroid a in asteroids)
            {
                #region Collision

                // Bullet
                bool destroyed = false;
                foreach (Bullet b in bullets)
                {
                    if (gameObjects.Contains(a) && gameObjects.Contains(b))
                    {
                        if (b.position.X - b.size.X * 2 < a.position.X + a.size.X * 2 &&
                            b.position.X + b.size.X * 2 > a.position.X - a.size.X * 2 &&
                            b.position.Y + b.size.Y * 2 > a.position.Y - a.size.Y * 2 &&
                            b.position.Y - b.size.Y * 2 < a.position.Y + a.size.Y * 2
                        )
                        {
                            score += 7 * (int)a.size.X;

                            Deinstantiate(a);
                            asteroids.Remove(a);
                            destroyed = true;
                            Deinstantiate(b);
                            bullets.Remove(b);
                            break;
                        }
                    }
                }
                if (destroyed) break;

                // Player
                if (player.isActive)
                {
                    if (player.position.X - player.size.X * 2 < a.position.X + a.size.X * 2 &&
                        player.position.X + player.size.X * 2 > a.position.X - a.size.X * 2 &&
                        player.position.Y + player.size.Y * 2 > a.position.Y - a.size.Y * 2 &&
                        player.position.Y - player.size.Y * 2 < a.position.Y + a.size.Y * 2
                    )
                    {
                        score += 2 * (int)a.size.X;
                        lives--;

                        Deinstantiate(a);
                        asteroids.Remove(a);
                        break;
                    }
                }


                #endregion

                #region Bounds Check
                if (c.position.X + c.size.X / 2 < a.position.X + a.size.X)
                    if (a.dirX > 0) a.dirX *= -1;
                if (c.position.X - c.size.X / 2 > a.position.X - a.size.X)
                    if (a.dirX < 0) a.dirX *= -1;

                if (c.position.Y + c.size.Y / 2 < a.position.Y + a.size.Y)
                    if (a.dirY > 0) a.dirY *= -1;
                if (c.position.Y - c.size.Y / 2 > a.position.Y - a.size.Y)
                    if (a.dirY < 0) a.dirY *= -1;
                #endregion
            }
        }

        protected override void Render(Graphics g)
        {
            g.Clear(Color.Black);

            if (!gameOver) player.Draw(g, c);
            foreach(Bullet b in bullets) b.Draw(g, c); 
            foreach(Asteroid a in asteroids) a.Draw(g, c);

            Vector3 viewPoint = c.GetCameraPosition();

            if (gameOver) {
                g.DrawString("GAME OVER", font, new SolidBrush(Color.White), viewPoint.X + c.size.X / 2 - 100, viewPoint.Y + c.size.Y / 2);
                g.DrawString("Press [Space] to restart", font, new SolidBrush(Color.White), viewPoint.X + c.size.X / 2 - 175, viewPoint.Y + c.size.Y / 2 + 40);
            }
            else
            {
                g.DrawString($"Score: {score}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y);
                g.DrawString($"Lives: {lives}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y + 40);
                g.DrawString($"Time : {sw.ElapsedMilliseconds.ToString()}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y + 80);
                if (highscore > 0) g.DrawString($"Highscore: {highscore}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y + c.size.Y - 40);
            }
            
        }

        private void InstantiateAsteroids()
        {
            float asteroidAmount = round * 1.5f;

            for (int i = 0; i < (int)asteroidAmount; i++)
            {

                int size = rnd.Next(6, 9);
                int rndPosX = rnd.Next(0, (int)c.size.X / 2);
                int rndPosY = rnd.Next(0, (int)c.size.Y / 2);

                float posX, posY;

                switch (rnd.Next(1, 5))
                {
                    case 1: posX = c.position.X + rndPosX; posY = c.position.Y + rndPosY; break;
                    case 2: posX = c.position.X - rndPosX; posY = c.position.Y + rndPosY; break;
                    case 3: posX = c.position.X - rndPosX; posY = c.position.Y - rndPosY; break;
                    case 4: posX = c.position.X + rndPosX; posY = c.position.Y - rndPosY; break;
                    default: posX = 0; posY = 0; break;
                }

                Asteroid asteroid = new Asteroid(
                    new Vector3(posX, posY, 0f),
                    new Vector3(0f, 0f, rnd.Next(0, 360)),
                    new Vector3(size, size, 0f),
                    Color.White,
                    true,
                    Color.Black
                );

                asteroids.Add(asteroid);
                Instantiate(asteroid);
            }
        }
    }
}
