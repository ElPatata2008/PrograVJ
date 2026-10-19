using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.AI
{
    public abstract class Strategy
    {
        public Strategy() { }

        public abstract void Update(float deltaTime);
    }
}
