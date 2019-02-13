using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.FSM
{
    [System.Serializable]
    public class FiniteStateMachineConfigs
    {
        public FiniteStateMachineConfig[] finiteStateMachineArray;
    }
    [System.Serializable]
    public class FiniteStateMachineConfig
    {
        public int id;
        public string name;
        public string[] conditions;
        public StateConfig[] states;
        public TransitionConfig[] transitions;
    }
    [System.Serializable]
    public class StateConfig
    {
        public int subFSMId;
        public string stateId;
        public string[] actions;
        public Vector3Int position;
    }
    [System.Serializable]
    public class TransitionConfig
    {
        public ConditionIdValue[] conditionIdValues;
        public string fromStateId;
        public string toStateId;
    }
    [System.Serializable]
    public class ConditionIdValue
    {
        public string conditionId;
        public int conditionValue;
    }
    [System.Serializable]
    public class FiniteStateMachineData
    {
        public string name;
        public List<StateData> states;
        public List<ConditionData> conditions;
    }
    [System.Serializable]
    public class StateData
    {
        public string stateName;
        public List<string> actions;
        public List<TransitionData> transitions;
    }
    [System.Serializable]
    public class TransitionData
    {
        public List<TransitionConditionData> keyValueDatas;
        public string fromState;
        public string toState;
    }
    [System.Serializable]
    public class ConditionData
    {
        public string condition;
        public bool value;
        public bool defaultValue;
    }
    [System.Serializable]
    public class TransitionConditionData
    {
        public string condtion;
        public bool targetValue;
    }
}
