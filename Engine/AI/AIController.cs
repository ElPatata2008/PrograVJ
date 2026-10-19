using PrograVJ.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.AI
{
    public class AIController
    {
        public GameObject pawn;
        public Blackboard bb;
        public Strategy brain;

        public AIController(Blackboard bb, Strategy brain)
        {
            this.bb = bb;
            this.brain = brain;
        }

        public void Update(float deltaTime)
        {
            if (pawn == null) return;
            brain.Update(deltaTime);
        }
    }
}
