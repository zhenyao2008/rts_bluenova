namespace BlueNoah.AI.FSM
{
    [System.Serializable]
    public class BoolVar
    {
        public static implicit operator BoolVar(bool val)
        {
            return new BoolVar(val);
        }

        public bool value = false;

        public bool defaultValue = false;

        public BoolVar(bool var)
        {
            value = var;
        }

        public void ResetToDefault()
        {
            value = defaultValue;
        }

    }

    public class ConditionKeyValue
    {
        public BoolVar boolVar;

        public bool targetValue;

        public ConditionKeyValue(BoolVar boolVar, bool value)
        {
            this.boolVar = boolVar;
            this.targetValue = value;
        }
    }
}
