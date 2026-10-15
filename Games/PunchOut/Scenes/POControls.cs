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

namespace PrograVJ.Games.PunchOut.Scenes
{
    public class POControls : Scene
    {
        public POControls(Camera c) : base(c)
        {
        }

        public override void Init()
        {

        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.Black);
            Square controls = new Square(
                Vector3.Zero,
                Vector3.Zero,
                sceneCamera.size,
                Color.White, Color.White, fillTexture: TextureManager.Get("controls")
            );
            controls.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Light Attack", FontManager.Get("po", 12), Color.White, new PointF(120, sceneCamera.size.Y / 2 - 50));
            DrawingUtils.DrawCenterText(g, "[Z]", FontManager.Get("po", 12), Color.White, new PointF(120, sceneCamera.size.Y / 2 - 20));

            DrawingUtils.DrawCenterText(g, "Block", FontManager.Get("po", 12), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 50));
            DrawingUtils.DrawCenterText(g, "[DOWN]", FontManager.Get("po", 12), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 20));

            DrawingUtils.DrawCenterText(g, "Uppercut", FontManager.Get("po", 12), Color.White, new PointF(sceneCamera.size.X - 150, sceneCamera.size.Y / 2 - 50));
            DrawingUtils.DrawCenterText(g, "[X]", FontManager.Get("po", 12), Color.White, new PointF(sceneCamera.size.X - 150, sceneCamera.size.Y / 2 - 20));

            DrawingUtils.DrawCenterText(g, "Dodge", FontManager.Get("po", 12), Color.White, new PointF(120, sceneCamera.size.Y - 120));
            DrawingUtils.DrawCenterText(g, "[LEFT] / [RIGHT]", FontManager.Get("po", 12), Color.White, new PointF(120, sceneCamera.size.Y - 100));

            DrawingUtils.DrawCenterText(g, "Press [ENTER] to continue.", FontManager.Get("po", 12), Color.White, new PointF(sceneCamera.size.X - 250, sceneCamera.size.Y - 120));
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("enter")) SceneManager.SetActive("intro");
        }

        public override void Update(float dt)
        {

        }
    }
}
