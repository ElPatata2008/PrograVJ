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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            float col1Pos = -sceneCamera.size.X / 2 + 75;
            float col2Pos = 60;
            Instantiate(cur = new Square(
                new Vector3(InputManager.MouseLocation.X, InputManager.MouseLocation.Y, 0),
                Vector3.Zero,
                Vector3.One,
                Color.Black, Color.White,
                new PointCollider(cur)
            ));

            Instantiate(tabi = new AccButton(
                new Vector3(col1Pos, sceneCamera.size.Y / 2 - 75, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("tabi"), 0,
                new BoxCollider2D(tabi)
            ));

            Instantiate(ciab = new AccButton(
                new Vector3(col1Pos, sceneCamera.size.Y / 2 - 200, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("ciab"), 1,
                new BoxCollider2D(ciab)
            ));

            Instantiate(aotw = new AccButton(
                new Vector3(col1Pos, sceneCamera.size.Y / 2 - 325, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("aotw"), 2,
                new BoxCollider2D(aotw)
            ));

            Instantiate(panl = new AccButton(
                new Vector3(col1Pos, sceneCamera.size.Y / 2 - 450, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("panl"), 3,
                new BoxCollider2D(panl)
            ));

            Instantiate(breg = new AccButton(
                new Vector3(col2Pos, sceneCamera.size.Y / 2 - 75, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("breg"), 4,
                new BoxCollider2D(breg)
            ));

            Instantiate(shak = new AccButton(
                new Vector3(col2Pos, sceneCamera.size.Y / 2 - 200, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("shak"), 5,
                new BoxCollider2D(shak)
            ));

            Instantiate(cros = new AccButton(
                new Vector3(col2Pos, sceneCamera.size.Y / 2 - 325, 0),
                Vector3.Zero,
                Vector3.One * 100,
                Color.Black, Color.White,
                TextureManager.Get("cros"), 6,
                new BoxCollider2D(cros)
            ));

            Instantiate(scen = new AccButton(
                new Vector3(col2Pos, sceneCamera.size.Y / 2 - 450, 0),
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
                Color.Black, Color.LightGray,
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
                Color.Black, Color.LightGray,
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
            tabi.delayTimer = 0;
            ciab.delayTimer = 0;
            aotw.delayTimer = 0;
            panl.delayTimer = 0;
            breg.delayTimer = 0;
            shak.delayTimer = 0;
            cros.delayTimer = 0;
            scen.delayTimer = 0;
        }

        public override void PaintScreen(Graphics g)
        {
            g.Clear(Color.White);
            tabi.Draw(g, sceneCamera);
            g.DrawString("Allows the ability to dash \nDouble tap a direction", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(135, 65 - 15));
            ciab.Draw(g, sceneCamera);
            g.DrawString("Allows the holder to double \njump", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(135, 190 - 15));
            aotw.Draw(g, sceneCamera);
            g.DrawString("10% increased movement \nspeed", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(135, 315 - 15));
            panl.Draw(g, sceneCamera);
            g.DrawString("Doubles movement speed \nafter taking damage", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(135, 440 - 15));
            breg.Draw(g, sceneCamera);
            g.DrawString("Triples natural health \nregeneration", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(sceneCamera.size.X / 2 + 120, 65 - 15));
            shak.Draw(g, sceneCamera);
            g.DrawString("Provides 5 Defense", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(sceneCamera.size.X / 2 + 120, 190));
            cros.Draw(g, sceneCamera);
            g.DrawString("Doubles length of invicibility \nafter taking damage", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(sceneCamera.size.X / 2 + 120, 315 - 15));
            scen.Draw(g, sceneCamera);
            g.DrawString("+10% increased damage \n+5% Crit Chance", FontManager.Get("andyBold", 18), Brushes.Black, new PointF(sceneCamera.size.X / 2 + 120, 440 - 15));

            Continue.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Continue", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X - 150, sceneCamera.size.Y - 65));
            goBack.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "Back", FontManager.Get("andyBold", 26), Color.Black, new PointF(150, sceneCamera.size.Y - 65));
            DrawingUtils.DrawCenterText(g, "Max 4 Accessories", FontManager.Get("andyBold", 26), Color.Black, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y - 65));

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
