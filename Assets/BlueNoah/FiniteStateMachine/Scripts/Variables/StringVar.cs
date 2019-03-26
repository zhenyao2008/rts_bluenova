using System.Collections;
using System.Collections.Generic;

namespace BlueNoah.AI.FSM
{
    public class StringVar
    {

        public static implicit operator StringVar(string val)
        {
            return new StringVar(val);
        }

        public string value;

        public string defaultValue = string.Empty;

        public StringVar(string var)
        {
            value = var;
        }

        public void ResetToDefault()
        {
            value = defaultValue;
        }
    }
}
