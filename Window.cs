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
    public class Window : Form
    {
        List<Keys> pressedKeys;
        float fps;

        BufferedGraphicsContext GraphicsManager;
        BufferedGraphics managedBackBuffer;

        public Window(int w, int h, float fps)
        {
            ClientSize = new Size(w, h);
            this.fps = fps;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            GraphicsManager = BufferedGraphicsManager.Current;
            GraphicsManager.MaximumBuffer = new Size(w, h);
            managedBackBuffer = GraphicsManager.Allocate(CreateGraphics(), ClientRectangle);

            pressedKeys = new List<Keys>();
            KeyDown += _KeyDownLogic;
            KeyUp += _KeyUpLogic;
        }

        public Graphics GetGraphics()
        {
            managedBackBuffer.Graphics.Clear(Color.Black);
            return managedBackBuffer.Graphics;
        }

        public void Render() => managedBackBuffer.Render(); // Swap Buffer

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
