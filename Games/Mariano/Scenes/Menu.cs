using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Scenes
{
    public class MainMenu : Scene
    {
        public MainMenu(Camera c) : base(c)
        {
        }

        public override void Init()
        {
            
        }

        public override void PaintScreen(Graphics g)
        {
            g.DrawString("[1] Start Game", FontManager.Get("byte", 36), Brushes.White, new PointF(25, sceneCamera.size.Y / 2));
            g.DrawString("[2] Leaderboard", FontManager.Get("byte", 36), Brushes.White, new PointF(25, sceneCamera.size.Y / 2 + 50));
            g.DrawString("[3] Exit", FontManager.Get("byte", 36), Brushes.White, new PointF(25, sceneCamera.size.Y / 2 + 100));
            DrawingUtils.DrawCenterText(g, "MARIANO vs TURTLES", FontManager.Get("byte", 50), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 4));
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("1")) SceneManager.SetActive("game");
            if (InputManager.JustPressedInput("2")) SceneManager.SetActive("leaderboard");
            if (InputManager.JustPressedInput("3")) Environment.Exit(0);
        }

        public override void Update(float dt)
        {
            
        }
    }
}
