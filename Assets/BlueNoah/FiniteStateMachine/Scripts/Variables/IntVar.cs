using System.Collections;
using System.Collections.Generic;

namespace BlueNoah.AI.FSM
{
    [System.Serializable]
    public class IntVar
    {

        public static implicit operator IntVar(int val)
        {
            return new IntVar(val);
        }

        public int value = 0;

        public int defaultValue = 0;

        public IntVar(int var)
        {
            value = var;
        }

        public void ResetToDefault()
        {
            value = defaultValue;
        }
    }
}
