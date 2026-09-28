using NAudio.Wave;
using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.MP.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.MP
{
    public class MoonPatrol : Game
    {

        public static List<Tuple<int, string>> scores = new List<Tuple<int, string>>();

        public MoonPatrol(int w, int h, float fps, CameraType type) : base(w, h, fps, type)
        {
            scores.Add(new Tuple<int, string>(800, "Buddy"));
            scores.Add(new Tuple<int, string>(400, "Pal"));
            scores.Add(new Tuple<int, string>(1000, "Companion"));
            scores.Add(new Tuple<int, string>(600, "Guy"));
            scores.Add(new Tuple<int, string>(200, "Onion"));
            scores.Add(new Tuple<int, string>(4600, "Developer"));


            FontManager.Load("ByteBounce.ttf", "byte");

            InputManager.Register("up", new List<Keys>() { Keys.Up, Keys.W });
            InputManager.Register("down", new List<Keys>() { Keys.Down, Keys.S });
            InputManager.Register("left", new List<Keys>() { Keys.Left, Keys.A });
            InputManager.Register("right", new List<Keys>() { Keys.Right, Keys.D });
            InputManager.Register("space", new List<Keys>() { Keys.Space });
            InputManager.Register("enter", new List<Keys>() { Keys.Enter });
            InputManager.Register("escape", new List<Keys>() { Keys.Escape });
            InputManager.Register("z", new List<Keys>() { Keys.Z });
            InputManager.Register("x", new List<Keys>() { Keys.X });

            AudioManager.LoadSFX("startGame.mp3", "start");
            //AudioManager.LoadSFX("explosion.mp3", "explosion");
            AudioManager.LoadSFX("explosionLoud.mp3", "explosionLoud");
            AudioManager.LoadSFX("hit1.mp3", "hit1");
            AudioManager.LoadSFX("hit2.mp3", "hit2");
            AudioManager.LoadSFX("saucerHit.mp3", "saucerHit");
            AudioManager.LoadSFX("saucerExplosion.mp3", "saucerExplosion");
            AudioManager.LoadSFX("cartHit.mp3", "cartHit");
            AudioManager.LoadSFX("cartExplosion.mp3", "cartExplosion");
            AudioManager.LoadSFX("plateExplosion.mp3", "plateExplosion");
            AudioManager.LoadSFX("shoot.mp3", "shoot");
            AudioManager.LoadSFX("menuButton.mp3", "menuButton");
            AudioManager.LoadSFX("buttonSelect.mp3", "buttonSelect");
            AudioManager.InitMixer();

            AudioManager.LoadMusic("menu.mp3", "menu");
            AudioManager.LoadMusic("leaderboard.mp3", "leaderboard");

            TextureManager.Load("player.png", "player");
            TextureManager.Load("playerHit.png", "playerHit");
            TextureManager.Load("saucer.png", "saucer");
            TextureManager.Load("cart.png", "cart");
            TextureManager.Load("plate.png", "plate");
            TextureManager.Load("ground.png", "ground");
            TextureManager.Load("bgFront.png", "bgFront");
            TextureManager.Load("bgMiddle.png", "bgMiddle");
            TextureManager.Load("bgBack.png", "bgBack");
            TextureManager.Load("logo.png", "logo");
            TextureManager.Load("moon.png", "moon");

            SceneManager.Register(new MPMainMenu(c), "menu");
            SceneManager.Register(new MPMainGame(c), "game");
            SceneManager.Register(new MPLeaderboards(c), "lb");

            //SceneManager.SetActive("game");
            SceneManager.SetActive("menu");
            //SceneManager.SetActive("lb");
        }

        public static void Terminate() => Environment.Exit(0);
        public static void DrawText(Graphics g, string text, Color color, Font font, PointF pos)
        {
            SizeF sizeStart = g.MeasureString(text, font);
            g.DrawString(text, font, new SolidBrush(color), (pos.X - sizeStart.Width) / 2, pos.Y);
        }

    }
}
