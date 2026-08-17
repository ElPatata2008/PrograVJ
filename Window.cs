using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace PrograVJ
{
    class Window : Form
    {
        List<Keys> pressedKeys;
        float fps;

        public Window(int w, int h, float fps)
        {
            ClientSize = new Size(w, h);
            this.fps = fps;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            pressedKeys = new List<Keys>();
            KeyDown += _KeyDownLogic;
            KeyUp += _KeyUpLogic;
        }

        private void _KeyDownLogic(object sender, KeyEventArgs e)
        {
            if(!pressedKeys.Contains(e.KeyCode))
                pressedKeys.Add(e.KeyCode);
        }

        private void _KeyUpLogic(object sender, KeyEventArgs e) { 
        
            if(pressedKeys.Contains(e.KeyCode))
                pressedKeys.Remove(e.KeyCode); 
        }

        public bool IsPressedKey(Keys key) => pressedKeys.Contains(key);
    }
}
