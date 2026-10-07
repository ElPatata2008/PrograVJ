using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.MP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Scenes
{
    public class GameOver : Scene
    {
        public GameOver(Camera c) : base(c)
        {
        }

        public override void Init()
        {
            //MoonPatrol.scores.Sort((a, b) => b.Item1.CompareTo(a.Item1));
            MarianoHermanos.lb.scores.Sort((a, b) => b.CompareTo(a));
        }

        public override void PaintScreen(Graphics g)
        {
            DrawingUtils.DrawCenterText(g, "Game Over", FontManager.Get("byte", 48), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
            DrawingUtils.DrawCenterText(g, $"Score: {MarianoHermanos.lastScore}", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
            DrawingUtils.DrawCenterText(g, "[1] Exit", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y - 50));
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("1")) Environment.Exit(0);
        }

        public override void Update(float dt)
        {

        }
    }
}
