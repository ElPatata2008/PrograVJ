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
using System.Windows.Forms;

namespace PrograVJ.Games.TerrariaBattleSimulator.Scenes
{
    public class BattleSelection : Scene
    {
        Square cur;
        UIButton boss1;
        UIButton boss2;
        private UIButton goBack;

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
                new Vector3(0, -50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.LightGray,
                () => { BattleSelected("boss1"); },
                new BoxCollider2D(boss1)
            ));

            Instantiate(boss2 = new UIButton(
                new Vector3(0, -125, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.LightGray,
                () => { BattleSelected("boss2"); },
                new BoxCollider2D(boss2)
            ));

            Instantiate(goBack = new UIButton(
                new Vector3(-sceneCamera.size.X / 2 + 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.White,
                () =>
                {
                    SceneManager.SetActive("accessorySelection");
                    SaveManager.Save("config.json");
                },
                new BoxCollider2D(goBack)
            ));
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
            boss1.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "King Slime", FontManager.Get("andyBold", 26), Color.Black, 
                new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 35));
            boss2.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Giant Probe", FontManager.Get("andyBold", 26), Color.Black,
                new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2 + 110));
            goBack.Draw(g, sceneCamera);
            cur.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            
        }

        public override void Update(float dt)
        {
            Vector3 cursorPos = sceneCamera.GetCursorOnWorld();
            cur.position.X = cursorPos.X;
            cur.position.Y = cursorPos.Y;
        }

        private void BattleSelected(string scene)
        {
            SceneManager.SetActive(scene);
        }
    }
}
