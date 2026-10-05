using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Scenes
{
    public class FroggerMenu : Scene
    {
        Square bg;
        bool starting = false;
        Stopwatch sw = new Stopwatch();
        public FroggerMenu(Camera c) : base(c)
        {
            
        }

        public override void Init()
        {
            Frogger.Frogger.victory = false;
            starting = false;
            bg = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, sceneCamera.size.Y, 0),
                Color.White, Color.White, fillTexture: TextureManager.Get("menu")
            );
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Black);
            bg.Draw(g, sceneCamera);
            Rectangle box = new Rectangle(0, (int)sceneCamera.size.Y - 50, (int)sceneCamera.size.X, 50);
            g.FillRectangle(Brushes.Black, box);
            g.DrawRectangle(Pens.White, box);
            DrawingUtils.DrawCenterText(g, "Press Enter to Start", FontManager.Get("uph", 24), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y - 50));
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
                SceneManager.SetActive("controls");
                sw.Reset();
            }
        }
    }
}
