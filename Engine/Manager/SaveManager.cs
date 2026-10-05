using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Manager
{
    public class SaveManager
    {
        public static List<ISaveable> saveables = new List<ISaveable>();
        private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        { WriteIndented = true, IncludeFields = true};

        public static void Register(ISaveable s)
        {
            if (!saveables.Contains(s)) saveables.Add(s);
        }

        public static void Unregister(ISaveable s) => saveables.Remove(s);

        public static void Save(string filePath)
        {
            SaveData container = new SaveData() { SaveTime = DateTime.Now };

            foreach (var item in saveables)
            {
                if (string.IsNullOrEmpty(item.ID)) continue;

                object state = item.SaveState();
                string jsonState = JsonSerializer.Serialize(state, jsonOptions);
                container.ObjectStates[item.ID] = jsonState;
            }

            string fullJson = JsonSerializer.Serialize(container, jsonOptions);
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, fullJson);
        }

        public static void Load(string filePath)
        {
            Console.WriteLine($"{filePath}: {File.Exists(filePath)}");
            if (!File.Exists(filePath)) { 
                return; }

            string fullJson = File.ReadAllText(filePath);
            var container = JsonSerializer.Deserialize<SaveData>(fullJson);

            foreach(var item in saveables)
            {
                if (container.ObjectStates.TryGetValue(item.ID, out string rawJson))
                {
                    item.LoadState(rawJson);
                }
            }
        }
    }
}
