using PrograVJ.Engine;
using PrograVJ.Engine.AI;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class PlayerBullet : Proyectile
    {
        Random rnd = new Random();

        public PlayerBullet(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, int dmg, float critChance, float speed, Collider collider = null, Rigidbody body = null, AIController controller = null, float borderWidth = 2, Bitmap fillTexture = null, WrapMode mode = WrapMode.Clamp, int repeatTileX = 1, int repeatTileY = 1, AnimationController animationController = null) : base(position, rotation, size, color, fillColor, dmg, critChance, speed, collider, body, controller, borderWidth, fillTexture, mode, repeatTileX, repeatTileY, animationController)
        {
        }

        public override void OnCollisionEnter(GameObject c)
        {
            if (c is Enemy e)
            {
                int damage = (rnd.NextDouble() < critChance) ? dmg * 2 : dmg;
                e.hp -= damage;
                landed = true;
                if (e.hp < 1)
                {
                    if (e is Boss2 || e is MinionBoss2) AudioManager.PlaySFX("meKill");
                    if (e is Boss1 || e is MinionBoss1) AudioManager.PlaySFX("enemyKill");
                } 
                else { 
                    if (e is Boss2 || e is MinionBoss2) AudioManager.PlaySFX("meHit");
                    if (e is MinionBoss1) AudioManager.PlaySFX("enemyHit");
                    if (e is Boss1 boss1)
                    {
                        AudioManager.PlaySFX("enemyHit");
                        if (rnd.NextDouble() < boss1.spawnChance * (1 + dmg / 100)) boss1.spawnEnemy = true;
                    }
                }
            }
            if (position.Y < -200) landed = true;
        }
    }
}
