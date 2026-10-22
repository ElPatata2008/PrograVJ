using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.Cuphead.Objects;
using PrograVJ.Games.Cuphead.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Cuphead
{
    public class TerrariaBattleSimulator : Game
    {
        public static Configuration config = new Configuration();
        public TerrariaBattleSimulator(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            SaveManager.Load("config.json");


            SceneManager.Register(new Boss1Battle(c), "game");

            SceneManager.SetActive("game");
        }
    }
}
