using PrograVJ.Engine;
using PrograVJ.Engine.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static PrograVJ.Games.Test.Objects.Player;

namespace PrograVJ.Games.Mariano.Objects
{
    public class Leaderboard : ISaveable
    {
        public string SaveID = "Leaderboard";
        public List<int> scores = new List<int>();
        public Leaderboard() { SaveManager.Register(this); }

        public class LeaderboardSaveData
        {
            public List<int> scores { get; set; }
        }

        public void AddScore(int score) => scores.Add(score);
        public bool ScoreListEmpty() => scores.Count == 0;

        public string ID => SaveID;

        public void LoadState(object state)
        {
            if (state is string jsonString)
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                var data = JsonSerializer.Deserialize<LeaderboardSaveData>(jsonString, options);

                if (data == null) return;

                scores = data.scores;
            }
        }

        public object SaveState() => new LeaderboardSaveData { scores = scores };
    }
}
