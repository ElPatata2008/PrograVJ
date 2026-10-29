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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Games.TerrariaBattleSimulator.Scenes
{
    public class BattleSelection : Scene
    {
        Square cur;
        UIButton boss1;
        UIButton boss2;
        private Square boss1Defeated;
        private Square boss2Defeated;
        private UIButton goBack;
        public Square crown;

        Parallax bg;
        Parallax trees;
        Parallax bg2;
        Parallax trees2;

        Square sky;

        public BattleSelection(Camera c) : base(c)
        {
            Instantiate(cur = new Square(
                new Vector3(InputManager.MouseLocation.X, InputManager.MouseLocation.Y, 0),
                Vector3.Zero,
                Vector3.One,
                Color.Black, Color.White,
                new PointCollider(cur)
            ));

            Instantiate(boss1 = new UIButton(
                new Vector3(-200, 0, 0),
                Vector3.Zero,
                new Vector3(144, 96, 0),
                Color.Black, Color.LightGray,
                () => {
                    TBS.bossName = "King Slime";
                    BattleSelected("boss1"); 
                },
                new BoxCollider2D(boss1),
                animationController: new AnimationController()
            ));

            crown = new Square(
                boss1.position - new Vector3(0, boss1.size.Y / 2 - 85, 0),
                Vector3.Zero,
                new Vector3(75, 37.5f, 0),
                Color.Black, Color.Gold,
                fillTexture: TextureManager.Get("crown")
            );

            SpriteSheet b1 = new SpriteSheet(TextureManager.Get("boss1idle"), 48, 32, 0.15f, 5);
            boss1.animationController.AddAnimation("default", b1);
            boss1.animationController.Play("default");

            boss1Defeated = new Square(
                boss1.position + new Vector3(0, 100, 0),
                Vector3.Zero, Vector3.One * 32,
                Color.White, Color.White,
                fillTexture: TextureManager.Get("bossDefeated")
            );

            Instantiate(boss2 = new UIButton(
                new Vector3(0, 0, 0),
                new Vector3(0, 0, -90),
                Vector3.One * 96,
                Color.Black, Color.LightGray,
                () => {
                    TBS.bossName = "Giant Probe";
                    BattleSelected("boss2"); 
                },
                new BoxCollider2D(boss2),
                fillTexture: TextureManager.Get("boss2phase1")
            ));

            boss2Defeated = new Square(
                boss2.position + new Vector3(0, 100, 0),
                Vector3.Zero, Vector3.One * 32,
                Color.White, Color.White,
                fillTexture: TextureManager.Get("bossDefeated")
            );


            Instantiate(goBack = new UIButton(
                new Vector3(-sceneCamera.size.X / 2 + 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.LightGray,
                () => {
                    SceneManager.SetActive("accessorySelection");
                    SaveManager.Save("config.json");
                    AudioManager.PlaySFX("back");
                },
                new BoxCollider2D(goBack)
            ));

            bg = new Parallax(
                TextureManager.Get("groundVanilla"),
                new Vector3(0, -250, 0),
                new Vector3(400, 160, 0),
                -sceneCamera.size.X / 2
            );
            trees = new Parallax(
                TextureManager.Get("treesVanilla"),
                new Vector3(0, -50, 0),
                new Vector3(400, 240, 0),
                -sceneCamera.size.X / 2
            );

            bg2 = new Parallax(
                TextureManager.Get("groundVanilla"),
                new Vector3(0, -150, 0),
                new Vector3(600, 240, 0),
                -sceneCamera.size.X / 2
            );

            trees2 = new Parallax(
                TextureManager.Get("treesVanilla"),
                new Vector3(0, 50, 0),
                new Vector3(400, 240, 0),
                -sceneCamera.size.X / 2
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
            boss1.delayTimer = 0;
            boss2.delayTimer = 0;
            goBack.delayTimer = 0;
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
            DrawingUtils.DrawCenterText(g, "Choose your adversary", FontManager.Get("andyBold", 48), Color.White, new PointF(sceneCamera.size.X / 2, 75));
            if (TBS.config.boss1Defeated) boss1Defeated.Draw(g, sceneCamera);
            boss1.Draw(g, sceneCamera);
            crown.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "King Slime", FontManager.Get("andyBold", 26), Color.White,
                new PointF(sceneCamera.size.X / 2 - 200, sceneCamera.size.Y / 2 + 75));
            if (TBS.config.boss2Defeated) boss2Defeated.Draw(g, sceneCamera);
            boss2.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Giant Probe", FontManager.Get("andyBold", 26), Color.White,
                new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 75));
            goBack.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Back", FontManager.Get("andyBold", 26), Color.Black, new PointF(150, sceneCamera.size.Y - 65));
        }

        public override void ProcessInput()
        {
            
        }

        public override void Update(float dt)
        {
            boss1.animationController.Update(dt);
            Vector3 cursorPos = sceneCamera.GetCursorOnWorld();
            cur.position.X = cursorPos.X;
            cur.position.Y = cursorPos.Y;

            bg.UpdateParallax(100 * dt);
            trees.UpdateParallax(75 * dt);
            bg2.UpdateParallax(50 * dt);
            trees2.UpdateParallax(25 * dt);
        }

        private void BattleSelected(string scene)
        {
            SceneManager.SetActive(scene);
        }
    }
}
