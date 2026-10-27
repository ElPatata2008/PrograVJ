using PrograVJ.Engine.Manager;
using PrograVJ.Games.TerrariaBattleSimulator;


//using PrograVJ.Games.PunchOut;

//using PrograVJ.Games.Frogger;
//using PrograVJ.Games.Mariano;


//using PrograVJ.Games.Arkanoid;
//using PrograVJ.Games.Asteroids;
//using PrograVJ.Games.MP;
using PrograVJ.Games.Test;

//using PrograVJ.Games.SimonSays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ
{
    internal class Program
    {
        public static Vector2 resolution = new Vector2(800, 600);
        static void Main(string[] args)
        {
            RegisterInputs();
            RegisterFonts();

            // Laboratorios
            //Game pong = new Pong(800, 600, 60);
            //Game arkanoid = new Arkanoid(800, 600, 60);
            //Game asteroid = new Asteroids((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); asteroid.StartGame();
            //Game simonSays = new SimonSays((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); simonSays.StartGame();
            //Game frogger = new Frogger((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); frogger.StartGame();
            //Game mariano = new MarianoHermanos((int)resolution.X, (int)resolution.Y, 144, Engine.CameraType.Orthographic); mariano.StartGame();
            //Game punchout = new PunchOut((int)resolution.X, (int)resolution.Y, 144, Engine.CameraType.Orthographic); punchout.StartGame();

            // Proyectos
            //Game mp = new MoonPatrol((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); mp.StartGame();
            Game tbs = new TBS((int)resolution.X, (int)resolution.Y, 144, Engine.CameraType.Orthographic); tbs.StartGame();

            //Game test = new TestGame((int)resolution.X, (int)resolution.Y, 120, Engine.CameraType.Orthographic); test.StartGame();
            Application.Run();
        }

        private static void RegisterFonts()
        {
            FontManager.Load("ByteBounce.ttf", "byte");
            FontManager.Load("andyBold.ttf", "andyBold");
        }

        private static void RegisterInputs()
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
        }
    }
}
