using PrograVJ.Engine;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ
{
    public abstract class Game
    {
        protected Window window;
        protected Graphics g;
        protected bool loop;
        protected float fps, frameTime, sleepTime;
        protected List<GameObject> gameObjects = new List<GameObject>();

        protected Camera c;

        public Game(int w, int h, float fps, CameraType type)
        {
            window = new Window(w, h, fps);
            g = window.CreateGraphics();

            this.fps = fps;
            loop = true;

            c = new Camera( type,
                new Vector3(0f, 0f, -50f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(w, h, 0f)
            );
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
                UpdateGame();
                RenderGraphics(g);
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

        public void Instantiate(GameObject obj) => gameObjects.Add(obj);
        public void Deinstantiate(GameObject obj) => gameObjects.Remove(obj);
       
        private void UpdateGameObjects()
        {
            foreach (GameObject obj in gameObjects) obj.Update();
        }

        private void UpdateGame()
        {
            Update();
            UpdateGameObjects();
        }

        private void RenderGraphics(Graphics g)
        {
            Render(window.GetGraphics());
            window.Render();
        }

    }
}
