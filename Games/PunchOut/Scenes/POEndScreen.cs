using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.PunchOut.Scenes
{
    public class POEndScreen : Scene
    {
        string endText = "";
        public POEndScreen(Camera c) : base(c)
        {
        }

        public override void Init()
        {
            AudioManager.StopMusic();
            switch (PunchOut.endType)
            {
                case 1: { 
                        endText = "VICTORY!";
                        AudioManager.PlaySFX("win");
                    } break;
                case 2: { 
                        endText = "DEFEAT!"; 
                        AudioManager.PlaySFX("defeat");
                    } break;
                case 3: { 
                        endText = "TIME UP!"; 
                        AudioManager.PlaySFX("defeat");
                    } break;
            }
        }

        public override void PaintScreen(Graphics g)
        {
            DrawingUtils.DrawCenterText(g, endText, FontManager.Get("po", 30), Color.White, new PointF(sceneCamera.size.X / 2, sceneCamera.size.Y / 2));
        }

        public override void ProcessInput()
        {
            
        }

        public override void Update(float dt)
        {
            
        }
    }
}
