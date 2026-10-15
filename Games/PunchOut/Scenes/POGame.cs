using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.PunchOut.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.PunchOut.Scenes
{
    public class POGame : Scene
    {
        Player player;
        Enemy enemy;
        EnemyPunch EnemyPunch;
        PlayerPunch playerPunch;
        Square bg;

        bool swStarted = false;
        bool win = false;
        bool gameOver = false;
        Stopwatch sw = new Stopwatch();
        Stopwatch gameSW = new Stopwatch();
        public POGame(Camera c) : base(c)
        {
            Instantiate(player = new Player(
                new Vector3(0, -50, 0),
                Vector3.Zero,
                new Vector3(50, 178, 0),
                Color.White, Color.White, new BoxCollider2D(player),
                animationController: new AnimationController()
            ));

            Instantiate(enemy = new Enemy(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(66, 226f, 0),
                Color.White, Color.White, new BoxCollider2D(enemy),
                animationController: new AnimationController()
            ));

            Instantiate(EnemyPunch = new EnemyPunch(
                new Vector3(enemy.position.X - 5, enemy.position.Y - 30, 0),
                Vector3.Zero,
                new Vector3(25, 25, 0),
                Color.Red, Color.Red, new BoxCollider2D(EnemyPunch)
            ));

            Instantiate(playerPunch = new PlayerPunch(
                new Vector3(player.position.X + 10, player.position.Y + 65, 0),
                Vector3.Zero,
                new Vector3(25, 25, 0),
                Color.Blue, Color.Blue, new BoxCollider2D(playerPunch)
            ));

            enemy.punch = EnemyPunch;
            player.punch = playerPunch;

            bg = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, sceneCamera.size.Y, 0),
                Color.White, Color.White,
                fillTexture: TextureManager.Get("bg")
            );
        }

        public override void Init() {
            player.StartTimer();
            enemy.StartTime();
            AudioManager.PlayMusic("music", true, 0);
            gameSW.Start(); 
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            bg.Draw(g, sceneCamera);
            enemy.Draw(g, sceneCamera);
            player.Draw(g, sceneCamera);

            DrawingUtils.DrawCenterText(g, "Little Mac", FontManager.Get("po", 16), Color.Black, new PointF(135, 70));
            g.FillRectangle(Brushes.Red, new RectangleF(30, 35, player.hp * 2, 30));
            g.DrawRectangle(new Pen(Brushes.Black, 5), new Rectangle(30, 35, 200, 30));

            if (player.strongatk.ElapsedMilliseconds / 1000 > 4)
            {
                Square star = new Square(
                    new Vector3(-110, sceneCamera.size.Y / 2 - 60, 0),
                    Vector3.Zero,
                    Vector3.One * 50,
                    Color.White, Color.White, fillTexture: TextureManager.Get("star")
                );
                star.Draw(g, sceneCamera);
            }

            DrawingUtils.DrawCenterText(g, "Glass Joe", FontManager.Get("po", 16), Color.Black, new PointF(sceneCamera.size.X - 125, 70));
            g.FillRectangle(Brushes.Red, new RectangleF(sceneCamera.size.X - 230, 35, enemy.hp * 2, 30));
            g.DrawRectangle(new Pen(Brushes.Black, 5), new Rectangle((int)sceneCamera.size.X - 230, 35, 200, 30));

            g.FillRectangle(Brushes.Black, new RectangleF(sceneCamera.size.X / 2 - 60, 30, 120, 50));
            int timeLeft = 180 - (int)gameSW.ElapsedMilliseconds / 1000;
            int seconds = timeLeft % 60;
            int minutes = timeLeft / 60;
            DrawingUtils.DrawCenterText(g, $"{minutes}:{seconds}", FontManager.Get("po", 18), Color.White, new PointF(sceneCamera.size.X / 2 + 4, 40));
        }

        public override void ProcessInput()
        {
            player.dl = InputManager.JustPressedInput("left");
            player.dr = InputManager.JustPressedInput("right");
            player.block = InputManager.JustPressedInput("down");
            player.atk1 = InputManager.JustPressedInput("z");
            player.atk2 = InputManager.JustPressedInput("x");
        }

        public override void Update(float dt)
        {
            if (enemy.hp <= 0 || player.hp <= 0 && !swStarted)
            {
                swStarted = true;
                gameSW.Stop();
                if (enemy.hp <= 0) player.gameover = true;
                if (player.hp <= 0)
                {
                    enemy.gameover = true;
                    PunchOut.endType = 2;
                }
                sw.Start();
            }

            if (sw.ElapsedMilliseconds / 1000 > 3 && !gameOver)
            {
                if (enemy.hp <= 0)
                {
                    player.animationController.Play("win");
                    PunchOut.endType = 1;
                }
                AudioManager.PlaySFX("crowd");
                gameOver = true;
            }

            if (sw.ElapsedMilliseconds / 1000 > 6)
            {
                SceneManager.SetActive("end");
            }
            
            if (gameSW.ElapsedMilliseconds / 1000 > 180)
            {
                PunchOut.endType = 3;
                SceneManager.SetActive("end");
            }
        }
    }
}
