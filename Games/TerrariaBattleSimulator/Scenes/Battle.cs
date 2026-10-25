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

        public Battle(Camera c) : base(c)
        {
            Instantiate(p = new Player(
                Vector3.Zero,
                Vector3.Zero,
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
        }

        public override void Init() { }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            if (bullets.Count > 0) foreach (Proyectile b in bullets) b.Draw(g, sceneCamera);
            ground.position.X = p.position.X;
            ground.Draw(g, sceneCamera);
            if (!bossDefeated) boss.Draw(g, sceneCamera);
            if (minions.Count > 0) foreach (Enemy m in minions) m.Draw(g, sceneCamera);
            p.Draw(g, sceneCamera);
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

            List<GameObject> removeList = new List<GameObject>();

            foreach (Proyectile b in bullets)
            {
                if (b.landed) removeList.Add(b);
                if (b.expire > 4f) removeList.Add(b);
            }

            foreach (Enemy m in minions) if (m.hp < 1) removeList.Add(m);

            foreach (GameObject go in removeList)
            {
                if (go is Proyectile b) bullets.Remove(b);
                if (go is Enemy e) minions.Remove(e);
                Destroy(go);
            }
            removeList.Clear();

            if (!bossDefeated)
            {
                if (boss.hp < 1)
                {
                    bossDefeated = true;
                    AudioManager.PlayMusic("title", true, 5000);
                    Destroy(boss);
                }
            }


            bool followPlayer = p.position.Y > -50;
            sceneCamera.position = new Vector3(p.position.X, followPlayer ? p.position.Y : -50, sceneCamera.position.Z);
        }
    }
}
