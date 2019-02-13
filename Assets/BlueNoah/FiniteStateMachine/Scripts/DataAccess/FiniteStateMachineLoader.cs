using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace BlueNoah.AI.FSM
{

    public static class FiniteStateMachineLoader
    {

        public const string M_FSM_CONFIG = "fsms/j_fsm";

        public static FiniteStateMachineConfigs unitAIConfig;

        public static Dictionary<int, FiniteStateMachineConfig> finiteStateMachineConfigDic;

        public static Dictionary<int, FiniteStateMachineConfig> LoadAIConfig()
        {
            finiteStateMachineConfigDic = new Dictionary<int, FiniteStateMachineConfig>();
            TextAsset textAsset = Resources.Load<TextAsset>(M_FSM_CONFIG);
            unitAIConfig = JsonUtility.FromJson<FiniteStateMachineConfigs>(textAsset.text);
            for (int i = 0; i < unitAIConfig.finiteStateMachineArray.Length; i++)
            {
                finiteStateMachineConfigDic.Add(unitAIConfig.finiteStateMachineArray[i].id, unitAIConfig.finiteStateMachineArray[i]);
            }
            return finiteStateMachineConfigDic;
        }

        public static void InitFSM(FiniteStateMachine finiteStateMachine, int configId)
        {
            if (finiteStateMachineConfigDic == null)
            {
                LoadAIConfig();
            }

            FiniteStateMachineConfig finiteStateMachineConfig = finiteStateMachineConfigDic[configId];

            finiteStateMachine.FSMId = configId;

            finiteStateMachine.finiteStateMachineConfig = finiteStateMachineConfig;

            Assembly assembly = Assembly.GetExecutingAssembly();

            for (int i = 0; i < finiteStateMachineConfig.conditions.Length; i++)
            {
                finiteStateMachine.AddCondition(ParseFiniteConditionConstant(finiteStateMachineConfig.conditions[i]));
            }
            for (int i = 0; i < finiteStateMachineConfig.states.Length; i++)
            {
                for (int j = 0; j < finiteStateMachineConfig.states[i].actions.Length; j++)
                {
                    
                    FSMAction action = assembly.CreateInstance("BlueNoah.AI.FSM." + finiteStateMachineConfig.states[i].actions[j]) as FSMAction;

                    finiteStateMachine.AddAction(ParseFiniteStateConstant(finiteStateMachineConfig.states[i].stateId), action);
                }

                if (finiteStateMachineConfigDic.ContainsKey(finiteStateMachineConfig.states[i].subFSMId))
                {
                    InitFSM(finiteStateMachine.GetState(ParseFiniteStateConstant(finiteStateMachineConfig.states[i].stateId)).SubFiniteStateMachine, finiteStateMachineConfig.states[i].subFSMId);
                }

                FSMState state = finiteStateMachine.GetState(ParseFiniteStateConstant(finiteStateMachineConfig.states[i].stateId));

                state.statePosition = finiteStateMachineConfig.states[i].position;

            }
            for (int i = 0; i < finiteStateMachineConfig.transitions.Length; i++)
            {
                FSMTransition transition = new FSMTransition(finiteStateMachine);

                transition.fromState = ParseFiniteStateConstant(finiteStateMachineConfig.transitions[i].fromStateId);

                transition.toState = ParseFiniteStateConstant(finiteStateMachineConfig.transitions[i].toStateId);

                for (int j = 0; j < finiteStateMachineConfig.transitions[i].conditionIdValues.Length; j++)
                {
                    BoolVar boolVar = finiteStateMachine.GetCondition(ParseFiniteConditionConstant(finiteStateMachineConfig.transitions[i].conditionIdValues[j].conditionId));

                    transition.AddCondition(boolVar, finiteStateMachineConfig.transitions[i].conditionIdValues[j].conditionValue == 0 ? false : true);
                }

                finiteStateMachine.AddTransition(transition.fromState, transition);
            }
        }

        public static FiniteConditionConstant ParseFiniteConditionConstant(string condition)
        {
            return (FiniteConditionConstant)(Enum.Parse(typeof(FiniteConditionConstant), condition));
        }

        public static FiniteStateConstant ParseFiniteStateConstant(string state)
        {
            return (FiniteStateConstant)(Enum.Parse(typeof(FiniteStateConstant), state));
        }


    }
}

