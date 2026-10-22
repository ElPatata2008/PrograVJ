using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static PrograVJ.Games.Test.Objects.Player;

namespace PrograVJ.Games.Cuphead.Objects
{
    public class Configuration : ISaveable
    {
        public string SaveID = "config";
        public int difficulty = 0;
        public string ID => SaveID;

        public Configuration() {  SaveManager.Register(this); }

        public class ConfigSaveData {
            public int diff { get; set; }
        }

        public void LoadState(object state)
        {
            if (state is string jsonString)
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                var data = JsonSerializer.Deserialize<ConfigSaveData>(jsonString, options);

                if (data == null) return;

                difficulty = data.diff;
            }
        }

        public object SaveState()
        {
            return new ConfigSaveData
            {
                diff = difficulty
            };
        }
    }
}
