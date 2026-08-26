using PrograVJ.Games;
using PrograVJ.Games.Arkanoid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Game pong = new Pong(800, 600, 60);
            Game arkanoid = new Arkanoid(800, 600, 60);

            arkanoid.StartGame();

            Application.Run();
        }
    }
}
