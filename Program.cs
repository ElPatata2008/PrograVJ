using PrograVJ.Games;
using PrograVJ.Games.Frogger;
using PrograVJ.Games.Mariano;


//using PrograVJ.Games.Arkanoid;
//using PrograVJ.Games.Asteroids;
using PrograVJ.Games.MP;
using PrograVJ.Games.Test;

//using PrograVJ.Games.SimonSays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            // Laboratorios
            //Game pong = new Pong(800, 600, 60);
            //Game arkanoid = new Arkanoid(800, 600, 60);
            //Game asteroid = new Asteroids((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); asteroid.StartGame();
            //Game simonSays = new SimonSays((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); simonSays.StartGame();
            //Game frogger = new Frogger((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); frogger.StartGame();
            Game mariano = new MarianoHermanos((int)resolution.X, (int)resolution.Y, 120, Engine.CameraType.Orthographic); mariano.StartGame();

            // Proyectos
            //Game mp = new MoonPatrol((int)resolution.X, (int)resolution.Y, 60, Engine.CameraType.Orthographic); mp.StartGame();

            //Game test = new TestGame((int)resolution.X, (int)resolution.Y, 120, Engine.CameraType.Orthographic); test.StartGame();
            Application.Run();
        }
    }
}
