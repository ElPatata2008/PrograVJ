using NAudio.Dmo.Effect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Manager
{
    public static class FontManager
    {
        private static PrivateFontCollection Fonts = new PrivateFontCollection();
        private static int fontIndex = 0;
        private static Dictionary<string, int> IndexMap = new Dictionary<string, int>();
        private static Dictionary<string, int> FilenameMap = new Dictionary<string, int>();
        private static string path = "Assets/Fonts/";

        public static void Load(string filename, string id)
        {
            string file = Path.Combine(path, filename);
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (IndexMap.ContainsKey(id)) throw new ArgumentException(id);

            if (FilenameMap.ContainsKey(id)) IndexMap.Add(id, FilenameMap[file]);
            else
            {
                Fonts.AddFontFile(file);
                IndexMap.Add(id, fontIndex); FilenameMap.Add(id, fontIndex);
                fontIndex++;
            }
        }

        public static Font Get(string id, int size, FontStyle fs = FontStyle.Regular)
        {
            if (!IndexMap.ContainsKey(id)) throw new ArgumentException(id);
            int idx = IndexMap[id];
            return new Font(Fonts.Families[idx], size, fs);
        }
    }
}
