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
    public class AccessorySelection : Scene
    {
        Square cur;
        AccButton tabi;
        AccButton ciab;
        AccButton aotw;
        AccButton panl;
        AccButton breg;
        AccButton shak;
        AccButton cros;
        AccButton scen;

        UIButton Continue;
        UIButton goBack;

        public AccessorySelection(Camera c) : base(c)
        {
            Instantiate(cur = new Square(
                new Vector3(InputManager.MouseLocation.X, InputManager.MouseLocation.Y, 0),
                Vector3.Zero,
                Vector3.One,
                Color.Black, Color.White,
                new PointCollider(cur)
            ));

            Instantiate(tabi = new AccButton(
                new Vector3(-sceneCamera.size.X / 2 + 100, sceneCamera.size.Y / 2 - 100, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("tabi"), 0,
                new BoxCollider2D(tabi)
            ));

            Instantiate(ciab = new AccButton(
                new Vector3(-sceneCamera.size.X / 2 + 250, sceneCamera.size.Y / 2 - 100, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("ciab"), 1,
                new BoxCollider2D(ciab)
            ));

            Instantiate(aotw = new AccButton(
                new Vector3(-sceneCamera.size.X / 2 + 400, sceneCamera.size.Y / 2 - 100, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("aotw"), 2,
                new BoxCollider2D(aotw)
            ));

            Instantiate(panl = new AccButton(
                new Vector3(-sceneCamera.size.X / 2 + 550, sceneCamera.size.Y / 2 - 100, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("panl"), 3,
                new BoxCollider2D(panl)
            ));

            Instantiate(breg = new AccButton(
                new Vector3(-sceneCamera.size.X / 2 + 700, sceneCamera.size.Y / 2 - 100, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("breg"), 4,
                new BoxCollider2D(breg)
            ));

            Instantiate(shak = new AccButton(
                new Vector3(-150, sceneCamera.size.Y / 2 - 250, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("shak"), 5,
                new BoxCollider2D(shak)
            ));

            Instantiate(cros = new AccButton(
                new Vector3(0, sceneCamera.size.Y / 2 - 250, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("cros"), 6,
                new BoxCollider2D(cros)
            ));

            Instantiate(scen = new AccButton(
                new Vector3(150, sceneCamera.size.Y / 2 - 250, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("scen"), 7,
                new BoxCollider2D(scen)
            ));

            Instantiate(Continue = new UIButton(
                new Vector3(sceneCamera.size.X / 2 - 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.White,
                () =>
                {
                    SceneManager.SetActive("battleSelection");
                    SaveManager.Save("config.json");
                },
                new BoxCollider2D(Continue)
            ));

            Instantiate(goBack = new UIButton(
                new Vector3(- sceneCamera.size.X / 2 + 150, -sceneCamera.size.Y / 2 + 50, 0),
                Vector3.Zero,
                new Vector3(200, 50, 0),
                Color.Black, Color.White,
                () =>
                {
                    SceneManager.SetActive("weaponSelection");
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
            tabi.Draw(g, sceneCamera);
            ciab.Draw(g, sceneCamera);
            aotw.Draw(g, sceneCamera);
            panl.Draw(g, sceneCamera);
            breg.Draw(g, sceneCamera);
            shak.Draw(g, sceneCamera);
            cros.Draw(g, sceneCamera);
            scen.Draw(g, sceneCamera);

            Continue.Draw(g, sceneCamera);
            goBack.Draw(g, sceneCamera);

            cur.Draw(g, sceneCamera);
        }

        public override void ProcessInput()
        {
            if (InputManager.JustPressedInput("escape"))
            {
                SaveManager.Save("config.json");
                Environment.Exit(0);
            }
        }

        public override void Update(float dt)
        {
            Vector3 cursorPos = sceneCamera.GetCursorOnWorld();
            cur.position.X = cursorPos.X;
            cur.position.Y = cursorPos.Y;
        }
    }
}
