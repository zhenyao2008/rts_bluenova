using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.FSM
{
    [System.Serializable]
    public class ShortVar
    {
        public static implicit operator ShortVar(short val)
        {
            return new ShortVar(val);
        }

        public short value = 0;

        public short defaultValue = 0;

        public ShortVar(short var)
        {
            value = var;
        }

        public void ResetToDefault()
        {
            value = defaultValue;
        }

    }
}