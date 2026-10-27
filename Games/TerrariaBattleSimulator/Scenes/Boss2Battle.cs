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
    public class Boss2Battle : Battle
    {
        public Boss2Battle(Camera c) : base(c)
        {
            Instantiate(boss = new Boss2(
                new Vector3(-600, 0, 0),
                Vector3.Zero,
                new Vector3(120, 120, 0),
                Color.Black, Color.White,
                new CircleCollider2D(boss),
                new Rigidbody(RigidbodyType.Dynamic, 50, 0),
                new AIController(bb, new FSM()),
                fillTexture: TextureManager.Get("boss2phase1"),
                animationController: new AnimationController()
            ));
        }

        public override void Init()
        {
            base.Init();
            AudioManager.PlayMusic("boss2", true, 5000);
        }

        public override void PaintScreen(Graphics g)
        {
            base.PaintScreen(g);
        }

        public override void ProcessInput()
        {
            base.ProcessInput();
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (!bossDefeated)
            {
                if (boss is Boss2 b)
                {
                    if (b.spawnEnemy)
                    {
                        MinionBoss2 m = b.SpawnMinion();
                        Instantiate(m);
                        minions.Add(m);
                        b.spawnEnemy = false;
                    }

                    if (b.spawnLaser)
                    {
                        Boss2Laser l = b.Shoot();
                        Instantiate(l);
                        bullets.Add(l);
                        b.spawnLaser = false;
                    }
                }
            }
        }
    }
}
