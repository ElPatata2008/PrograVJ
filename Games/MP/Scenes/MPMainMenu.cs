using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.MP.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace PrograVJ.Games.MP.Scenes
{
    public class MPMainMenu : Scene
    {
        int menuIndex = 0;

        Parallax ground;
        Parallax bgFront;
        Parallax bgMiddle;
        Parallax bgBack;
        Square Logo;
        Cube cube;
        Square Fade;
        bool inTransition = false;
        bool fadeIn = false;
        bool fadeOut = false;

        public MPMainMenu(Camera c) : base(c)
        {
            bgBack = new Parallax(TextureManager.Get("bgBack"), c.position, c.size, -c.size.X);
            bgMiddle = new Parallax(TextureManager.Get("bgMiddle"), new Vector3(c.position.X, c.position.Y - 25, c.position.Z), c.size, -c.size.X);
            bgFront = new Parallax(TextureManager.Get("bgFront"), new Vector3(c.position.X, c.position.Y - 75, c.position.Z), c.size, -c.size.X);
            ground = new Parallax(TextureManager.Get("ground"), new Vector3(c.position.X, c.position.Y - 180, c.position.Z), new Vector3(c.size.X, 200, c.size.Z), -c.size.X);
            Logo = new Square(
                new Vector3(c.position.X, c.position.Y + 150, 0),
                Vector3.Zero,
                new Vector3(600, 150, 0),
                Color.White, Color.White, fillTexture: TextureManager.Get("logo")
            );

            cube = new Cube(
                new Vector3(0, 100, 0),
                Vector3.Zero,
                new Vector3(100, 100, 100),
                Color.Black, Color.White
            );
            Fade = new Square(
                Vector3.Zero,
                Vector3.Zero,
                new Vector3(sceneCamera.size.X, sceneCamera.size.Y + 10, 200),
                Color.Black, Color.Black
            );

        }

        public override void Init()
        {
            //AudioManager.StopLayeredMusic(1000);
            AudioManager.PlayMusic("menu", true, 1000);
            
            Fade.size = new Vector3(sceneCamera.size.X, sceneCamera.size.Y + 10, 200);
            Fade.rotation = new Vector3(0, 0, 1);
            inTransition = true;
            fadeOut = false;
            fadeIn = true;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            
            bgBack.RenderParallax(g, sceneCamera);
            bgMiddle.RenderParallax(g, sceneCamera);
            bgFront.RenderParallax(g, sceneCamera);
            ground.RenderParallax(g, sceneCamera);

            Logo.Draw(g, sceneCamera);


            //if (MoonPatrol.scores.Count > 0) MoonPatrol.DrawText(g, $"Highest Score: {MoonPatrol.scores.Max()}", Color.Black, FontManager.Get("byte", 48), new PointF(sceneCamera.size.X, 10));
            g.FillRectangle(new SolidBrush(menuIndex == 0 ? Color.LightBlue : Color.Gray), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y / 2, 350, 50));
            g.DrawRectangle(new Pen(Color.Black, 3), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y / 2, 350, 50));
            MoonPatrol.DrawText(g, "Start", Color.Black, FontManager.Get("byte", 48), new PointF(sceneCamera.size.X, sceneCamera.size.Y / 2));

            g.FillRectangle(new SolidBrush(menuIndex == 1 ? Color.LightBlue : Color.Gray), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y / 2 + 60, 350, 50));
            g.DrawRectangle(new Pen(Color.Black, 3), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y / 2 + 60, 350, 50));
            MoonPatrol.DrawText(g, "Leaderboard", Color.Black, FontManager.Get("byte", 48), new PointF(sceneCamera.size.X, sceneCamera.size.Y / 2 + 60));

            g.FillRectangle(new SolidBrush(menuIndex == 2 ? Color.LightBlue : Color.Gray), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y / 2 + 120, 350, 50));
            g.DrawRectangle(new Pen(Color.Black, 3), new Rectangle((int)sceneCamera.size.X / 2 - 175, (int)sceneCamera.size.Y / 2 + 120, 350, 50));
            MoonPatrol.DrawText(g, "Exit", Color.Black, FontManager.Get("byte", 48), new PointF(sceneCamera.size.X, sceneCamera.size.Y / 2 + 120));

            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, (int)sceneCamera.size.Y - 30, (int)sceneCamera.size.X, 30));
            g.DrawRectangle(new Pen(Color.White, 4), new Rectangle(0, (int)sceneCamera.size.Y - 30, (int)sceneCamera.size.X, 30));
            MoonPatrol.DrawText(g, "Move in Menu: [UP]/[W] & [DOWN]/[S] | Select: [ENTER]", Color.White, FontManager.Get("byte", 24), new PointF(sceneCamera.size.X, sceneCamera.size.Y - 28));


            if (inTransition) Fade.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("down") && menuIndex < 2) { menuIndex++; AudioManager.PlaySFX("buttonSelect"); }
            if (InputManager.JustPressedInput("up") && menuIndex > 0) { menuIndex--; AudioManager.PlaySFX("buttonSelect"); }    
            if (InputManager.JustPressedInput("enter") && !inTransition) FadeOut(); 
        }


        public override void Update()
        {
            if (!inTransition) MenuLogic();
            else TransitionScreen();
        }

        private void MenuLogic()
        {
            bgBack.UpdateParallax(0.2f);
            bgMiddle.UpdateParallax(0.5f);
            bgFront.UpdateParallax(0.9f);
            ground.UpdateParallax(2.5f);
        }

        private void FadeOut()
        {
            inTransition = true; 
            fadeOut = true; 
            if (menuIndex == 0) AudioManager.PlaySFX("start");
            else AudioManager.PlaySFX("menuButton");
        }

        private void TransitionScreen()
        {
            if (fadeIn)
            {
                Fade.size.X /= 1.5f;
                //Console.WriteLine($"Fade X: {Fade.size.X}");
                if (Fade.size.X < 1) { inTransition = false; fadeIn = false; Fade.size.Y = Fade.size.X; }
            }
            if (fadeOut)
            {
                if (menuIndex == 0)
                {
                    Fade.size.X *= 1.15f;
                    Fade.size.Y *= 1.15f;
                    Fade.rotation.Z *= 1.125f;
                    //Console.WriteLine($"Fade X: {Fade.size.X}");
                    if (Fade.size.X >= sceneCamera.size.X * 2) { inTransition = false; SceneManager.SetActive("game"); }
                }
                else
                {
                    Fade.rotation.Z = 0f;
                    Fade.size.Y = sceneCamera.size.Y;
                    Fade.size.X *= menuIndex == 1 ? 1.5f : 1.2f;
                    if (Fade.size.X >= sceneCamera.size.X * 2) { 
                        if (menuIndex == 1) { inTransition = false; SceneManager.SetActive("lb"); }
                        if (menuIndex == 2) Environment.Exit(0);
                    }
                }
            }
        }
    }
}
