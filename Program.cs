using PrograVJ.Games;
using PrograVJ.Games.Arkanoid;
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
            //Game pong = new Pong(800, 600, 60);
            //Game arkanoid = new Arkanoid(800, 600, 60);
            Game test = new Test((int)resolution.X, (int)resolution.Y, 60);

            test.StartGame();

            Application.Run();
        }
    }
}
