using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine
{
    public interface ISaveable
    {
        string ID { get; }
        object SaveState();
        void LoadState(object state);
    }
    [Serializable] public class SaveData
    {
        public string SaveName { get; set; } = "SaveData";
        public DateTime SaveTime { get; set; } = DateTime.Now;
        public float PlaytimeSeconds { get; set; }

        public Dictionary<string, string> ObjectStates { get; set; } = new Dictionary<string, string>();
    }
}
