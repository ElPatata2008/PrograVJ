using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.MP.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.MP.Scenes
{
    public class MPLeaderboards : Scene
    {
        Square moon;
        Parallax bg;
        Square Fade;
        bool inTransition = false;
        bool fadeIn = false;
        bool fadeOut = false;
        public MPLeaderboards(Camera c) : base(c)
        {
            moon = new Square(
                new Vector3(c.position.X - 200, c.position.Y - 500, 0),
                Vector3.Zero,
                new Vector3(1000, 1000, 1000),
                Color.White, Color.White, fillTexture: TextureManager.Get("moon")
            );

            bg = new Parallax(TextureManager.Get("bgBack"), c.position, c.size, -c.size.X);

            Fade = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, sceneCamera.size.Y, 0),
                Color.Black, Color.Black
            );
        }

        public override void Init()
        {
            AudioManager.PlayMusic("leaderboard", true, 1000);
            MoonPatrol.scores.Sort((a, b) => b.Item1.CompareTo(a.Item1));
            Fade.size.Y = sceneCamera.size.Y;
            inTransition = true;
            fadeOut = false;
            fadeIn = true;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            bg.RenderParallax(g, sceneCamera);
            moon.Draw(g, sceneCamera);
            g.FillRectangle(new SolidBrush(Color.LightBlue), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y - 60, 350, 50));
            g.DrawRectangle(new Pen(Color.Black, 3), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y - 60, 350, 50));
            MoonPatrol.DrawText(g, "Back", Color.Black, FontManager.Get("byte", 48), new PointF(sceneCamera.size.X, sceneCamera.size.Y - 60));
            MoonPatrol.DrawText(g, "Leaderboard", Color.White, FontManager.Get("byte", 48), new PointF(sceneCamera.size.X, 15));
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), new Rectangle((int)sceneCamera.size.X / 2 - 300, (int)sceneCamera.size.Y / 2 - 225, 600, 425));
            g.DrawRectangle(new Pen(Brushes.Black, 5), new Rectangle((int)sceneCamera.size.X / 2 - 300, (int)sceneCamera.size.Y / 2 - 225, 600, 425));

            for (int i = 0; i < Math.Min(10, MoonPatrol.scores.Count); i++)
            {
                g.DrawString($"{i + 1} | {MoonPatrol.scores[i].Item1} | {MoonPatrol.scores[i].Item2}", FontManager.Get("byte", 48), Brushes.White, sceneCamera.size.X / 2 - 250, 80 + (i * 40));
            }
            

            if (inTransition) Fade.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("enter")) { inTransition = true; fadeOut = true; AudioManager.PlaySFX("menuButton"); }
        }

        public override void Update(float dt)
        {
            if (!inTransition)
            {
                bg.UpdateParallax(0.2f);
                moon.rotation.Z += 0.05f * dt;
                if (moon.rotation.Z > 360) moon.rotation.Z = 0;
            }
            else Transition(); 
        }

        private void Transition()
        {
            if (fadeIn)
            {
                Fade.size.Y /= 1.5f;
                //Console.WriteLine($"Fade X: {Fade.size.X}");
                if (Fade.size.Y < 1) { inTransition = false; fadeIn = false; }
            }
            if (fadeOut)
            {
                Fade.size.Y *= 1.5f;
                //Console.WriteLine($"Fade X: {Fade.size.X}");
                if (Fade.size.Y > sceneCamera.size.Y) { inTransition = false; SceneManager.SetActive("menu"); }
            }
        }
    }
}
