using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.Asteroids
{
    public class Asteroids : Game
    {

        Ship player;
        List<Bullet> bullets = new List<Bullet>();
        List<Asteroid> asteroids = new List<Asteroid>();
        List<PowerUp> powerUps = new List<PowerUp>();
        Asteroid closestAsteroid;

        bool a, d, w, mlb, clicked, space;
        Font font = new Font("Arial", 20, FontStyle.Bold);

        Random rnd = new Random();

        Square worldLimits;
        int score = 0, highscore = 0, lives = 3;
        int round = 1, bestRound = 0;
        float timeLimit;
        Stopwatch sw = new Stopwatch();
        bool gameOver = false, hasPowerUp = false;

        public Asteroids(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            sw.Start();

            Instantiate(player = new Ship(
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 0f),
                new Vector3(5f, 5f, 5f),
                Color.White, true, Color.Black,
                0.1f, 5f, 5f
                //Color.Black
            ));

            Instantiate(worldLimits = new Square(
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 0f),
                new Vector3(30f, 30f, 0f),
                Color.White, true, Color.Black
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
            #region Game Over Conditions
            if (timeLimit < 1)
            {
                gameOver = true;
                player.isActive = false;
            }
            else timeLimit = (round * (int)Math.Log(6 * round) + 10) - (sw.ElapsedMilliseconds / 1000);

            if (lives == 0)
            {
                gameOver = true;
                player.isActive = false;
            }
            #endregion

            #region Player
            if (!gameOver)
            {
                if (a) player.RotateShipLeft();
                if (d) player.RotateShipRight();

                if (450f <= player.position.X + player.size.X) player.position.X = 445f;
                if (-450f >= player.position.X - player.size.X) player.position.X = -445f;
                if (450f <= player.position.Y + player.size.Y) player.position.Y = 445f;
                if (-450f >= player.position.Y - player.size.Y) player.position.Y = -445f;

                player.StartMoving(w);

                c.position = player.position;

                if (mlb && !clicked)
                {
                    clicked = true;
                    Color puColor = Color.White;

                    if (hasPowerUp) puColor = Color.LightSkyBlue;


                    Bullet bullet = new Bullet(
                        player.position, player.rotation,
                        new Vector3(3f, 0.5f, 0f),
                        puColor,
                        true,
                        puColor,
                        15f
                    );
                    bullets.Add(bullet);
                    Instantiate(bullet);
                    
                    if (hasPowerUp)
                    {
                        Bullet bullet1 = new Bullet(
                            player.position, 
                            new Vector3(0f, 0f, player.rotation.Z + 20),
                            new Vector3(3f, 0.5f, 0f),
                            puColor,
                            true,
                            puColor,
                            15f
                        );
                        Bullet bullet2 = new Bullet(
                            player.position,
                            new Vector3(0f, 0f, player.rotation.Z - 20),
                            new Vector3(3f, 0.5f, 0f),
                            puColor,
                            true,
                            puColor,
                            15f
                        );
                        bullets.Add(bullet1); bullets.Add(bullet2);
                        Instantiate(bullet1); Instantiate(bullet2);

                    }

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
                    if (bestRound < round) bestRound = round;
                    round = 1;
                    player.Restore();
                    hasPowerUp = false;
                    powerUps.Clear();
                    asteroids.Clear(); 
                    InstantiateAsteroids();
                }
            }
            #endregion

            #region Bullets
            foreach (Bullet b in bullets)
            {
                if (gameObjects.Contains(b))
                {
                    #region Bullet Limits
                    if (450 < b.position.X - b.size.X)
                    {
                        Deinstantiate(b);
                        bullets.Remove(b);
                        break;
                    }
                    if (-450 > b.position.X + b.size.X)
                    {
                        Deinstantiate(b);
                        bullets.Remove(b);
                        break;
                    }
                    if (450 < b.position.Y - b.size.Y)
                    {
                        Deinstantiate(b);
                        bullets.Remove(b);
                        break;
                    }
                    if (-450 > b.position.Y + b.size.Y)
                    {
                        Deinstantiate(b);
                        bullets.Remove(b);
                        break;
                    }
                    #endregion
                }
            }
            #endregion

            #region Asteroids
            if (asteroids.Count == 0) {
                round++;
                InstantiateAsteroids();
                Console.WriteLine("Generating");
            }


            float maxDist = 100000;
            foreach(Asteroid a in asteroids)
            {
                double powX = Math.Pow(a.position.X + player.position.X, 2);
                double powY = Math.Pow(a.position.Y + player.position.Y, 2);
                double sqDist = Math.Sqrt(powX + powY);
                if (sqDist < maxDist)
                {
                    maxDist = (float)sqDist;
                    closestAsteroid = a;
                }
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

                            Random rndpu = new Random();

                            if (rndpu.Next(10) == 1 && !hasPowerUp)
                            {
                                PowerUp powerUp = new PowerUp(
                                    a.position,
                                    Vector3.Zero,
                                    new Vector3(3f, 3f, 3f),
                                    Color.AliceBlue, true, Color.LightSkyBlue
                                );

                                powerUps.Add(powerUp);
                                Instantiate(powerUp);
                            }

                            if (a.type == 1)
                            {   
                                Asteroid a1 = new Asteroid(
                                    new Vector3(a.position.X, a.position.Y, 0f),
                                    new Vector3(0f, 0f, a.rotation.Z + 90),
                                    new Vector3((a.size.X / 4) * 3, (a.size.X / 4) * 3, 0f),
                                    Color.White,
                                    true, Color.Black,
                                    0
                                );
                                Asteroid a2 = new Asteroid(
                                    new Vector3(a.position.X, a.position.Y, 0f),
                                    new Vector3(0f, 0f, a.rotation.Z - 90),
                                    new Vector3((a.size.X / 4) * 3, (a.size.X / 4) * 3, 0f),
                                    Color.White,
                                    true, Color.Black,
                                    0
                                );

                                asteroids.Add(a1); asteroids.Add(a2);
                                Instantiate(a1); Instantiate(a2);
                            }

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
                        hasPowerUp = false;

                        Deinstantiate(a);
                        asteroids.Remove(a);
                        break;
                    }
                }


                #endregion

                #region Bounds Check
                if (450 < a.position.X + a.size.X)
                    if (a.dirX > 0) a.dirX *= -1;
                if (-450 > a.position.X - a.size.X)
                    if (a.dirX < 0) a.dirX *= -1;

                if (450 < a.position.Y + a.size.Y)
                    if (a.dirY > 0) a.dirY *= -1;
                if (-450 > a.position.Y - a.size.Y)
                    if (a.dirY < 0) a.dirY *= -1;
                #endregion
            }
            #endregion

            #region Power Up
            foreach(PowerUp p in powerUps)
            {
                if (gameObjects.Contains(p))
                {
                    if (player.position.X - player.size.X * 2 < p.position.X + p.size.X * 2 &&
                        player.position.X + player.size.X * 2 > p.position.X - p.size.X * 2 &&
                        player.position.Y + player.size.Y * 2 > p.position.Y - p.size.Y * 2 &&
                        player.position.Y - player.size.Y * 2 < p.position.Y + p.size.Y * 2
                    )
                    {
                        score += 40;
                        hasPowerUp = true;
                        Deinstantiate(p);
                        powerUps.Remove(p);
                        break;
                    }
                }
            }
            #endregion
        }

        protected override void Render(Graphics g)
        {
            g.Clear(Color.Black);

            worldLimits.Draw(g, c);

            if (!gameOver) player.Draw(g, c);
            foreach(PowerUp pu in powerUps) pu.Draw(g, c);
            foreach(Bullet b in bullets) b.Draw(g, c); 
            foreach(Asteroid a in asteroids) a.Draw(g, c);

            Vector3 viewPoint = c.GetCameraPosition();

            if (gameOver) {
                g.DrawString("GAME OVER", font, new SolidBrush(Color.White), viewPoint.X + c.size.X / 2 - 100, viewPoint.Y + c.size.Y / 2);
                g.DrawString("Press [Space] to restart", font, new SolidBrush(Color.White), viewPoint.X + c.size.X / 2 - 175, viewPoint.Y + c.size.Y / 2 + 40);
            }
            else
            {
                g.DrawString($"Lives: {lives}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y + 30);
                g.DrawString($"Score: {score}", font, new SolidBrush(Color.White), viewPoint.X + 150, viewPoint.Y + 30);
                g.DrawString($"Asteroids: {asteroids.Count()}", font, new SolidBrush(Color.White), viewPoint.X + 300, viewPoint.Y);
                g.DrawString($"Round: {round}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y);
                g.DrawString($"Time : {timeLimit}", font, new SolidBrush(Color.White), viewPoint.X + 150, viewPoint.Y);
                if (hasPowerUp) g.DrawString("POWERED UP!", font, new SolidBrush(Color.LightBlue), viewPoint.X + 40, viewPoint.Y + 60);



                g.DrawString($"Closest Asteroid", font, new SolidBrush(Color.White), viewPoint.X + c.size.X - 230, viewPoint.Y);
                g.DrawString($"[{closestAsteroid.position.X.ToString("F1")}, {closestAsteroid.position.Y.ToString("F1")}]", font, new SolidBrush(Color.White), viewPoint.X + c.size.X - 225, viewPoint.Y + 30);
                g.DrawString($"Player Position", font, new SolidBrush(Color.White), viewPoint.X + c.size.X - 220, viewPoint.Y + 60);
                g.DrawString($"[{player.position.X.ToString("F1")}, {player.position.Y.ToString("F1")}]", font, new SolidBrush(Color.White), viewPoint.X + c.size.X - 225, viewPoint.Y + 90);


                if (bestRound > 0) g.DrawString($"Best Round: {bestRound}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y + c.size.Y - 60);
                if (highscore > 0) g.DrawString($"Highscore : {highscore}", font, new SolidBrush(Color.White), viewPoint.X, viewPoint.Y + c.size.Y - 30);
            }
            
        }

        private void InstantiateAsteroids()
        {
            float asteroidAmount = round * 1.5f;
            timeLimit = (round * (int)Math.Log(6 * round) + 10) - (sw.ElapsedMilliseconds / 1000);
            sw.Restart();

            Random rndType = new Random();
            for (int i = 0; i < (int)asteroidAmount; i++)
            {
                int type = rndType.Next(8);

                Console.WriteLine(type);

                int size = rnd.Next(6, 9);
                int posX = rnd.Next(-450, 450);
                int posY = rnd.Next(-450, 450);

                Asteroid asteroid = new Asteroid(
                    new Vector3(posX, posY, 0f),
                    new Vector3(0f, 0f, rnd.Next(0, 360)),
                    new Vector3(size, size, 0f),
                    type > 5 && round > 3 ? Color.Red : Color.White,
                    true, Color.Black,
                    type > 5 && round > 3 ? 1 : 0
                );

                asteroids.Add(asteroid);
                Instantiate(asteroid);
            }
        }
    }
}
