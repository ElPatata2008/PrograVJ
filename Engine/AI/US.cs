using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.AI
{

    public abstract class Consideration
    {
        public abstract float Evaluate(AIController controller);
    }

    public class UtilityAction
    {
        string actionName;
        List<Consideration> considerations = new List<Consideration>();
        Action<AIController> actionLogic;

        public float Score(AIController controller)
        {
            float score = 1f;
            foreach(var con in considerations)
            {
                score *= con.Evaluate(controller);
            }
            return score;
        }

        public void Execute(AIController controller) => actionLogic(controller);
    }

    public class US : Strategy
    {
        AIController controller;
        List<UtilityAction> actions;
        UtilityAction curr;

        public US(AIController controller) { this.controller = controller; }

        public override void Update(float deltaTime)
        {
            UtilityAction bestAction = null;
            float highestScore = -1;
            foreach (var action in actions)
            {
                float score = action.Score(controller);
                if (score > highestScore)
                {
                    bestAction = action;
                    highestScore = score;
                }
            }

            if (bestAction != null)
            {
                curr = bestAction;
                curr.Execute(controller);
            }
        }
    }
}
