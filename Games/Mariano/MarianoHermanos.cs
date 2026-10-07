using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.Mariano.Objects;
using PrograVJ.Games.Mariano.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.Mariano
{
    public class MarianoHermanos : Game
    {
        public static Leaderboard lb = new Leaderboard();
        public static int lastScore = 0;
        public MarianoHermanos(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            FontManager.Load("ByteBounce.ttf", "byte");

            InputManager.Register("up", new List<Keys>() { Keys.Up, });
            InputManager.Register("down", new List<Keys>() { Keys.Down, });
            InputManager.Register("left", new List<Keys>() { Keys.Left, });
            InputManager.Register("right", new List<Keys>() { Keys.Right, });
            InputManager.Register("1", new List<Keys>() { Keys.D1, Keys.NumPad1 });
            InputManager.Register("2", new List<Keys>() { Keys.D2, Keys.NumPad2 });
            InputManager.Register("3", new List<Keys>() { Keys.D3, Keys.NumPad3 });

            SceneManager.Register(new Scenes.MainMenu(c), "menu");
            SceneManager.Register(new MainGame(c), "game");
            SceneManager.Register(new LeaderboardMenu(c), "leaderboard");
            SceneManager.Register(new GameOver(c), "gameover");

            SceneManager.SetActive("menu");
        }
    }
}
