using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
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
                () => { if (!instructions) SceneManager.SetActive("weaponSelection"); },
                new BoxCollider2D(start)
            ));

            Instantiate(instr = new UIButton(
                new Vector3(0, -125, 0),
                Vector3.Zero, btnSize,
                Color.Black, Color.LightGray,
                () => { if (!instructions) instructions = true; },
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
                        Environment.Exit(0);
                    }
                },
                new BoxCollider2D(exit)
            ));

            Instantiate(retur = new UIButton(
                new Vector3(-sceneCamera.size.X / 2 + 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero, btnSize,
                Color.Black, Color.LightGray,
                () => { if (instructions) instructions = false; },
                new BoxCollider2D(retur)
            ));
        }

        public override void Init()
        {
            start.delayTimer = 0;
            instr.delayTimer = 0;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);


            if (!instructions)
            {
                title.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "BATTLE SIMULATOR EDITION", FontManager.Get("andyBold", 48), Color.Black, new PointF(sceneCamera.size.X / 2, 200));
                start.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Play", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 35));
                instr.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Instructions", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 110));
                exit.Draw(g, sceneCamera);
                DrawingUtils.DrawCenterText(g, "Exit", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 185));
            }
            else retur.Draw(g, sceneCamera);

            cur.Draw(g, sceneCamera);
        }

        public override void ProcessInput() { }

        public override void Update(float dt)
        {
            Vector3 cursorPos = sceneCamera.GetCursorOnWorld();
            cur.position.X = cursorPos.X;
            cur.position.Y = cursorPos.Y;
        }
    }
}
