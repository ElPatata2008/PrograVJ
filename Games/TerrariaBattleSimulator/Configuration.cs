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

namespace PrograVJ.Games.TerrariaBattleSimulator.Objects
{
    public class Configuration : ISaveable
    {
        public string SaveID = "config";
        public string ID => SaveID;
        public int difficulty = 0;
        public bool boss1Defeated = false;
        public bool boss2Defeated = false;
        public List<bool> equipment = new List<bool>()
        {
            false,
            false,
            false,
            false,
            false
        };

        public int weapon = 0;

        public Configuration() {  SaveManager.Register(this); }

        public class ConfigSaveData {
            public int diff { get; set; }
            public bool b1d { get; set; }
            public bool b2d { get; set; }
            public List<bool> eq { get; set; }
            public int w { get; set; }
        }

        public void LoadState(object state)
        {
            if (state is string jsonString)
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                var data = JsonSerializer.Deserialize<ConfigSaveData>(jsonString, options);

                if (data == null) return;

                difficulty = data.diff;
                boss1Defeated = data.b1d;
                boss2Defeated = data.b2d;
                equipment = data.eq;
                weapon = data.w;
            }
        }

        public object SaveState()
        {
            return new ConfigSaveData
            {
                diff = difficulty,
                b1d = boss1Defeated,
                b2d = boss2Defeated,
                eq = equipment,
                w = weapon
            };
        }
    }
}
