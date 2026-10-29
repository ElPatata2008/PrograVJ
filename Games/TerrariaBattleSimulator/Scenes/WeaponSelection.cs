using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects.Buttons;
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
    public class WeaponSelection : Scene
    {
        Square cur;
        WpnButton undertaker;
        WpnButton musket;
        WpnButton minishark;
        WpnButton vortex;

        DifButton classic;
        DifButton expert;
        DifButton master;

        UIButton Continue;
        UIButton goBack;

        int weaponfontSize = 16;

        Parallax bg;
        Parallax trees;
        Parallax bg2;
        Parallax trees2;

        Square sky;

        public WeaponSelection(Camera c) : base(c)
        {
            Instantiate(cur = new Square(
                new Vector3(InputManager.MouseLocation.X, InputManager.MouseLocation.Y, 0),
                Vector3.Zero,
                Vector3.One,
                Color.Black, Color.White,
                new PointCollider(cur)
            ));

            Instantiate(undertaker = new WpnButton(
                new Vector3(-150, 150, 0),
                Vector3.Zero,
                new Vector3(100, 50, 0),
                Color.Black, Color.White,
                TextureManager.Get("undertaker"), 0,
                new BoxCollider2D(undertaker)
            ));

            Instantiate(musket = new WpnButton(
                new Vector3(0, 150, 0),
                Vector3.Zero,
                new Vector3(100, 50, 0),
                Color.Black, Color.White,
                TextureManager.Get("musket"), 1,
                new BoxCollider2D(musket)
            ));

            Instantiate(minishark = new WpnButton(
                new Vector3(150, 150, 0),
                Vector3.Zero,
                new Vector3(100, 50, 0),
                Color.Black, Color.White,
                TextureManager.Get("minishark"), 2,
                new BoxCollider2D(minishark)
            ));

            Instantiate(classic = new DifButton(
                new Vector3(-150, -100, 0),
                Vector3.Zero,
                Vector3.One * 125,
                Color.Black, Color.White, 
                TextureManager.Get("classic"), 0,
                new BoxCollider2D(classic)
            ));

            Instantiate(expert = new DifButton(
                new Vector3(0, -100, 0),
                Vector3.Zero,
                Vector3.One * 125,
                Color.Black, Color.White,
                TextureManager.Get("expert"), 1,
                new BoxCollider2D(expert)
            ));

            Instantiate(master = new DifButton(
                new Vector3(150, -100, 0),
                Vector3.Zero,
                Vector3.One * 125,
                Color.Black, Color.White,
                TextureManager.Get("master"), 2,
                new BoxCollider2D(master)
            ));

            Instantiate(Continue = new UIButton(
                new Vector3(sceneCamera.size.X / 2 - 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.LightGray,
                () =>
                {
                    SceneManager.SetActive("accessorySelection");
                    SaveManager.Save("config.json");
                    AudioManager.PlaySFX("continue");
                },
                new BoxCollider2D(Continue)
            ));

            Instantiate(goBack = new UIButton(
                new Vector3(-sceneCamera.size.X / 2 + 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.LightGray,
                () => {
                    SceneManager.SetActive("titleScreen");
                    SaveManager.Save("config.json");
                    AudioManager.PlaySFX("back");
                },
                new BoxCollider2D(goBack)
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
            if (TBS.config.boss1Defeated && TBS.config.boss2Defeated)
            {
                Instantiate(vortex = new WpnButton(
                    new Vector3(300, 150, 0),
                    Vector3.Zero,
                    new Vector3(100, 50, 0),
                    Color.Black, Color.White,
                    TextureManager.Get("vortex"), 3,
                    new BoxCollider2D(vortex)
                ));
            }
            Continue.delayTimer = 0;
            goBack.delayTimer = 0;
            undertaker.delayTimer = 0;
            musket.delayTimer = 0;
            minishark.delayTimer = 0;
            if (vortex != null) vortex.delayTimer = 0;
            classic.delayTimer = 0;
            expert.delayTimer = 0;
            master.delayTimer = 0;
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
            DrawingUtils.DrawCenterText(g, "Weapons", FontManager.Get("andyBold", 36), Color.White, new PointF(sceneCamera.size.X / 2, 50));
            undertaker.Draw(g,sceneCamera);
            DrawingUtils.DrawCenterText(g, "Damage : 26", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 - 150, sceneCamera.size.Y / 2 - 120));
            DrawingUtils.DrawCenterText(g, "Crit % : 4%", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 - 150, sceneCamera.size.Y / 2 - 100));
            DrawingUtils.DrawCenterText(g, "Fire Rate : 0.33s", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 - 150, sceneCamera.size.Y / 2 - 80));
            musket.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Damage : 38", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 120));
            DrawingUtils.DrawCenterText(g, "Crit % : 12%", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 100));
            DrawingUtils.DrawCenterText(g, "Fire Rate : 0.53s", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 80));
            minishark.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Damage : 13", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 + 150, sceneCamera.size.Y / 2 - 120));
            DrawingUtils.DrawCenterText(g, "Crit % : 4%", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 + 150, sceneCamera.size.Y / 2 - 100));
            DrawingUtils.DrawCenterText(g, "Fire Rate : 0.13s", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 + 150, sceneCamera.size.Y / 2 - 80));
            if (vortex != null) { 
                vortex.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Damage : 57", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 + 300, sceneCamera.size.Y / 2 - 120));
                DrawingUtils.DrawCenterText(g, "Crit % : 4%", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 + 300, sceneCamera.size.Y / 2 - 100));
                DrawingUtils.DrawCenterText(g, "Fire Rate : 0.083s", FontManager.Get("andyBold", weaponfontSize), Color.White, new PointF(sceneCamera.size.X / 2 + 300, sceneCamera.size.Y / 2 - 80));
            }

            DrawingUtils.DrawCenterText(g, "Difficulty", FontManager.Get("andyBold", 36), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 - 25));
            classic.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Classic", FontManager.Get("andyBold", 20), Color.White, new PointF(sceneCamera.size.X / 2 - 150, sceneCamera.size.Y / 2 + 165));
            expert.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Expert", FontManager.Get("andyBold", 20), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 165));
            master.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Master", FontManager.Get("andyBold", 20), Color.White, new PointF(sceneCamera.size.X / 2 + 150, sceneCamera.size.Y / 2 + 165));

            Continue.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Continue", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X - 150, sceneCamera.size.Y - 65));
            goBack.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Back", FontManager.Get("andyBold", 26), Color.Black, new PointF(150, sceneCamera.size.Y - 65));
        }

        public override void ProcessInput()
        {
            
        }

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
