using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Manager
{
    public class SceneManager
    {
        public static Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        public static string ActiveIndex = null;

        public static void Register(Scene scene, string sceneID)
        {
            if (scenes.ContainsKey(sceneID)) { throw new ArgumentException(sceneID); }
            scenes[sceneID] = scene;
        }

        public static void SetActive(string sceneID)
        {
            if (!scenes.ContainsKey(sceneID)) { throw new ArgumentException(sceneID); }
            ActiveIndex = sceneID;
            scenes[ActiveIndex].Init();
        }

        public static Scene GetActive() => scenes[ActiveIndex];

    }
}
