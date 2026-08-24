using PrograVJ.Games;
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
            Game game = new Pong(800, 600, 60);
            game.StartGame();

            Application.Run();
        }
    }
}
