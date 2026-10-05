using PrograVJ.Engine;
using PrograVJ.Engine.Colliders;
using PrograVJ.Engine.Manager;
using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PrograVJ.Engine.Figures;

namespace PrograVJ.Games.Test.Objects
{
    public class Player : Square, ISaveable
    {
        public string SaveID = "Player";
        public int score = 0;

        public Player(Vector3 position, Vector3 rotation, Vector3 size, Color color, Color fillColor, Collider collider = null, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, fillColor, collider, borderWidth, fillTexture)
        {
            SaveManager.Register(this);
        }

        public class PlayerSaveData {
            public Vector3 position { get; set; }
            public Vector3 rotation { get; set; }
            public Vector3 scale { get; set; }
            public int puntaje { get; set; }
        }

        public string ID => SaveID;

        public void LoadState(object state)
        {
            if (state is string jsonString)
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                var data = JsonSerializer.Deserialize<PlayerSaveData>(jsonString, options);

                if (data == null) return; 

                position = data.position;
                rotation = data.rotation;
                size = data.scale;
                score = data.puntaje;

                Console.WriteLine($"{data.position}");
                Console.WriteLine($"{data.rotation}");
                Console.WriteLine($"{data.scale}");
                Console.WriteLine($"{data.puntaje}");

            }
        }

        public override void OnColiisionStay(GameObject c)
        {
            if (c is Square square) square.fillColor = Color.Red;
            if (c is Polygon circle) circle.fillColor = Color.Blue;
        }

        public override void OnCollisionEnter(GameObject c)
        {
            if (c is Polygon) score++;
        }

        public override void OnCollisionExit(GameObject c)
        {
            if (c is Square square) square.fillColor = Color.White;
            if (c is Polygon circle) circle.fillColor = Color.White;
        }

        public object SaveState()
        {
            return new PlayerSaveData
            {
                position = position,
                rotation = rotation,
                scale = size,
                puntaje = score
            };
        }

        public override void Update(float dt)
        {

        }
    }
}
