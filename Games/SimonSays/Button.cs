using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.SimonSays
{
    public class Button : Square
    {
        public int btnId;
        Color initColor;
        bool pressed;

        public Button(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor, 
            int btnId,
            float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, isActive, fillColor, borderWidth, fillTexture)
        {
            this.btnId = btnId;
            this.initColor = fillColor;
        }

        public override void Update()
        {
            
        }

        public void IsPressed(bool key)
        {
            if (key)
            {
                fillColor = Color.Gray;
            }
            else
            {
                fillColor = initColor;
            }
        }
    }
}
