using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PrograVJ.Engine.Manager
{
    public static class TextureManager 
    {
        private static Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
        private static string path = "Assets/Images/";

        public static void Load(string filename, string id)
        {
            string file = Path.Combine(path, filename);
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (images.ContainsKey(id)) throw new ArgumentException(id);
            images.Add(id, new Bitmap(file));
        }

        public static Bitmap Get(string id)
        {
            if (!images.ContainsKey(id)) throw new ArgumentException(id);
            return images[id];
        }
    }
}
