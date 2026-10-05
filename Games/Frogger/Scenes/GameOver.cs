using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Scenes
{
    public class FroggerGameOver : Scene
    {
        bool starting = false;
        Stopwatch sw = new Stopwatch();
        public FroggerGameOver(Camera c) : base(c)
        {
        }

        public override void Init()
        {
            starting = false;
        }

        public override void PaintScreen(Graphics g)
        {
            string victory = Frogger.Frogger.victory ? "Victory" : "Game Over";
            DrawingUtils.DrawCenterText(g, victory, FontManager.Get("uph", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
            DrawingUtils.DrawCenterText(g, "Press Enter to Continue", FontManager.Get("uph", 24), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 100));
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("enter") && !starting)
            {
                starting = true;
                AudioManager.PlaySFX("start");
                sw.Start();
            }
        }

        public override void Update(float dt)
        {
            if (sw.ElapsedMilliseconds / 1000 > 1.5f)
            {
                SceneManager.SetActive("menu");
                sw.Reset();
            }
        }
    }
}
