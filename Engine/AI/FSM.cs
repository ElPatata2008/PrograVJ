using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.AI
{
    public abstract class FSMState
    {
        public AIController controller;
        public FSMState(AIController controller) { this.controller = controller; }

        public abstract void OnExit();
        public abstract void OnEnter();
        public abstract void Update(float deltaTime);
    }

    public class FSM : Strategy
    {
        Dictionary<string, FSMState> states = new Dictionary<string, FSMState>();
        FSMState prev;
        FSMState curr;

        public FSM() { }

        public void AddState(string name, FSMState state)
        {
            if (states.ContainsKey(name)) throw new ArgumentException(name);
            states[name] = state;
            if (curr == null)
            {
                curr = state;
                ChangeState(name);
            }
        }

        public void ChangeState(string name)
        {
            FSMState next = states[name];
            curr.OnExit();
            prev = curr;
            curr = next;
            curr.OnEnter();
        }

        public override void Update(float deltaTime) => curr.Update(deltaTime); 
    }
}
