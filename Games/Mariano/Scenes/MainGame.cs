using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.Mariano.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Scenes
{
    public class MainGame : Scene
    {
        Player p;

        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> stoppedEnemies = new List<Enemy>();

        Stopwatch enemySpawnRate = new Stopwatch();
        Stopwatch POWcooldown = new Stopwatch();

        #region Platforms
        List<Terrain> terrain = new List<Terrain>();
        Terrain ground;
        Terrain platC;
        Terrain platL1;
        Terrain platL2;
        Terrain platR1;
        Terrain platR2;

        List<Square> pipes = new List<Square>();
        Square pipeL1;
        Square pipeL2;
        Square pipeR1;
        Square pipeR2;

        POW POW;

        #endregion

        public MainGame(Camera c) : base(c)
        {
            Instantiate(p = new Player(
                new Vector3(0, -150, 0),
                Vector3.Zero,
                new Vector3(20, 40, 0),
                Color.White, Color.Red, 
                50f, 150f,
                new BoxCollider2D(p), 
                new Rigidbody(RigidbodyType.Dynamic, 100, 1),
                fillTexture: TextureManager.Get("mario")
            ));
            Instantiate(POW = new POW(
                new Vector3(0, -100, 0),
                Vector3.Zero,
                Vector3.One * 40,
                Color.White, Color.Blue,
                new BoxCollider2D(POW),
                fillTexture: TextureManager.Get("pow")
            ));

            terrain.Add(ground = new Terrain(
                new Vector3(0, -300, 0),
                Vector3.Zero,
                new Vector3(1000, 100, 0),
                Color.White, Color.Brown,
                new BoxCollider2D(ground),
                fillTexture: TextureManager.Get("ground"),
                mode: System.Drawing.Drawing2D.WrapMode.Tile,
                repeatTileX: 100,
                repeatTileY: 10
            ));
            //ground.mode = System.Drawing.Drawing2D.WrapMode.Tile;

            terrain.Add(platC = new Terrain(
                new Vector3(0, 25, 0),
                Vector3.Zero,
                new Vector3(400, 10, 0),
                Color.White, Color.LightBlue,
                new BoxCollider2D(platC),
                fillTexture: TextureManager.Get("platform"),
                mode: System.Drawing.Drawing2D.WrapMode.Tile,
                repeatTileX: 40
            ));
            terrain.Add(platL1 = new Terrain(
                new Vector3(-sceneCamera.size.X / 2, -100, 0),
                Vector3.Zero,
                new Vector3(500, 10, 0),
                Color.White, Color.LightBlue,
                new BoxCollider2D(platL1),
                fillTexture: TextureManager.Get("platform"),
                mode: System.Drawing.Drawing2D.WrapMode.Tile,
                repeatTileX: 50
            ));
            terrain.Add(platL2 = new Terrain(
                new Vector3(-sceneCamera.size.X / 2, 150, 0),
                Vector3.Zero,
                new Vector3(500, 10, 0),
                Color.White, Color.LightBlue,
                new BoxCollider2D(platL2),
                fillTexture: TextureManager.Get("platform"),
                mode: System.Drawing.Drawing2D.WrapMode.Tile,
                repeatTileX: 50
            ));
            terrain.Add(platR1 = new Terrain(
                new Vector3(sceneCamera.size.X / 2, -100, 0),
                Vector3.Zero,
                new Vector3(500, 10, 0),
                Color.White, Color.LightBlue,
                new BoxCollider2D(platR1),
                fillTexture: TextureManager.Get("platform"),
                mode: System.Drawing.Drawing2D.WrapMode.Tile,
                repeatTileX: 50
            ));
            terrain.Add(platR2 = new Terrain(
                new Vector3(sceneCamera.size.X / 2, 150, 0),
                Vector3.Zero,
                new Vector3(500, 10, 0),
                Color.White, Color.LightBlue,
                new BoxCollider2D(platR2),
                fillTexture: TextureManager.Get("platform"),
                mode: System.Drawing.Drawing2D.WrapMode.Tile,
                repeatTileX: 50
            ));

            pipes.Add(pipeL1 = new Square(
                new Vector3(-sceneCamera.size.X / 2, -225, 0),
                new Vector3(0, 0, 180),
                new Vector3(50, 50, 50),
                Color.White, Color.DarkGreen,
                fillTexture: TextureManager.Get("pipe")
            ));
            pipes.Add(pipeL2 = new Square(
                new Vector3(-sceneCamera.size.X / 2, 180, 0),
                new Vector3(0, 0, 180),
                new Vector3(50, 50, 50),
                Color.White, Color.DarkGreen,
                fillTexture: TextureManager.Get("pipe")
            ));
            pipes.Add(pipeR1 = new Square(
                new Vector3(sceneCamera.size.X / 2, -225, 0),
                Vector3.Zero,
                new Vector3(50, 50, 50),
                Color.White, Color.DarkGreen,
                fillTexture: TextureManager.Get("pipe")
            ));
            pipes.Add(pipeR2 = new Square(
                new Vector3(sceneCamera.size.X / 2, 180, 0),
                Vector3.Zero,
                new Vector3(50, 50, 50),
                Color.White, Color.DarkGreen,
                fillTexture: TextureManager.Get("pipe")
            ));

            foreach (var t in terrain) Instantiate(t);
        }

        public override void Init()
        {
            p.score = 0;
            p.hp = 3;
            p.position = new Vector3(0, -150, 0);
            p.isGrounded = false;
            enemySpawnRate.Restart();
            AudioManager.PlayMusic("music", true, 0);
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Black);
            p.Draw(g, sceneCamera);
            foreach (Enemy e in enemies) e.Draw(g, sceneCamera);
            foreach (var p in pipes) p.Draw(g, sceneCamera);
            if (POW.isActive) POW.Draw(g, sceneCamera);
            foreach (Terrain t in terrain) t.Draw(g, sceneCamera);

            DrawingUtils.DrawCenterText(g, $"Score: {p.score}", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, 10));
            DrawingUtils.DrawCenterText(g, $"HP: {p.hp}", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, 40));
        }

        public override void ProcessInput() { }

        public override void Update(float dt)
        {
            if (p.hp == 0)
            {
                List<GameObject> cleaning = new List<GameObject>();
                foreach (Enemy e in enemies) cleaning.Add(e);
                foreach (GameObject e in cleaning) { Destroy(e); enemies.Remove(e as Enemy); }

                MarianoHermanos.lb.AddScore(p.score);
                MarianoHermanos.lastScore = p.score;
                SaveManager.Save("score.json");
                SceneManager.SetActive("gameover");
            }
            if (p.position.X + p.size.X < -sceneCamera.size.X / 2) {
                if (p.position.Y < -200) p.position.Y = 156;
                p.position.X = sceneCamera.size.X / 2 + p.size.X / 2;  p.position.Y += 1;
            }
            if (p.position.X - p.size.X >  sceneCamera.size.X / 2) { 
                if (p.position.Y < -200) p.position.Y = 156;
                p.position.X = -sceneCamera.size.X / 2 - p.size.X / 2; p.position.Y += 1;
            }

            if (POW.gotActivated)
            {
                foreach (Enemy e in enemies)
                {
                    e.StopMovement();
                    stoppedEnemies.Add(e);
                }
                POWcooldown.Start();
                POW.gotActivated = false;
            }

            if (POWcooldown.ElapsedMilliseconds / 1000 > 5)
            {
                POWcooldown.Reset();
                foreach (Enemy e in stoppedEnemies) e.ContinueMovement();
                stoppedEnemies.Clear();
            }

            foreach (Enemy e in enemies)
            {
                if (e.position.X + e.size.X < -sceneCamera.size.X / 2)
                {
                    if (e.position.Y < -200) e.position.Y = 155;
                    e.position.X = sceneCamera.size.X / 2 + e.size.X / 2; e.position.Y += 1;
                }
                if (e.position.X - e.size.X > sceneCamera.size.X / 2) {
                    if (e.position.Y < -200) e.position.Y = 155;
                    e.position.X = -sceneCamera.size.X / 2 - e.size.X / 2; e.position.Y += 1; 
                }
            }

            foreach (Enemy e in enemies)
            {
                if (e.hp == 0)
                {
                    p.score += 100 * e.maxHp;
                    Destroy(e);
                    enemies.Remove(e);
                    AudioManager.PlaySFX("enemyDeath");
                    break;
                }
            }
            

            GenerateEnemy();
        }

        private void GenerateEnemy()
        {
            if (enemySpawnRate.ElapsedMilliseconds / 1000 > 2)
            {
                enemySpawnRate.Restart();
                Random rnd = new Random();
                int N = rnd.Next(0, 2) == 0 ? 0 : 1;
                int hp = rnd.Next(0, 2) == 0 ? 2 : 0;
                float posX = N == 0 ? pipeL2.position.X : pipeR2.position.X; 
                float posY = N == 0 ? pipeL2.position.Y : pipeR2.position.Y;
                Enemy e = new Enemy(
                    new Vector3(posX, posY, 0),
                    Vector3.Zero,
                    new Vector3(40, 30, 0),
                    Color.White, Color.Green,
                    N, 1 + hp,
                    body: new Rigidbody(RigidbodyType.Dynamic, 10, 1),
                    fillTexture: TextureManager.Get("enemy2")
                );
                e.CreateCollision();
                enemies.Add(e); Instantiate(e);
                Console.WriteLine("Spawned");
            }
        }
    }
}
 