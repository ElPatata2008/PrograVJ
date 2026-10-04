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
            FontManager.Load("upheavtt.ttf", "uph");
            AudioManager.InitMixer();

            InputManager.Register("up", new List<Keys>() { Keys.W, Keys.Up} );
            InputManager.Register("down", new List<Keys>() { Keys.S, Keys.Down } );
            InputManager.Register("left", new List<Keys>() { Keys.A, Keys.Left } );
            InputManager.Register("right", new List<Keys>() { Keys.D, Keys.Right } );
            InputManager.Register("enter", new List<Keys>() { Keys.Enter } );

            TextureManager.Load("car_1.png", "car1");
            TextureManager.Load("car_2.png", "car2");
            TextureManager.Load("car_3.png", "car3");
            TextureManager.Load("end_frog.png", "life");
            TextureManager.Load("flower_ground_2.png", "ground");
            TextureManager.Load("frog_death0002.png", "death");
            TextureManager.Load("frog0000.png", "frog");
            TextureManager.Load("truck.png", "truck");
            TextureManager.Load("log.png", "log");
            TextureManager.Load("menu.png", "menu");
            TextureManager.Load("wasd.png", "wasd");

            AudioManager.LoadSFX("continue.mp3", "start");
            AudioManager.LoadSFX("death.mp3", "death");
            AudioManager.LoadSFX("victory.mp3", "victory");
            AudioManager.LoadSFX("move.mp3", "move");

            AudioManager.LoadMusic("music.mp3", "music");

            SceneManager.Register(new FroggerMenu(c), "menu");
            SceneManager.Register(new FroggerMainGame(c), "game");
            SceneManager.Register(new FroggerControls(c), "controls");
            SceneManager.Register(new FroggerGameOver(c), "gameover");

            SceneManager.SetActive("menu");
            //SceneManager.SetActive("controls");

        }
    }
}
