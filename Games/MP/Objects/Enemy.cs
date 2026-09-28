using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.MP.Objects
{
    public class Enemy : Square
    {
        public int hp;
        public float speed { get; set; }
        public Enemy(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor, float borderWidth = 2, Bitmap fillTexture = null) : base(position, rotation, size, color, isActive, fillColor, borderWidth, fillTexture)
        {
        }
    }
}
