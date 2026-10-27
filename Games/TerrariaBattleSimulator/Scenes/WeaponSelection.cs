using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using PrograVJ.Games.TerrariaBattleSimulator.Objects;
using PrograVJ.Games.TerrariaBattleSimulator.Objects.Buttons;
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
                new Vector3(-150, 100, 0),
                Vector3.Zero,
                new Vector3(100, 50, 0),
                Color.Black, Color.White,
                TextureManager.Get("undertaker"), 0,
                new BoxCollider2D(undertaker)
            ));

            Instantiate(musket = new WpnButton(
                new Vector3(0, 100, 0),
                Vector3.Zero,
                new Vector3(100, 50, 0),
                Color.Black, Color.White,
                TextureManager.Get("musket"), 1,
                new BoxCollider2D(musket)
            ));

            Instantiate(minishark = new WpnButton(
                new Vector3(150, 100, 0),
                Vector3.Zero,
                new Vector3(100, 50, 0),
                Color.Black, Color.White,
                TextureManager.Get("minishark"), 2,
                new BoxCollider2D(minishark)
            ));

            if (TBS.config.boss1Defeated && TBS.config.boss2Defeated)
            {
                Instantiate(vortex = new WpnButton(
                    new Vector3(300, 100, 0),
                    Vector3.Zero,
                    new Vector3(100, 50, 0),
                    Color.Black, Color.White,
                    TextureManager.Get("vortex"), 3,
                    new BoxCollider2D(vortex)
                ));
            }

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
                Color.Black, Color.White,
                () =>
                {
                    SceneManager.SetActive("accessorySelection");
                    SaveManager.Save("config.json");
                },
                new BoxCollider2D(Continue)
            ));

            Instantiate(goBack = new UIButton(
                new Vector3(-sceneCamera.size.X / 2 + 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.White,
                () =>
                {
                    SceneManager.SetActive("battleSelection");
                    SaveManager.Save("config.json");
                },
                new BoxCollider2D(goBack)
            ));
        }

        public override void Init()
        {
            Continue.delayTimer = 0;
            goBack.delayTimer = 0;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            undertaker.Draw(g,sceneCamera);
            musket.Draw(g, sceneCamera);
            minishark.Draw(g, sceneCamera);
            if (TBS.config.boss1Defeated && TBS.config.boss2Defeated) vortex.Draw(g, sceneCamera);
            classic.Draw(g, sceneCamera);
            expert.Draw(g, sceneCamera);
            master.Draw(g, sceneCamera);

            Continue.Draw(g, sceneCamera);
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
    }
}
