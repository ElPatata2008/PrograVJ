using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ
{
    public abstract class GameLoader
    {
        Window window;
        protected Graphics g;
        protected bool loop;
        //protected List<GameObjects> gameObjects = new List<GameObjects>();
        protected float fps, frameTime, sleepTime;

        public GameLoader(int w, int h, float fps)
        {
            window = new Window(w, h, fps);
            g = window.CreateGraphics();

            this.fps = fps;
            loop = true;

        }

        public void StartGame()
        {
            window.Show();
            Thread t = new Thread(Loop);
            t.Start();
        }

        private void Loop()
        {
            while (loop)
            {
                if (!loop) break;

                Stopwatch sw = new Stopwatch();
                sw.Start();
                ProcessInput();
                Update();
                RenderGraphics(g);
                //Render(g);
                sw.Stop();

                frameTime = sw.ElapsedMilliseconds;
                sleepTime = 1000 / fps - frameTime;
                if (sleepTime < 0) sleepTime = 1;
                Thread.Sleep((int)sleepTime);
                sw.Reset();
            }

            Environment.Exit(0);
        }


        protected abstract void ProcessInput();
        protected abstract void Update();
        protected abstract void Render(Graphics g);

        private void RenderGraphics(Graphics g)
        {
            Render(window.GetGraphics());
            window.Render();
        }

    }
}
