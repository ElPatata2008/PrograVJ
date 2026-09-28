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
            InputManager.Register("up", new List<Keys>() { Keys.Up, });
            InputManager.Register("down", new List<Keys>() { Keys.Down, });
            InputManager.Register("left", new List<Keys>() { Keys.Left, });
            InputManager.Register("right", new List<Keys>() { Keys.Right, });
            InputManager.Register("w", new List<Keys>() { Keys.W });
            InputManager.Register("s", new List<Keys>() { Keys.S });
            InputManager.Register("a", new List<Keys>() { Keys.A });
            InputManager.Register("d", new List<Keys>() { Keys.D });

            InputManager.Register("np8", new List<Keys>() { Keys.NumPad8 });
            InputManager.Register("np2", new List<Keys>() { Keys.NumPad2 });
            InputManager.Register("np4", new List<Keys>() { Keys.NumPad4 });
            InputManager.Register("np6", new List<Keys>() { Keys.NumPad6 });

            InputManager.Register("space", new List<Keys>() { Keys.Space });
            InputManager.Register("enter", new List<Keys>() { Keys.Enter });
            InputManager.Register("escape", new List<Keys>() { Keys.Escape });
            InputManager.Register("z", new List<Keys>() { Keys.Z });
            InputManager.Register("x", new List<Keys>() { Keys.X });

            SceneManager.Register(new TestMain(c), "main");

            SceneManager.SetActive("main");
        }
    }
}
