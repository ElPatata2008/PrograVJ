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
        Random rand = new Random();

        bool gameOver = false;
        bool starting = true;
        Stopwatch sw = new Stopwatch();
        Stopwatch timeLimit = new Stopwatch();
        
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
                Color.LightSlateGray, Color.Aqua
            ));
            terrain.Add(water1);

            Instantiate(water2 = new Square(
                new Vector3(0, 700, 0),
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, 250, 0),
                Color.LightSlateGray, Color.Aqua
            ));
            terrain.Add(water2);
        }

        public override void Init()
        {
            Instantiate(player = new Player(
                startPos,
                Vector3.Zero,
                new Vector3(50, 50, 50),
                Color.Black, Color.White, new CircleCollider2D(player)
            )
            { speed = 10, hp = 3 });
            sw.Restart();
            starting = true;
            rand = new Random();
            sceneCamera.position = cameraStartPos;
            CreateHazards();
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.DarkGreen);
            foreach (GameObject t in terrain) t.Draw(g, sceneCamera);
            foreach (GameObject c in hazards) if (c != null) c.Draw(g, sceneCamera);

            if (starting) DrawingUtils.DrawCenterText(g, 
                $"Starting in {3 - sw.ElapsedMilliseconds / 1000}", FontManager.Get("byte", 36), Color.White, 
                new PointF(sceneCamera.position.X + sceneCamera.size.X / 2, sceneCamera.position.Y + sceneCamera.size.Y / 2));
            if (gameOver) DrawingUtils.DrawCenterText(g,
                $"Restarting in {3 - sw.ElapsedMilliseconds / 1000}", FontManager.Get("byte", 36), Color.White,
                new PointF(sceneCamera.position.X + sceneCamera.size.X / 2, sceneCamera.position.Y + sceneCamera.size.Y / 2));
            player.Draw(g, sceneCamera);
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

                    if (player.position.Y >= 0 && player.position.Y <= 600) sceneCamera.position = new Vector3(sceneCamera.position.X, player.position.Y, sceneCamera.position.Z);
                    if (player.position.Y == 850) GameOver(true);
                }
                else
                {
                    if (sw.ElapsedMilliseconds / 1000 > 2)
                    {
                        if (Frogger.Frogger.victory || player.hp == 0) SceneManager.SetActive("gameover");
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
                float Xsize = type ? Frogger.Frogger.tileSize * 2 : Frogger.Frogger.tileSize * 3;
                float Xpos = dir ? -sceneCamera.size.X / 2 - Xsize / 2 : sceneCamera.size.X / 2 + Xsize / 2;
                Car car = new Car(
                    new Vector3(Xpos, road1.position.Y + (pos) * Frogger.Frogger.tileSize, 0),
                    new Vector3(0, 0, dir ? 0 : 180),
                    new Vector3(Xsize, Frogger.Frogger.tileSize, 0),
                    Color.White, type ? Color.Red : Color.Blue
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
                float Xsize = type ? Frogger.Frogger.tileSize * 2 : Frogger.Frogger.tileSize * 3;
                float Xpos = dir ? -sceneCamera.size.X / 2 - Xsize / 2 : sceneCamera.size.X / 2 + Xsize / 2;
                Car car = new Car(
                    new Vector3(Xpos, road2.position.Y + (pos) * Frogger.Frogger.tileSize, 0),
                    new Vector3(0, 0, dir ? 0 : 180),
                    new Vector3(Xsize, Frogger.Frogger.tileSize, 0),
                    Color.White, type ? Color.Red : Color.Blue,
                    new CircleCollider2D(hazards[i])
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
                    Color.White, Color.Brown
                );
                log.CreateCollider();
                //Console.WriteLine($"Log created at {log.position.Y}, rotation {log.rotation.Z}");
                hazards.Add(log); Instantiate(log);
            }
        }

        private void RestartRound()
        {
            player.position = startPos;
            sw.Restart();
            starting = true;
            gameOver = false;
            sceneCamera.position = cameraStartPos;
            foreach(var h in hazards) Destroy(h);
            hazards.Clear();
            CreateHazards();
        }

        private void GameStart()
        {
            sw.Reset();
            starting = false;
            timeLimit.Start();
        }

        private void GameOver(bool victory)
        {
            gameOver = true; 
            sw.Start(); 
            Frogger.Frogger.victory = victory;
        }

        private void CheckCollisions()
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
                        if (!player.isMoving)
                        {
                            player.position.X += log.rotation.Z == 180 ? -log.speed : log.speed;
                        }
                    }
                    else
                    {
                        
                    }
                }
            } 
        }
    }
}
