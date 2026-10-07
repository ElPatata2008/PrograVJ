using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.MP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Mariano.Scenes
{
    public class GameOver : Scene
    {
        Stopwatch sw = new Stopwatch();
        public GameOver(Camera c) : base(c)
        {
        }

        public override void Init()
        {
            //MoonPatrol.scores.Sort((a, b) => b.Item1.CompareTo(a.Item1));
            AudioManager.StopMusic();
            sw.Start();

        }

        public override void PaintScreen(Graphics g)
        {
            DrawingUtils.DrawCenterText(g, "Game Over", FontManager.Get("byte", 48), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 25));
            DrawingUtils.DrawCenterText(g, $"Score: {MarianoHermanos.lastScore}", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 25));
        }

        public override void ProcessInput() { }

        public override void Update(float dt)
        {
            if (sw.ElapsedMilliseconds / 1000 > 4)
            {
                sw.Reset();
                SceneManager.SetActive("menu");
            }
        }
    }
}
