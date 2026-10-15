using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.PunchOut.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.PunchOut
{
    public class PunchOut : Game
    {
        public static int endType = 0;
        public PunchOut(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            FontManager.Load("punch-out-nes.ttf", "po");
            AudioManager.InitMixer();

            InputManager.Register("up", new List<Keys>(){ Keys.Up });
            InputManager.Register("down", new List<Keys>(){ Keys.Down });
            InputManager.Register("left", new List<Keys>(){ Keys.Left });
            InputManager.Register("right", new List<Keys>(){ Keys.Right });
            InputManager.Register("z", new List<Keys>(){ Keys.Z });
            InputManager.Register("x", new List<Keys>(){ Keys.X });
            InputManager.Register("enter", new List<Keys>(){ Keys.Enter });

            AudioManager.LoadSFX("enemyDefeat.mp3", "enemyDefeat");
            AudioManager.LoadSFX("crowd.mp3", "crowd");
            AudioManager.LoadSFX("enemyHurt.mp3", "enemyHurt");
            AudioManager.LoadSFX("enemyDefeated.wav", "enemyDefeated");
            AudioManager.LoadSFX("playerHurt.wav", "playerHurt");
            AudioManager.LoadSFX("gameOver.mp3", "gameOver");
            AudioManager.LoadSFX("gameStart.mp3", "gameStart");
            AudioManager.LoadSFX("playerDefeat.mp3", "playerDefeat");
            AudioManager.LoadSFX("strongHit.mp3", "strongHit");
            AudioManager.LoadSFX("defeat.mp3", "defeat");
            AudioManager.LoadSFX("win.mp3", "win");
            AudioManager.LoadSFX("intro.mp3", "intro");

            AudioManager.LoadMusic("music.mp3", "music");

            TextureManager.Load("littleMacIntro.png", "lmIntro");
            TextureManager.Load("glassJoeIntro.png", "gjIntro");

            TextureManager.Load("star.png", "star");
            TextureManager.Load("controls.png", "controls");

            TextureManager.Load("glassJoeIdle.png", "gjIdle");
            TextureManager.Load("glassJoeAttackStart.png", "gjAttackStart");
            TextureManager.Load("glassJoeAttack.png", "gjAttack");
            TextureManager.Load("glassJoeAttackSpecial.png", "gjAttackSpecial");
            TextureManager.Load("glassJoeBlock.png", "gjBlock");
            TextureManager.Load("glassJoeHitLight.png", "gjHitLight");
            TextureManager.Load("glassJoeHitHard.png", "gjHitHard");
            TextureManager.Load("glassJoeDefeat.png", "gjDefeat");
            TextureManager.Load("glassJoeDefeated.png", "gjDefeated");

            TextureManager.Load("littleMacIdle.png", "lmIdle");
            TextureManager.Load("littleMacHit.png", "lmHurt");
            TextureManager.Load("littleMacDodgeLeft.png", "lmDL");
            TextureManager.Load("littleMacDodgeRight.png", "lmDR");
            TextureManager.Load("littleMacBlock.png", "lmBlock");
            TextureManager.Load("littleMacAttackLight.png", "lmAttackLight");
            TextureManager.Load("littleMacAttackHard.png", "lmAttackHard");
            TextureManager.Load("littleMacDefeat.png", "lmDefeat");
            TextureManager.Load("littleMacWin.png", "lmWin");

            TextureManager.Load("bg.png", "bg");

            SceneManager.Register(new POGame(c), "game");
            SceneManager.Register(new POIntro(c), "intro");
            SceneManager.Register(new POEndScreen(c), "end");
            SceneManager.Register(new POControls(c), "controls");

            SceneManager.SetActive("controls");


        }
    }
}
