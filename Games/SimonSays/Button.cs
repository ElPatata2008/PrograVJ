using PrograVJ.Engine.Figures;
using PrograVJ.Engine.Manager;
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
        Bitmap buttonPressed;
        bool changedToPressed = false;
        bool changedToNotPressed = false;
        Bitmap buttonNotPressed;

        bool soundPlayed = false;

        public Button(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor, 
            int btnId, Bitmap buttonPressed,
            float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, isActive, fillColor, borderWidth, fillTexture)
        {
            this.btnId = btnId;
            this.initColor = fillColor;
            this.buttonPressed = buttonPressed;
            buttonNotPressed = fillTexture;
        }

        public override void Update()
        {
            
        }

        public void IsPressed(bool key)
        {
            if (key)
            {
                //fillColor = Color.Gray;
                if (!changedToPressed)
                {
                    fillTexture = buttonPressed;
                    changedToPressed = true;
                    changedToNotPressed = false;
                }
                if (!soundPlayed)
                {
                    switch (btnId)
                    {
                        case 0: AudioManager.Play("ssbtn1"); break;
                        case 1: AudioManager.Play("ssbtn2"); break;
                        case 2: AudioManager.Play("ssbtn3"); break;
                        case 3: AudioManager.Play("ssbtn4"); break;
                    }
                    soundPlayed = true;
                }
            }
            else
            {
                //fillColor = initColor;
                if (!changedToNotPressed)
                {
                    fillTexture = buttonNotPressed;
                    changedToNotPressed = true;
                    changedToPressed = false;
                }

                soundPlayed = false;
            }
        }
    }
}
