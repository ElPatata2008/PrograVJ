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
    public class LeaderboardMenu : Scene
    {
        public LeaderboardMenu(Camera c) : base(c)
        {
        }

        public override void Init()
        {
            SaveManager.Load("score.json");
            MarianoHermanos.lb.scores.Sort((a, b) => b.CompareTo(a));
        }

        public override void PaintScreen(Graphics g)
        {
            if (MarianoHermanos.lb.scores.Count > 0)
            {
                for (int i = 0; i < Math.Min(MarianoHermanos.lb.scores.Count, 10); i++)
                {
                    DrawingUtils.DrawCenterText(g, $"{MarianoHermanos.lb.scores[i]}", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, 50 * i + 20));
                }  
            }
            DrawingUtils.DrawCenterText(g, "[1] Back", FontManager.Get("byte", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y - 50));
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("1")) SceneManager.SetActive("menu");
        }

        public override void Update(float dt)
        {
            
        }
    }
}
