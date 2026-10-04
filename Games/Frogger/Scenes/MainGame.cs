using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.Frogger;
using PrograVJ.Games.Frogger.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Scenes
{
    public class FroggerMainGame : Scene
    {
        Player player;
        Vector3 startPos = new Vector3(0, -250, 0);
        Vector3 cameraStartPos;

        #region Terrain
        List<GameObject> terrain = new List<GameObject>();
        Square road1;
        Square road2;
        Square water1;
        Square water2;
        #endregion

        List<GameObject> hazards = new List<GameObject>();
        bool safeOnLog = false;
        Random rand = new Random();

        bool gameOver = false;
        bool starting = true;
        Stopwatch sw = new Stopwatch();
        Stopwatch timeLimit = new Stopwatch();

        Square life;
        
        public FroggerMainGame(Camera c) : base(c)
        {
            cameraStartPos = sceneCamera.position;
            Instantiate(road1 = new Square(
                new Vector3(0, -50, 0),
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, 250, 0),
                Color.White, Color.Black
            ));
            terrain.Add(road1);

            Instantiate(road2 = new Square(
                new Vector3(0, 250, 0),
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, 150, 0),
                Color.White, Color.Black
            ));
            terrain.Add(road2);

            Instantiate(water1 = new Square(
                new Vector3(0, 425, 0),
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, 100, 0),
                Color.LightSlateGray, Color.Aqua, new BoxCollider2D(water1)
            ));
            terrain.Add(water1);

            Instantiate(water2 = new Square(
                new Vector3(0, 700, 0),
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, 250, 0),
                Color.LightSlateGray, Color.Aqua, new BoxCollider2D(water2)
            ));
            terrain.Add(water2);

            Instantiate(player = new Player(
                startPos,
                Vector3.Zero,
                new Vector3(49, 49, 49),
                Color.Black, Color.White, new CircleCollider2D(player), fillTexture: TextureManager.Get("frog")
            )
            { speed = 10, hp = 3 });
        }

        public override void Init()
        {
            player.hp = 3;
            sw.Restart();
            timeLimit.Reset();
            starting = true;
            rand = new Random();
            sceneCamera.position = cameraStartPos;
            CreateHazards();
            AudioManager.PlayMusic("music", true, 0);
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.DarkGreen);
            g.FillRectangle(new TextureBrush(TextureManager.Get("ground")), new Rectangle((int)sceneCamera.position.X, -500, (int)sceneCamera.size.X, 1200));
            foreach (GameObject t in terrain) t.Draw(g, sceneCamera);
            foreach (GameObject c in hazards) if (c != null) c.Draw(g, sceneCamera);
            player.Draw(g, sceneCamera);

            Rectangle ui = new Rectangle(0, 0, (int)sceneCamera.size.X, 50);
            g.FillRectangle(Brushes.Black, ui); g.DrawRectangle(Pens.White, ui);
            var displayTime = 30 - timeLimit.ElapsedMilliseconds / 1000;
            if (timeLimit.ElapsedMilliseconds / 1000 <= 30) DrawingUtils.DrawCenterText(g, $"Time Left: {displayTime}", FontManager.Get("uph", 24), Color.White, new PointF((sceneCamera.size.X / 4) * 3, 5));
            if (player.hp > 0)
            {
                for (int i = -1; i < player.hp - 1; i++)
                {
                    life = new Square(
                        new Vector3(sceneCamera.position.X - sceneCamera.size.X / 4 + 40 * i, sceneCamera.position.Y + sceneCamera.size.Y / 2 - 25, 0),
                        Vector3.Zero,
                        new Vector3(30, 30, 30),
                        Color.Red, Color.Red, fillTexture: TextureManager.Get("life")
                    );
                    life.Draw(g, sceneCamera);
                }
            }
            

            if (starting) DrawingUtils.DrawCenterText(g, 
                $"Starting in {3 - sw.ElapsedMilliseconds / 1000}", FontManager.Get("uph", 36), Color.White, 
                new PointF(sceneCamera.position.X + sceneCamera.size.X / 2, sceneCamera.position.Y + sceneCamera.size.Y / 2));
            if (gameOver) DrawingUtils.DrawCenterText(g,
                $"Restarting in {3 - sw.ElapsedMilliseconds / 1000}", FontManager.Get("uph", 36), Color.White,
                new PointF(sceneCamera.position.X + sceneCamera.size.X / 2, sceneCamera.position.Y + sceneCamera.size.Y / 2));
        }

        public override void ProcessInput()
        {
            if (!gameOver && !starting)
            {
                if (InputManager.GetInput("up")) player.Up();
                if (InputManager.GetInput("down")) player.Down();
                if (InputManager.GetInput("left")) player.Left();
                if (InputManager.GetInput("right")) player.Right();
            }
        }

        public override void Update()
        {
            if (starting)
            {
                if (sw.ElapsedMilliseconds / 1000 > 2) GameStart();
            }
            else
            {
                if (!gameOver)
                {
                    CheckCollisions();
                    
                    foreach(var h in hazards)
                    {
                        if (h.position.X + h.size.X < -sceneCamera.size.X / 2) h.position.X = sceneCamera.size.X / 2 + h.size.X / 2;
                        if (h.position.X - h.size.X > sceneCamera.size.X / 2) h.position.X = -sceneCamera.size.X / 2 - h.size.X / 2;
                    }


                    if (player.position.Y >= 0 && player.position.Y <= 650) sceneCamera.position = new Vector3(sceneCamera.position.X, player.position.Y, sceneCamera.position.Z);
                    if (player.position.Y >= 810) GameOver(true);

                    if (timeLimit.ElapsedMilliseconds / 1000 > 30) GameOver(false);

                    if (player.position.X > 350 || player.position.X < -350) GameOver(false); 
                }
                else
                {
                    if (player.position.X > 350) player.position.X += player.speed;
                    else if (player.position.X < -350) player.position.X -= player.speed;

                    if (sw.ElapsedMilliseconds / 1000 > 2)
                    {
                        if (Frogger.Frogger.victory || player.hp == 0)
                        {
                            AudioManager.PlaySFX(Frogger.Frogger.victory ? "victory" : "death");
                            SceneManager.SetActive("gameover");
                        }
                        else RestartRound();
                    }
                }
            }
        }

        private void CreateHazards()
        {
            // Road 1
            for (int i = 0; i < 5; i++)
            {
                bool type = rand.Next(0, 2) == 0;
                int pos = i - 2;
                bool dir = Math.Abs(pos) % 2 == 0;
                float Xsize = type ? Frogger.Frogger.tileSize : Frogger.Frogger.tileSize * 2;
                float Xpos = dir ? -sceneCamera.size.X / 2 - Xsize / 2 : sceneCamera.size.X / 2 + Xsize / 2;
                Car car = new Car(
                    new Vector3(Xpos, road1.position.Y + (pos) * Frogger.Frogger.tileSize, 0),
                    new Vector3(0, 0, dir ? 0 : 180),
                    new Vector3(Xsize, Frogger.Frogger.tileSize, 0),
                    Color.White, Color.White, fillTexture: type ? TextureManager.Get("car" + (rand.Next(1, 4))) : TextureManager.Get("truck")
                )
                { speed = type ? 5 : 2 };
                car.CreateCollider();
                hazards.Add(car); Instantiate(car);
            }

            // Road 2
            for (int i = 0; i < 3; i++)
            {
                bool type = rand.Next(0, 2) == 0;
                int pos = i - 1;
                bool dir = Math.Abs(pos) % 2 == 0;
                float Xsize = type ? Frogger.Frogger.tileSize : Frogger.Frogger.tileSize * 2;
                float Xpos = dir ? -sceneCamera.size.X / 2 - Xsize / 2 : sceneCamera.size.X / 2 + Xsize / 2;
                Car car = new Car(
                    new Vector3(Xpos, road2.position.Y + (pos) * Frogger.Frogger.tileSize, 0),
                    new Vector3(0, 0, dir ? 0 : 180),
                    new Vector3(Xsize, Frogger.Frogger.tileSize, 0),
                    Color.White, Color.White, new CircleCollider2D(hazards[i]),
                    fillTexture: type ? TextureManager.Get("car" + (rand.Next(1, 4))) : TextureManager.Get("truck")
                )
                { speed = type ? 5 : 2 };
                hazards.Add(car); Instantiate(car);
            }

            // Water 1
            for (int i = 0; i < 2; i++)
            {
                bool type = rand.Next(0, 2) == 0;
                int pos = i - 1;
                bool dir = Math.Abs(pos) % 2 == 0;
                float Xsize = type ? Frogger.Frogger.tileSize * 2 : Frogger.Frogger.tileSize * 3;
                float Xpos = dir ? -sceneCamera.size.X / 2 - Xsize / 2 : sceneCamera.size.X / 2 + Xsize / 2;
                Log log = new Log(
                    new Vector3(Xpos, water1.position.Y + (pos) * Frogger.Frogger.tileSize + 25, 0),
                    new Vector3(0, 0, dir ? 0 : 180),
                    new Vector3(Xsize, Frogger.Frogger.tileSize, 0),
                    Color.White, Color.Brown, fillTexture: TextureManager.Get("log")
                );
                log.CreateCollider();
                //Console.WriteLine($"Log created at {log.position.Y}, rotation {log.rotation.Z}");
                hazards.Add(log); Instantiate(log);
            }

            // Water 2
            for (int i = 0; i < 5; i++)
            {
                bool type = rand.Next(0, 2) == 0;
                int pos = i - 2;
                bool dir = rand.Next(0, 2) == 0;
                float Xsize = type ? Frogger.Frogger.tileSize * 4 : Frogger.Frogger.tileSize * 6;
                float Xpos = dir ? -sceneCamera.size.X / 2 - Xsize / 2 : sceneCamera.size.X / 2 + Xsize / 2;
                Log log = new Log(
                    new Vector3(Xpos, water2.position.Y + (pos) * Frogger.Frogger.tileSize, 0),
                    new Vector3(0, 0, dir ? 0 : 180),
                    new Vector3(Xsize, Frogger.Frogger.tileSize, 0),
                    Color.White, Color.Brown, fillTexture: TextureManager.Get("log")
                );
                log.CreateCollider();
                //Console.WriteLine($"Log created at {log.position.Y}, rotation {log.rotation.Z}");
                hazards.Add(log); Instantiate(log);
            }
        }

        private void RestartRound()
        {
            player.fillTexture = TextureManager.Get("frog");
            player.position = startPos;
            sw.Restart();
            timeLimit.Reset();
            starting = true;
            gameOver = false;
            sceneCamera.position = cameraStartPos;
            foreach(var h in hazards) Destroy(h);
            hazards.Clear();
            CreateHazards();
            AudioManager.ResumeMusic();
        }

        private void GameStart()
        {
            sw.Reset();
            starting = false;
            timeLimit.Start();
        }

        private void GameOver(bool victory)
        {
            AudioManager.PlaySFX("death");
            player.fillTexture = TextureManager.Get("death");
            gameOver = true;
            player.RemoveLife();
            sw.Start();
            timeLimit.Stop();
            Frogger.Frogger.victory = victory;
            AudioManager.PauseMusic();
        }

        private void CheckCollisions()
        {
            if (!player.isMoving)
            {
                foreach (var h in hazards)
                {
                    if (h is Car car)
                    {
                        if (player.isColliding(car))
                        {
                            GameOver(false);
                        }
                    }
                    if (h is Log log)
                    {
                        if (log.isColliding(player))
                        {
                            safeOnLog = true;
                            player.position.X += log.rotation.Z == 180 ? -log.speed : log.speed;
                        }
                    }
                }

                if (!safeOnLog)
                {
                    if (player.position.Y < 825)
                    {
                        if (water1.isColliding(player)) GameOver(false);
                        if (water2.isColliding(player)) GameOver(false);
                    }
                }
            }

            safeOnLog = false;
        }
    }
}
