using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Scenes
{
    public class MainMenu : Scene
    {
        Square bg;
        public MainMenu(Camera c) : base(c)
        {
            bg = new Square(
                Vector3.Zero,
                Vector3.Zero,
                sceneCamera.size,
                Color.White, Color.White,
                fillTexture: TextureManager.Get("mainmenu")
            );
        }

        public override void Init()
        {
            
        }

        public override void PaintScreen(Graphics g)
        {
            bg.Draw(g, sceneCamera);
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), new RectangleF(150, 100, sceneCamera.size.X - 300, 400));
            DrawingUtils.DrawCenterText(g, "[1] Start Game", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
            DrawingUtils.DrawCenterText(g, "[2] Leaderboard", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 50));
            DrawingUtils.DrawCenterText(g, "[3] Exit", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 100));
            DrawingUtils.DrawCenterText(g, "MARIANO GAMING", FontManager.Get("byte", 50), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 4));
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
