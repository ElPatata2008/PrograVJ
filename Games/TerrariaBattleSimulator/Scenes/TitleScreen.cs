using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects.Scenery;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.TerrariaBattleSimulator.Scenes
{
    public class TitleScreen : Scene
    {
        private Square cur;
        Square title;

        Vector3 btnSize = new Vector3(200, 50, 0);

        UIButton start;
        UIButton instr;
        UIButton retur;
        UIButton exit;

        bool instructions = false;
        Parallax bg;
        Parallax trees;
        Parallax bg2;
        Parallax trees2;

        Square sky;

        public TitleScreen(Camera c) : base(c)
        {
            Instantiate(cur = new Square(
                new Vector3(InputManager.MouseLocation.X, InputManager.MouseLocation.Y, 0),
                Vector3.Zero,
                Vector3.One,
                Color.Black, Color.White,
                new PointCollider(cur)
            ));

            title = new Square(
                new Vector3(0, 200, 0),
                Vector3.Zero,
                new Vector3(486, 142, 0),
                Color.White, Color.White,
                fillTexture: TextureManager.Get("title")
            );

            Instantiate(start = new UIButton(
                new Vector3(0, -50, 0),
                Vector3.Zero, btnSize,
                Color.Black, Color.LightGray,
                () =>
                {
                    if (!instructions)
                    {
                        SceneManager.SetActive("weaponSelection");
                        AudioManager.PlaySFX("continue");
                    }
                },
                new BoxCollider2D(start)
            ));

            Instantiate(instr = new UIButton(
                new Vector3(0, -125, 0),
                Vector3.Zero, btnSize,
                Color.Black, Color.LightGray,
                () =>
                {
                    if (!instructions)
                    {
                        instructions = true;
                        AudioManager.PlaySFX("continue");
                    } 
                },
                new BoxCollider2D(instr)
            ));

            Instantiate(exit = new UIButton(
                new Vector3(0, -200, 0),
                Vector3.Zero, btnSize,
                Color.Black, Color.LightGray,
                () => { 
                    if (!instructions)
                    {
                        SaveManager.Save("config.json");
                        AudioManager.PlaySFX("back");
                        Environment.Exit(0);
                    }
                },
                new BoxCollider2D(exit)
            ));

            Instantiate(retur = new UIButton(
                new Vector3(-sceneCamera.size.X / 2 + 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero, btnSize,
                Color.Black, Color.LightGray,
                () =>
                {
                    if (instructions) { 
                        instructions = false;
                        AudioManager.PlaySFX("back");
                    }
                },
                new BoxCollider2D(retur)
            ));

            bg = new Parallax(
                TextureManager.Get("groundVanilla"),
                new Vector3(0, -250, 0), 
                new Vector3(400, 160, 0), 
                -sceneCamera.size.X
            );
            trees = new Parallax(
                TextureManager.Get("treesVanilla"),
                new Vector3(0, -50, 0),
                new Vector3(400, 240, 0),
                -sceneCamera.size.X
            );

            bg2 = new Parallax(
                TextureManager.Get("groundVanilla"),
                new Vector3(0, -150, 0),
                new Vector3(600, 240, 0),
                -sceneCamera.size.X
            );

            trees2 = new Parallax(
                TextureManager.Get("treesVanilla"),
                new Vector3(0, 50, 0),
                new Vector3(400, 240, 0),
                -sceneCamera.size.X
            );

            sky = new Square(
                Vector3.Zero, Vector3.Zero,
                sceneCamera.size,
                Color.White, Color.White,
                fillTexture: TextureManager.Get("skyDay")
            );

        }

        public override void Init()
        {
            start.delayTimer = 0;
            instr.delayTimer = 0;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            sky.Draw(g, sceneCamera);
            trees2.RenderParallax(g, sceneCamera);
            bg2.RenderParallax(g, sceneCamera);
            trees.RenderParallax(g, sceneCamera);
            bg.RenderParallax(g, sceneCamera);
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), new RectangleF(0, 0, sceneCamera.size.X, sceneCamera.size.Y));
            if (!instructions)
            {
                title.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "BATTLE SIMULATOR EDITION", FontManager.Get("andyBold", 36), Color.White, new PointF(sceneCamera.size.X / 2, 200));
                start.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Play", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 35));
                instr.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Instructions", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 110));
                exit.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Exit", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 185));
            }
            else { 
                retur.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Back", FontManager.Get("andyBold", 26), Color.Black, new PointF(150, sceneCamera.size.Y - 65));
            }
        }

        public override void ProcessInput() { }

        public override void Update(float dt)
        {
            Vector3 cursorPos = sceneCamera.GetCursorOnWorld();
            cur.position.X = cursorPos.X;
            cur.position.Y = cursorPos.Y;

            bg.UpdateParallax(100 * dt);
            trees.UpdateParallax(75 * dt);
            bg2.UpdateParallax(50 * dt);
            trees2.UpdateParallax(25 * dt);
        }
    }
}
