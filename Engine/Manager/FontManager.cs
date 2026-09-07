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
        private static PrivateFontCollection Fonts;
        private static int fontIndex = 0;
        private static Dictionary<string, int> IndexMap;
        private static Dictionary<string, int> FilenameMap;

        public static void Load(string filename, string id)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException();
            if (IndexMap.ContainsKey(id)) throw new ArgumentException();

            if (FilenameMap.ContainsKey(id)) IndexMap.Add(id, FilenameMap[filename]);
            else
            {
                Fonts.AddFontFile(filename);
                IndexMap.Add(id, fontIndex); FilenameMap.Add(id, fontIndex);
                fontIndex++;
            }
        }

        public static Font Get(string id, int size)
        {
            if (!IndexMap.ContainsKey(id)) throw new ArgumentException();
            int idx = IndexMap[id];
            return new Font(Fonts.Families[idx], size);
        }
    }
}
