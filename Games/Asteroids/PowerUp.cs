using PrograVJ.Engine.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Games.Asteroids
{
    public class PowerUp : Square
    {
        public PowerUp(Vector3 position, Vector3 rotation, Vector3 size, Color color, bool isActive, Color fillColor) : base(position, rotation, size, color, isActive, fillColor)
        {
        }

        public override void Update()
        {
            rotation.Z += 10f;
        }
    }
}
