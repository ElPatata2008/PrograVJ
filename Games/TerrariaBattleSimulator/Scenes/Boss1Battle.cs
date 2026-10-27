using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Scenes
{
    public class Boss1Battle : Battle
    {
        bool explode = false;
        Random rnd = new Random();
        public Boss1Battle(Camera c) : base(c)
        {
            Instantiate(boss = new Boss1(
                new Vector3(600, 0, 0),
                Vector3.Zero,
                new Vector3(200, 150, 0),
                Color.Black, Color.DeepSkyBlue,
                new BoxCollider2D(boss),
                new Rigidbody(RigidbodyType.Dynamic, 50, 1),
                new AIController(bb, new FSM()),
                animationController: new AnimationController()
            ));
        }

        public override void Init()
        {
            base.Init();
            Console.WriteLine(TBS.config.difficulty);
            AudioManager.PlayMusic("boss1", true, 5000);
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

            if (!bossDefeated) {
                if (boss is Boss1 b) {
                    if (b.spawnEnemy) {
                        MinionBoss1 m = b.SpawnMinion(MathUtils.GetDirectionFromAngle(rnd.Next(20, 90)) * 500f, true);
                        Instantiate(m);
                        minions.Add(m);
                        b.spawnEnemy = false;
                    }
                    if (b.attack) {
                        List<Boss1Spike> spikes = b.Attack();
                        foreach(Boss1Spike s in spikes) {
                            Instantiate(s);
                            bullets.Add(s);
                        }
                        b.attack = false;
                    }
                }
            }

            if (bossDefeated && !explode) {
                explode = true;
                for (int i = 0; i < 10; i++) {
                    MinionBoss1 m = (boss as Boss1).SpawnMinion(MathUtils.GetDirectionFromAngle(rnd.Next(50, 130)) * 400f, false);
                    Instantiate(m);
                    minions.Add(m);
                }
            }

            foreach(Enemy minion in minions) {
                if (minion is MinionBoss1 m) { 
                    if (m.spiked) {
                        if (m.attack) {
                            Boss1Spike s = m.Shoot();
                            Instantiate(s);
                            bullets.Add(s);
                            m.attack = false;
                        }
                    }
                }
            }
        }
    }
}
