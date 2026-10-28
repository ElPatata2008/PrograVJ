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
    public class TBS : Game
    {
        public static Configuration config = new Configuration();
        public static string bossName = string.Empty;
        public TBS(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            SaveManager.Load("config.json");
            AudioManager.InitMixer();
            FontManager.Load("andyBold.ttf", "andyBold");

            // Boss 1
            TextureManager.Load("boss1Jump.png", "boss1jump");
            TextureManager.Load("boss1idle.png", "boss1idle");
            TextureManager.Load("boss1attackCharge.png", "boss1attackCharge");
            TextureManager.Load("boss1attackRelease.png", "boss1attackRelease");

            // Boss 2
            TextureManager.Load("boss2phase1.png", "boss2phase1");
            TextureManager.Load("boss2phase2.png", "boss2phase2");
            TextureManager.Load("minionBoss2.png", "minionBoss2");

            // Accesories
            TextureManager.Load("Tabi.png", "tabi");
            TextureManager.Load("Cloud_in_a_Bottle.png", "ciab");
            TextureManager.Load("Anklet_of_the_Wind.png", "aotw");
            TextureManager.Load("Panic_Necklace.png", "panl");
            TextureManager.Load("Band_of_Regeneration.png", "breg");
            TextureManager.Load("Shackle.png", "shak");
            TextureManager.Load("Cross_Necklace.png", "cros");
            TextureManager.Load("Putrid_Scent.png", "scen");

            // Weapons
            TextureManager.Load("The_Undertaker.png", "undertaker");
            TextureManager.Load("Musket.png", "musket");
            TextureManager.Load("Minishark.png", "minishark");
            TextureManager.Load("Vortex_Beater.png", "vortex");

            // Difficulties
            TextureManager.Load("classic.png", "classic");
            TextureManager.Load("expert.png", "expert");
            TextureManager.Load("master.png", "master");

            // Others
            TextureManager.Load("title.png", "title");
            TextureManager.Load("panic.png", "panic");

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

            SceneManager.Register(new TitleScreen(c), "titleScreen");
            SceneManager.Register(new WeaponSelection(c), "weaponSelection");
            SceneManager.Register(new AccessorySelection(c), "accessorySelection");
            SceneManager.Register(new BattleSelection(c), "battleSelection");
            SceneManager.Register(new Boss1Battle(c), "boss1");
            SceneManager.Register(new Boss2Battle(c), "boss2");

            AudioManager.PlayMusic("title", true, 0);
            SceneManager.SetActive("battleSelection");
        }
    }
}
