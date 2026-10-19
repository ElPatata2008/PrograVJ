using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.AI
{
    public enum NodeState { Running, Success, Failure }
    public abstract class BTNode
    {
        public abstract NodeState Evaluate(AIController controller);
    }

    public class BT : Strategy
    {
        AIController controller;
        BTNode root;

        public BT(AIController controller, BTNode root) 
        {
            this.controller = controller;
            this.root = root;
        }

        public void SetRoot(BTNode node) => root = node;

        public override void Update(float deltaTime) => root.Evaluate(controller);
    }
}
