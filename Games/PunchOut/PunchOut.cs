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
        public PunchOut(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            InputManager.Register("up", new List<Keys>(){ Keys.Up });
            InputManager.Register("down", new List<Keys>(){ Keys.Down });
            InputManager.Register("left", new List<Keys>(){ Keys.Left });
            InputManager.Register("right", new List<Keys>(){ Keys.Right });

            TextureManager.Load("glassJoeIdle.png", "gjIdle");
            TextureManager.Load("glassJoeAttack.png", "gjAttack");
            TextureManager.Load("glassJoeBlock.png", "gjBlock");

            TextureManager.Load("littleMacIdle.png", "lmIdle");
            TextureManager.Load("littleMacHit.png", "lmHurt");
            TextureManager.Load("littleMacDodgeLeft.png", "lmDL");
            TextureManager.Load("littleMacDodgeRight.png", "lmDR");
            TextureManager.Load("littleMacBlock.png", "lmBlock");

            TextureManager.Load("bg.png", "bg");

            SceneManager.Register(new POGame(c), "game");

            SceneManager.SetActive("game");


        }
    }
}
