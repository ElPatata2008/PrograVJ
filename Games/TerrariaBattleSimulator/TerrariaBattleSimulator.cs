using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using PrograVJ.Games.TerrariaBattleSimulator.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator
{
    public class TerrariaBattleSimulator : Game
    {
        public static Configuration config = new Configuration();
        public TerrariaBattleSimulator(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            SaveManager.Load("config.json");
            AudioManager.InitMixer();

            TextureManager.Load("boss2phase1.png", "boss2phase1");
            TextureManager.Load("boss2phase2.png", "boss2phase2");
            TextureManager.Load("minionBoss2.png", "minionBoss2");

            AudioManager.LoadSFX("metalEnemyHit.wav", "meHit");
            AudioManager.LoadSFX("metalEnemyKill.wav", "meKill");
            AudioManager.LoadSFX("enemyHit.wav", "enemyHit");
            AudioManager.LoadSFX("enemyKill.wav", "enemyKill");
            AudioManager.LoadSFX("boss2Phase1Charge.wav", "roar1");
            AudioManager.LoadSFX("boss2Phase2Charge.wav", "roar2");
            AudioManager.LoadSFX("boss2Laser.wav", "laser");
            AudioManager.LoadSFX("gunShot.wav", "gunshot");
            AudioManager.LoadSFX("shotgunShot.wav", "shotgunshot");
            AudioManager.LoadSFX("vortex.wav", "vortex");

            AudioManager.LoadMusic("title.mp3", "title");
            AudioManager.LoadMusic("boss2.mp3", "boss2");
            AudioManager.LoadMusic("boss1.mp3", "boss1");

            SceneManager.Register(new Boss1Battle(c), "boss1");
            SceneManager.Register(new Boss2Battle(c), "boss2");

            SceneManager.SetActive("boss2");
        }
    }
}
