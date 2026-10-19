using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.AI
{
    public class Blackboard
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        public void SetValue(string key, object value)
        {
            if (data.ContainsKey(key)) throw new ArgumentException(key);
            data[key] = value;
        }

        public object GetValue(string key) 
        {
            if (!data.ContainsKey(key)) throw new ArgumentException(key);
            return data[key];
        }

        public bool HasKey(string key) => data.ContainsKey(key);

    }
}
