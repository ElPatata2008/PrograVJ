using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.TerrariaBattleSimulator.Scenes
{
    public class Battle : Scene
    {
        public Blackboard bb = new Blackboard();
        public Enemy boss;
        public List<Enemy> minions = new List<Enemy>();
        public Player p;
        public List<Proyectile> bullets = new List<Proyectile>();
        public Square ground;
        public bool bossDefeated = false;
        public bool lost = false;
        private Square panic;
        private Square bossIndicator;
        private float respawnTimer = 0;

        public Battle(Camera c) : base(c)
        {
            Instantiate(p = new Player(
                Vector3.Zero, Vector3.Zero,
                new Vector3(25, 50, 0),
                Color.Black, Color.White,
                new BoxCollider2D(p),
                new Rigidbody(RigidbodyType.Dynamic, 50, 1)
            ));

            ground = new Square(
                new Vector3(p.position.X, -sceneCamera.size.Y / 2 - 200, 0),
                Vector3.Zero,
                new Vector3(sceneCamera.size.X * 1.5f, sceneCamera.size.Y, 0),
                Color.Green, Color.Brown, borderWidth: 4f
            );

            panic = new Square(
                new Vector3(-sceneCamera.size.X / 2 + 75, sceneCamera.size.Y / 2 - 75, 0),
                Vector3.Zero,
                Vector3.One * 50,
                Color.Black, Color.White,
                fillTexture: TextureManager.Get("panic")
            );
            bossIndicator = new Square(
                new Vector3(sceneCamera.position.X, sceneCamera.position.Y, 0),
                Vector3.Zero,
                Vector3.One * 50,
                Color.Black, Color.White
                //fillTexture: TextureManager.Get("bossIndicator")
            );
        }

        public override void Init() {
            boss.InitStats();
            switch (TBS.config.weapon)
            {
                case 0: p.SetGunStats(26, 2, 0.33f); break; // Undertaker
                case 1: p.SetGunStats(38, 0, 0.53f); p.critChance += 0.08f; break; // Musket
                case 2: p.SetGunStats(13, 4, 0.13f); break; // Minishark
                case 3: p.SetGunStats(57, 4, 0.083f); break;
            }
            if (TBS.config.equipment[2]) {
                p.speed *= 1.1f;
                p.maxSpeed *= 1.1f;
            }
            if (TBS.config.equipment[3]) {
                p.panicSpeed = p.speed * 2;
                p.panicMaxSpeed = p.maxSpeed * 2;
            }
            if (TBS.config.equipment[4]) p.regen += 2;
            if (TBS.config.equipment
                [5]) p.defense += 5;
            if (TBS.config.equipment[6]) p.maxiFrames *= 2;
            if (TBS.config.equipment[7]){
                float newDamage = p.gunDamage * 1.1f;
                p.gunDamage = (int)newDamage;
                p.critChance += 0.05f;
            }
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            if (bullets.Count > 0) foreach (Proyectile b in bullets) b.Draw(g, sceneCamera);
            ground.position.X = p.position.X;
            ground.Draw(g, sceneCamera);
            if (!bossDefeated) boss.Draw(g, sceneCamera);
            if (minions.Count > 0) foreach (Enemy m in minions) m.Draw(g, sceneCamera);
            if (p.panic)
            {
                panic.Draw(g, sceneCamera);
                g.DrawString($"{5f - p.panicTimer:F1}", FontManager.Get("andyBold", 26), Brushes.Black, new PointF(25, 75));
            }

            if (!lost)
            {
                float hpLeft = (float)p.hp / 400;
                Point hpPos = new Point((int)sceneCamera.size.X / 2 + 75, 25);
                g.FillRectangle(Brushes.Red, new RectangleF(hpPos.X, hpPos.Y, 300f * hpLeft, 40));
                g.DrawRectangle(new Pen(Brushes.Black, 4f), new Rectangle(hpPos.X, hpPos.Y, 300, 40));
                DrawingUtils.DrawCenterText(g, $"Health : {p.hp}/400", FontManager.Get("andyBold", 26), Color.Black,
                    new PointF(sceneCamera.size.X / 1.5f + 100, 75));
                p.Draw(g, sceneCamera);
            } else
            {
                DrawingUtils.DrawCenterText(g, "You were slain...", FontManager.Get("andyBold", 28), Color.Red,
                    new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 35));
                DrawingUtils.DrawCenterText(g, $"{10 - respawnTimer:F0}", FontManager.Get("andyBold", 28), Color.Red,
                    new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
            }

            if (!bossDefeated)
            {
                float hpLeft = (float)boss.hp / (float)boss.maxHp;
                Point hpPos = new Point((int)(sceneCamera.size.X / 2 - 300), (int)(sceneCamera.size.Y - 100));
                g.FillRectangle(Brushes.Red, new RectangleF(hpPos.X, hpPos.Y, 600f * hpLeft, 40));
                g.DrawRectangle(new Pen(Brushes.Black, 4f), new Rectangle(hpPos.X, hpPos.Y, 600, 40));
                float dist = Vector3.Distance(p.position, boss.position);
                if (dist > sceneCamera.size.X / 1.5f) {
                    bossIndicator.Draw(g, sceneCamera);
                    DrawingUtils.DrawCenterText(g, $"{dist/30:F0} m", FontManager.Get("andyBold", 28), Color.Black,
                        new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
                }
            }
            
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("escape"))
            {
                SaveManager.Save("config.json");
                Environment.Exit(0);
            }
        }

        public override void Update(float dt)
        {
            float angle = MathUtils.GetAngleFrom2Points(sceneCamera.position, boss.position);
            Vector2 dir = MathUtils.GetDirectionFromAngle(angle);

            bossIndicator.rotation.Z = angle;
            bossIndicator.position.X = sceneCamera.position.X + dir.X * 100;
            bossIndicator.position.Y = sceneCamera.position.Y + dir.Y * 100;



            if (!lost)
            {
                if (!bossDefeated) boss.UpdatePlayerPos(p.position);
                foreach (Enemy minion in minions) minion.UpdatePlayerPos(p.position);

                if (InputManager.MBL)
                {
                    if (p.attackDelay > p.firerate)
                    {
                        PlayerBullet b = p.Shoot(sceneCamera);
                        Instantiate(b);
                        bullets.Add(b);
                    }
                }

                bool followPlayer = p.position.Y > -50;
                sceneCamera.position = new Vector3(p.position.X, followPlayer ? p.position.Y : -50, sceneCamera.position.Z);
            }
            else
            {
                respawnTimer += dt;
                if (respawnTimer >= 10)
                {
                    SceneManager.SetActive("weaponSelection");
                    AudioManager.PlayMusic("title", true, 5000);
                    sceneCamera.position.X = 0;
                    sceneCamera.position.Y = 0;
                }
            }

            List<GameObject> removeList = new List<GameObject>();

            foreach (Proyectile b in bullets)
            {
                if (b.landed) removeList.Add(b);
                if (b.expire > 4f) removeList.Add(b);
            }

            foreach (Enemy m in minions) if (m.hp < 1) removeList.Add(m);

            foreach (GameObject go in removeList) {
                if (go is Proyectile b) bullets.Remove(b);
                if (go is Enemy e) minions.Remove(e);
                Destroy(go);
            }
            removeList.Clear();

            if (!lost) {
                if (p.hp < 1) {
                    lost = true;
                    Destroy(p);
                }
            }

            if (!bossDefeated) {
                if (boss.hp < 1) {
                    bossDefeated = true;
                    AudioManager.PlayMusic("title", true, 5000);
                    Destroy(boss);
                }
            }

            panic.position = sceneCamera.position + new Vector3(-sceneCamera.size.X / 2 + 50, sceneCamera.size.Y / 2 - 50, -sceneCamera.position.Z);
        }
    }
}
