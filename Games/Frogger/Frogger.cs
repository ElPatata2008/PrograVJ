using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.Frogger
{
    public class Frogger : Game
    {
        public static int tileSize = 50;
        public static bool victory = false;
        public Frogger(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            FontManager.Load("ByteBounce.ttf", "byte");

            InputManager.Register("up", new List<Keys>() { Keys.W, Keys.Up} );
            InputManager.Register("down", new List<Keys>() { Keys.S, Keys.Down } );
            InputManager.Register("left", new List<Keys>() { Keys.A, Keys.Left } );
            InputManager.Register("right", new List<Keys>() { Keys.D, Keys.Right } );
            InputManager.Register("enter", new List<Keys>() { Keys.Enter } );

            SceneManager.Register(new FroggerMenu(c), "menu");
            SceneManager.Register(new FroggerMainGame(c), "game");
            SceneManager.Register(new FroggerControls(c), "controls");
            SceneManager.Register(new FroggerGameOver(c), "gameover");

            SceneManager.SetActive("game");

        }
    }
}
