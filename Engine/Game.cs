using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
                new Vector3(0f, 0f, -100f), 
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
            Stopwatch sw = new Stopwatch();
            while (loop)
            {
                if (!loop) break;

                float deltaTime = (float)sw.Elapsed.TotalSeconds;
                sw.Restart();
                ProcessInput();
                UpdateGame(deltaTime);
                RenderGraphics(g);
                sw.Stop();

                frameTime = sw.ElapsedMilliseconds;
                sleepTime = 1000 / fps - frameTime;
                if (sleepTime < 0) sleepTime = 1;
                Thread.Sleep((int)sleepTime);
            }

            Environment.Exit(0);
        }


        protected void Init() { SceneManager.GetActive().Init(); }
        protected void ProcessInput() { SceneManager.GetActive().ProcessInput(); }
        protected void Update(float dt) { SceneManager.GetActive().Update(dt); }
        protected void Render(Graphics g) {
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            SceneManager.GetActive().PaintScreen(g);  
        }

        public void Instantiate(GameObject obj) => gameObjects.Add(obj);
        public void Deinstantiate(GameObject obj) => gameObjects.Remove(obj);
       
        private void UpdateGameObjects(float dt)
        {
            foreach (GameObject obj in SceneManager.GetActive().sceneGameObjects)
            {
                obj.Update(dt);
                if (obj.body != null) obj.body.Update(dt);
                obj.PhysicsUpdate(dt, SceneManager.GetActive().sceneGameObjects);
                if (obj.collider != null) obj.collider.Update(dt);
            }
        }

        private void UpdateGame(float dt)
        {
            Update(dt);
            UpdateGameObjects(dt);
        }

        private void RenderGraphics(Graphics g)
        {
            Render(window.GetGraphics());
            window.Render();
            //foreach (GameObject obj in SceneManager.GetActive().sceneGameObjects) obj.Draw(g, c);
        }

    }
}
