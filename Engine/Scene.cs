using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrograVJ.Engine
{
    public abstract class Scene
    {
        public List<GameObject> sceneGameObjects = new List<GameObject>();
        public Camera sceneCamera;

        public Scene(Camera c)
        {
            sceneCamera = c;
        }

        protected void Instantiate(GameObject go)
        {
            if (sceneGameObjects.Contains(go)) { throw new ArgumentException(); }
            sceneGameObjects.Add(go);
        }

        protected void Destroy(GameObject go)
        {
            if (!sceneGameObjects.Contains(go)) { throw new ArgumentException(); }
            sceneGameObjects.Remove(go);
        }

        public abstract void Init();
        public abstract void ProcessInput();
        public abstract void Update();
        public abstract void PaintScreen(Graphics g);
    }
}
