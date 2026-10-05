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
    public class FroggerControls : Scene
    {
        bool starting = false;
        Stopwatch sw = new Stopwatch();
        Square controls;
        Square car;
        Square truck;
        Square log;
        public FroggerControls(Camera c) : base(c)
        {
            
        }

        public override void Init()
        {
            starting = false;
            controls = new Square(
                new Vector3(-200, 100, 0),
                Vector3.Zero,
                new Vector3(200, 150, 0),
                Color.White, Color.White, fillTexture: TextureManager.Get("wasd")
            );

            car = new Square(
                new Vector3(100, 200, 0),
                Vector3.Zero,
                new Vector3(75, 75, 0),
                Color.White, Color.White, fillTexture: TextureManager.Get("car1")
            );
            truck = new Square(
                new Vector3(300, 200, 0),
                Vector3.Zero,
                new Vector3(150, 75, 0),
                Color.White, Color.White, fillTexture: TextureManager.Get("truck")
            );
            log = new Square(
                new Vector3(200, 0, 0),
                Vector3.Zero,
                new Vector3(225, 75, 0),
                Color.White, Color.White, fillTexture: TextureManager.Get("log")
            );
        }

        public override void PaintScreen(Graphics g)
        {
            controls.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Move around\nwith WASD keys", FontManager.Get("uph", 24), Color.White, new PointF(sceneCamera.size.X / 2 - 200, sceneCamera.size.Y / 2));

            car.Draw(g, sceneCamera);
            truck.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Cars and Trucks\nare dangerous!", FontManager.Get("uph", 24), Color.White, new PointF(sceneCamera.size.X / 2 + 200, sceneCamera.size.Y / 2 - 150));

            log.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Logs will be handy", FontManager.Get("uph", 24), Color.White, new PointF(sceneCamera.size.X / 2 + 200, sceneCamera.size.Y / 2 + 50));

            DrawingUtils.DrawCenterText(g, "Reach the top before times runs out!", FontManager.Get("uph", 24), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 175));

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
                SceneManager.SetActive("game");
                sw.Reset();
            }
        }
    }
}
