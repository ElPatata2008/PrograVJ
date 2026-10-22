using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.Cuphead.Objects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace PrograVJ.Games.Cuphead.Scenes
{
    public class Boss1Battle : Scene
    {
        Blackboard bb = new Blackboard();
        Boss1 boss;
        List<MinionBoss1> minions = new List<MinionBoss1>();
        Player p;
        List<PlayerBullet> bullets = new List<PlayerBullet>();
        Square ground;

        //float ground = -200f;
        public Boss1Battle(Camera c) : base(c)
        {
            Instantiate(p = new Player(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(25, 50, 0),
                Color.Black, Color.White,
                new BoxCollider2D(p),
                new Rigidbody(RigidbodyType.Dynamic, 50, 1)
            ));

            Instantiate(boss = new Boss1(
                new Vector3(200, 0, 0),
                Vector3.Zero,
                new Vector3(200, 100, 0),
                Color.Black, Color.DeepSkyBlue,
                new BoxCollider2D(boss),
                new Rigidbody(RigidbodyType.Dynamic, 50, 1),
                new AIController(bb, new FSM())
            ));

            ground = new Square(
                new Vector3(p.position.X, -sceneCamera.size.Y / 2 - 200, 0),
                Vector3.Zero,
                new Vector3(sceneCamera.size.X * 1.5f, sceneCamera.size.Y, 0),
                Color.RosyBrown, Color.Brown
            );
        }

        public override void Init()
        {
            Console.WriteLine(TerrariaBattleSimulator.config.difficulty);
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            if (bullets.Count > 0) foreach (PlayerBullet b in bullets) b.Draw(g, sceneCamera);
            boss.Draw(g, sceneCamera);
            p.Draw(g, sceneCamera);
            ground.position.X = p.position.X;
            ground.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("escape"))
            {
                SaveManager.Save("config.json");
                Environment.Exit(0);
            }

            //if (InputManager.JustPressedInput("w")) TerrariaBattleSimulator.config.difficulty = 0;
        }

        public override void Update(float dt)
        {
            //Console.WriteLine($"{InputManager.MouseLocation} | {p.position}");
            //Console.WriteLine($"{p.position.X + InputManager.MouseLocation.X} | {p.position.X}");

            if (InputManager.MBL)
            {
                if (p.attackDelay.ElapsedMilliseconds / 1000 > 0.5f)
                {
                    PlayerBullet b = p.Shoot(sceneCamera);
                    Instantiate(b);
                    bullets.Add(b);
                }
            }

            List<GameObject> removeList = new List<GameObject>();

            foreach (PlayerBullet b in bullets) {
                if (b.expire > 10f) removeList.Add(b);
            }

            foreach (GameObject go in removeList)
            {
                if (go is PlayerBullet b) bullets.Remove(b);
                Destroy(go);
            }

            removeList.Clear();

            bool followPlayer = p.position.Y > -50;
            sceneCamera.position = new Vector3(p.position.X, followPlayer ? p.position.Y : -50, sceneCamera.position.Z);

            boss.facingRight = p.position.X > boss.position.X;
        }
    }
}
