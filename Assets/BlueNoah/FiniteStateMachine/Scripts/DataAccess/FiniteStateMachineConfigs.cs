using System.Collections.Generic;
using UnityEngine;
//Flame Synchonisition
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
        public BoolParam[] boolParam;
        public ShortParam[] shortParam;
        public IntParam[] intParams;
        public StringParam[] stringParams;
        public StateConfig[] states;
        public TransitionConfig[] transitions;
    }

    [System.Serializable]
    public class StateConfig
    {
        public int subFSMId;
        public short stateId;
        public string[] actions;
        public Vector3 position;
    }
    [System.Serializable]
    public class TransitionConfig
    {
        public ConditionIdValue[] conditionIdValues;
        public short fromStateId;
        public short toStateId;
    }
    [System.Serializable]
    public class ConditionIdValue
    {
        //唯一のキー
        public short conditionId;
        public string conditionName;
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
        public short fromState;
        public short toState;
    }
    [System.Serializable]
    public class ConditionData
    {
        public short conditionId;
        public bool value;
        public bool defaultValue;
    }
    [System.Serializable]
    public class TransitionConditionData
    {
        public short condition;
        public bool targetValue;
    }

    [System.Serializable]
    public class StringParam
    {
        public string paramName;
        public string paramValue;
    }

    [System.Serializable]
    public class ShortParam
    {
        public string paramName;
        public short paramValue;
    }

    [System.Serializable]
    public class IntParam
    {
        public string paramName;
        public int paramValue;
    }

    [System.Serializable]
    public class BoolParam
    {
        public string paramName;
        public bool paramValue;
    }

    // [System.Serializable]
    // public class VectorIntArrayParam
    // {
    //     public string paramName;
    //     public List<Vector3Int> paramValue;
    // }

    // [System.Serializable]
    // public class IntArrayParam
    // {
    //     public string paramName;
    //     public int[] paramValue;
    // }

    // [System.Serializable]
    // public class StringArrayParam
    // {
    //     public string paramName;
    //     public string[] paramValue;
    // }

    // [System.Serializable]
    // public class Vector3IntParam
    // {
    //     public string paramName;
    //     public Vector3Int paramValue;
    // }

    // [System.Serializable]
    // public class ActionWithParams
    // {
    //     //Action class name , full path , include namespace.
    //     public string action;
    //     public StringParam[] stringParams;
    //     public IntParam[] intParams;
    //     public VectorIntArrayParam[] vectorArrayParams;
    //     public IntArrayParam[] intArrayParams;
    //     public StringArrayParam[] stringArrayParams;
    // }
}
