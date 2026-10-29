using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects.Scenery;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Nodes;
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
        private Square weapon; 
        private float respawnTimer = 0;
        private float winTimer = 0;
        private float wepSizeY;

        Parallax bg;
        Parallax trees;
        Parallax bg2;
        Parallax trees2;

        public Square sky;

        public Battle(Camera c) : base(c)
        {
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
                Vector3.One * 25,
                Color.Black, Color.White,
                fillTexture: TextureManager.Get("bossIndicator")
            );

            weapon = new Square(
                Vector3.Zero, Vector3.Zero,
                new Vector3(66, 28, 0),
                Color.White, Color.White,
                fillTexture: TextureManager.Get("undertaker")
            );
            wepSizeY = 28;

            bg = new Parallax(
                TextureManager.Get("groundVanilla"),
                new Vector3(0, -272.5f, 0),
                new Vector3(400, 160, 0),
                sceneCamera.position.X - sceneCamera.size.X / 2
            );
            trees = new Parallax(
                TextureManager.Get("treesVanilla"),
                new Vector3(0, -75, 0),
                new Vector3(400, 240, 0),
                -sceneCamera.size.X / 2
            );

            bg2 = new Parallax(
                TextureManager.Get("groundVanilla"),
                new Vector3(0, -200, 0),
                new Vector3(600, 240, 0),
                -sceneCamera.size.X / 2
            );

            trees2 = new Parallax(
                TextureManager.Get("treesVanilla"),
                new Vector3(0, 25, 0),
                new Vector3(400, 240, 0),
                -sceneCamera.size.X / 2
            );

            sky = new Square(
                Vector3.Zero, Vector3.Zero,
                sceneCamera.size,
                Color.White, Color.White,
                fillTexture: TextureManager.Get("skyNight")
            );

        }

        public override void Init() {
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

            boss.InitStats();
            switch (TBS.config.weapon)
            {
                case 0: weapon.fillTexture = TextureManager.Get("undertaker"); p.SetGunStats(26, 2, 0.33f); break; // Undertaker
                case 1: weapon.fillTexture = TextureManager.Get("musket");     p.SetGunStats(38, 0, 0.53f); p.critChance += 0.08f; break; // Musket
                case 2: weapon.fillTexture = TextureManager.Get("minishark");  p.SetGunStats(13, 4, 0.13f); break; // Minishark
                case 3: weapon.fillTexture = TextureManager.Get("vortex");     p.SetGunStats(57, 4, 0.083f); break;
            }
            if (TBS.config.equipment[2]) {
                p.speed *= 1.1f;
                p.maxSpeed *= 1.1f;
            }
            if (TBS.config.equipment[3]) {
                p.panicSpeed = p.speed * 2;
                p.panicMaxSpeed = p.maxSpeed * 2;
            }
            if (TBS.config.equipment[4]) p.regenRate /= 3;
            if (TBS.config.equipment
                [5]) p.defense += 5;
            if (TBS.config.equipment[6]) p.maxiFrames *= 2;
            if (TBS.config.equipment[7]){
                float newDamage = p.gunDamage * 1.1f;
                p.gunDamage = (int)newDamage;
                p.critChance += 0.05f;
            }
            lost = false;
            bossDefeated = false;

            winTimer = 0;

            respawnTimer = 0;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            sky.Draw(g, sceneCamera);
            trees2.RenderParallax(g, sceneCamera);
            bg2.RenderParallax(g, sceneCamera);
            trees.RenderParallax(g, sceneCamera);
            bg.RenderParallax(g, sceneCamera);
            if (bullets.Count > 0) foreach (Proyectile b in bullets) b.Draw(g, sceneCamera);
            if (!bossDefeated) boss.Draw(g, sceneCamera);
            if (minions.Count > 0) foreach (Enemy m in minions) m.Draw(g, sceneCamera);
            if (!lost) {
                p.Draw(g, sceneCamera);
                weapon.Draw(g, sceneCamera);
            }
            if (boss is Boss2) g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new RectangleF(0, 0, sceneCamera.size.X, sceneCamera.size.Y));

            if (p.panic)
            {
                panic.Draw(g, sceneCamera);
                g.DrawString($"{5f - p.panicTimer:F1}", FontManager.Get("andyBold", 26), (boss is Boss2) ? Brushes.White : Brushes.Black, new PointF(25, 75));
            }

            if (!lost)
            {
                float hpLeft = (float)p.hp / 400;
                Point hpPos = new Point((int)sceneCamera.size.X / 2 + 75, 25);
                g.FillRectangle(Brushes.Red, new RectangleF(hpPos.X, hpPos.Y, 300f * hpLeft, 40));
                g.DrawRectangle(new Pen(Brushes.Black, 4f), new Rectangle(hpPos.X, hpPos.Y, 300, 40));
                DrawingUtils.DrawCenterText(g, $"Health : {p.hp}/400", FontManager.Get("andyBold", 26), (boss is Boss2) ? Color.White : Color.Black,
                    new PointF(sceneCamera.size.X / 1.5f + 100, 75));
                
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
                DrawingUtils.DrawCenterText(g, TBS.bossName, FontManager.Get("andyBold", 26), Color.Black, new PointF(hpPos.X + 300, hpPos.Y + 50));
                DrawingUtils.DrawCenterText(g, $"{boss.hp}/{boss.maxHp}", FontManager.Get("andyBold", 26), Color.Black, new PointF(hpPos.X + 300, hpPos.Y + 5));
                

                float dist = Vector3.Distance(p.position, boss.position);
                if (dist > sceneCamera.size.X / 1.5f) {
                    bossIndicator.Draw(g, sceneCamera);
                    DrawingUtils.DrawCenterText(g, $"{dist/30:F0} m", FontManager.Get("andyBold", 28), (boss is Boss2) ? Color.White : Color.Black,
                        new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
                }
            }else
            {
                if (!lost)
                {
                    DrawingUtils.DrawCenterText(g, "Victory!", FontManager.Get("andyBold", 28), Color.Green,
                        new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 35));
                    DrawingUtils.DrawCenterText(g, $"{10 - winTimer:F0}", FontManager.Get("andyBold", 28), Color.Green,
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

            List<GameObject> removeList = new List<GameObject>();

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

                Vector3 mouse = sceneCamera.GetCursorOnWorld();
                float wepAngle = MathUtils.GetAngleFrom2Points(p.position, mouse);
                weapon.rotation.Z = wepAngle;
                if (wepAngle < 0) wepAngle += 360;
                if (wepAngle >= 90 && wepAngle < 270) weapon.size.Y = -wepSizeY;
                else weapon.size.Y = wepSizeY;
                    weapon.position = p.position;

                bool followPlayer = p.position.Y > -50;
                sceneCamera.position = new Vector3(p.position.X, followPlayer ? p.position.Y : -50, sceneCamera.position.Z);
            }
            else
            {
                respawnTimer += dt;
                if (respawnTimer >= 10)
                {
                    respawnTimer = 0;
                    sceneCamera.position.X = 0;
                    sceneCamera.position.Y = 0;

                    foreach (Proyectile b in bullets) removeList.Add(b);
                    foreach (Enemy m in minions) removeList.Add(m);

                    foreach (GameObject go in removeList)
                    {
                        if (go is Proyectile b) bullets.Remove(b);
                        if (go is Enemy e) minions.Remove(e);
                        Destroy(go);
                    }
                    removeList.Clear();

                    AudioManager.PlayMusic("title", true, 5000);
                    SceneManager.SetActive("weaponSelection");
                }
            }

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
                    winTimer = 0;
                    AudioManager.PlaySFX("death");
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
            else
            {
                if (!lost)
                {
                    winTimer += dt;
                    if (winTimer >= 10)
                    {
                        winTimer = 0;
                        sceneCamera.position.X = 0;
                        sceneCamera.position.Y = 0;
                        if (boss is Boss1) TBS.config.boss1Defeated = true;
                        if (boss is Boss2) TBS.config.boss2Defeated = true;

                        foreach (Proyectile b in bullets) removeList.Add(b);
                        foreach (Enemy m in minions) removeList.Add(m);

                        foreach (GameObject go in removeList) {
                            if (go is Proyectile b) bullets.Remove(b);
                            if (go is Enemy e) minions.Remove(e);
                            Destroy(go);
                        }
                        removeList.Clear();


                        SaveManager.Save("config.json");
                        SceneManager.SetActive("titleScreen");
                    }
                }
            }

            panic.position = sceneCamera.position + new Vector3(-sceneCamera.size.X / 2 + 50, sceneCamera.size.Y / 2 - 50, -sceneCamera.position.Z);

            sky.position = sceneCamera.position - new Vector3(0, 0, sceneCamera.position.Z);

            bg.limitsL = sceneCamera.position.X + sceneCamera.size.X / 2;
            bg.limitsR = sceneCamera.position.X - sceneCamera.size.X / 2;

            trees.limitsL = sceneCamera.position.X + sceneCamera.size.X / 2;
            trees.limitsR = sceneCamera.position.X - sceneCamera.size.X / 2;

            bg2.limitsL = sceneCamera.position.X + sceneCamera.size.X / 2;
            bg2.limitsR = sceneCamera.position.X - sceneCamera.size.X / 2;

            trees2.limitsL = sceneCamera.position.X + sceneCamera.size.X / 2;
            trees2.limitsR = sceneCamera.position.X - sceneCamera.size.X / 2;


            bg.UpdateParallax(0 * dt);
            trees.UpdateParallax(0 * dt);
            bg2.UpdateParallax(p.body.velocity.X * -0.5f * dt);
            trees2.UpdateParallax(p.body.velocity.X * -0.4f * dt);
        }
    }
}
