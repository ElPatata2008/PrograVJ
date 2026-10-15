using PrograVJ.Engine;
using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.PunchOut.Scenes
{
    public class POIntro : Scene
    {

        Stopwatch sw = new Stopwatch();
        Square lm;
        Square gj;

        AnimationController lmac = new AnimationController();
        AnimationController gjac = new AnimationController();
         
        public POIntro(Camera c) : base(c)
        {
            SpriteSheet lmIntro = new SpriteSheet(TextureManager.Get("lmIntro"), 81, 73, 0.5f, 4);
            lmac.AddAnimation("intro", lmIntro);

            lm = new Square(
                new Vector3(-200, 0, 0),
                Vector3.Zero,
                Vector3.One * 400,
                Color.White, Color.White, animationController: lmac
            );

            SpriteSheet gjIntro = new SpriteSheet(TextureManager.Get("gjIntro"), 81, 79, 0.5f, 2);
            gjac.AddAnimation("intro", gjIntro);

            gj = new Square(
                new Vector3(200, 0, 0),
                Vector3.Zero,
                Vector3.One * 400,
                Color.White, Color.White, animationController: gjac
            );

        }

        public override void Init()
        {
            AudioManager.PlaySFX("intro");
            lmac.Play("intro");
            gjac.Play("intro");
            sw.Start();
        }

        public override void PaintScreen(Graphics g)
        {
            lm.Draw(g, sceneCamera);
            gj.Draw(g, sceneCamera);
            DrawingUtils.DrawCenterText(g, "LITTLE MAC", FontManager.Get("po", 18), Color.White, new PointF(150, + 50));
            DrawingUtils.DrawCenterText(g, "V/S", FontManager.Get("po", 18), Color.White, new PointF(sceneCamera.size.X / 2, + 50));
            DrawingUtils.DrawCenterText(g, "GLASS JOE", FontManager.Get("po", 18), Color.White, new PointF(sceneCamera.size.X - 150, + 50));
        }

        public override void ProcessInput()
        {
        }

        public override void Update(float dt)
        {
            lmac.Update(dt);
            gjac.Update(dt);
            if (sw.ElapsedMilliseconds / 1000 > 7)
            {
                sw.Stop();
                SceneManager.SetActive("game");
            }
        }
    }
}
