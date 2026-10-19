using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.Test.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.Test
{
    public class TestGame : Game
    {
        public TestGame(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            
            //TextureManager.Load("red_balbing_roll.png", "rollRB");
            //TextureManager.Load("red_balbing_walk.png", "walkRB");
            //TextureManager.Load("red_balbing_dizzy.png", "dizzyRB");
            //TextureManager.Load("red_balbing_stand.png", "standRB");


            SceneManager.Register(new TestMain(c), "main");

            SceneManager.SetActive("main");
        }
    }
}
