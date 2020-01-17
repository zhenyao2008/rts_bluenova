using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace BlueNoah.AI.FSM
{

    public static class FiniteStateMachineLoader
    {

        static FiniteStateMachineConfig[] LoadFiniteStateMachineConfig(string configPath)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(configPath);
            Debug.Log("<color=yellow>Load FSM from:" + configPath + "</color>");
            FiniteStateMachineConfigs unitConfigs = JsonUtility.FromJson<FiniteStateMachineConfigs>(textAsset.text);
            return unitConfigs.finiteStateMachineArray;
        }

        public static FiniteStateMachineConfigs unitAIConfig;

        public static Dictionary<int, FiniteStateMachineConfig> finiteStateMachineConfigDic;

        public static Dictionary<int, FiniteStateMachineConfig> LoadAIConfig(string fsmConfigPath)
        {
            finiteStateMachineConfigDic = new Dictionary<int, FiniteStateMachineConfig>();
            // TextAsset textAsset = Resources.Load<TextAsset>(M_FSM_CONFIG);
            // unitAIConfig = JsonUtility.FromJson<FiniteStateMachineConfigs>(textAsset.text);
            List<FiniteStateMachineConfig> fsmConfigs = new List<FiniteStateMachineConfig>();
            // fsmConfigs.AddRange(LoadFiniteStateMachineConfig("configs/fsms/j_fsm"));
            fsmConfigs.AddRange(LoadFiniteStateMachineConfig(fsmConfigPath));
            for (int i = 0; i < fsmConfigs.Count; i++)
            {
                if (!finiteStateMachineConfigDic.ContainsKey(fsmConfigs[i].id))
                {
                    finiteStateMachineConfigDic.Add(fsmConfigs[i].id, fsmConfigs[i]);
                }
                else
                {
                    Debug.LogError(fsmConfigs[i].id + " is existing.");
                }
            }
            return finiteStateMachineConfigDic;
        }

        public static void InitFSM(FiniteStateMachine finiteStateMachine, int configId)
        {
          
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

                    FSMAction action = assembly.CreateInstance(finiteStateMachineConfig.states[i].actions[j]) as FSMAction;

                    //obj.GetType().GetField(fieldname).SetValue(obj,value);

                    finiteStateMachine.AddAction(ParseFiniteStateConstant(finiteStateMachineConfig.states[i].stateId), action);
                }
                if (finiteStateMachineConfig.states[i].actionWithParams != null)
                {
                    for (int j = 0; j < finiteStateMachineConfig.states[i].actionWithParams.Length; j++)
                    {
                        FSMAction action = assembly.CreateInstance(finiteStateMachineConfig.states[i].actionWithParams[j].action) as FSMAction;

                        if (finiteStateMachineConfig.states[i].actionWithParams[j].stringParams != null)
                        {
                            for (int z = 0; z < finiteStateMachineConfig.states[i].actionWithParams[j].stringParams.Length; z++)
                            {
                                StringParam param = finiteStateMachineConfig.states[i].actionWithParams[j].stringParams[z];
                                action.GetType().GetField(param.paramName).SetValue(action, param.paramValue);
                            }
                        }
                        if (finiteStateMachineConfig.states[i].actionWithParams[j].intParams != null)
                        {
                            for (int z = 0; z < finiteStateMachineConfig.states[i].actionWithParams[j].intParams.Length; z++)
                            {
                                IntParam param = finiteStateMachineConfig.states[i].actionWithParams[j].intParams[z];
                                action.GetType().GetField(param.paramName).SetValue(action, param.paramValue);
                            }
                        }

                        if (finiteStateMachineConfig.states[i].actionWithParams[j].vectorArrayParams != null)
                        {
                            for (int z = 0; z < finiteStateMachineConfig.states[i].actionWithParams[j].vectorArrayParams.Length; z++)
                            {
                                VectorArrayParam param = finiteStateMachineConfig.states[i].actionWithParams[j].vectorArrayParams[z];
                                action.GetType().GetField(param.paramName).SetValue(action, param.paramValue);
                            }
                        }

                        if (finiteStateMachineConfig.states[i].actionWithParams[j].intArrayParams != null)
                        {
                            for (int z = 0; z < finiteStateMachineConfig.states[i].actionWithParams[j].intArrayParams.Length; z++)
                            {
                                IntArrayParam param = finiteStateMachineConfig.states[i].actionWithParams[j].intArrayParams[z];
                                action.GetType().GetField(param.paramName).SetValue(action, param.paramValue);
                            }
                        }

                        if (finiteStateMachineConfig.states[i].actionWithParams[j].stringArrayParams != null)
                        {
                            for (int z = 0; z < finiteStateMachineConfig.states[i].actionWithParams[j].stringArrayParams.Length; z++)
                            {
                                StringArrayParam param = finiteStateMachineConfig.states[i].actionWithParams[j].stringArrayParams[z];
                                action.GetType().GetField(param.paramName).SetValue(action, param.paramValue);
                            }
                        }
                        finiteStateMachine.AddAction(ParseFiniteStateConstant(finiteStateMachineConfig.states[i].stateId), action);
                    }
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

                transition.toState = ParseFiniteStateConstant(finiteStateMachineConfig.transitions[i].toStateId);

                for (int j = 0; j < finiteStateMachineConfig.transitions[i].conditionIdValues.Length; j++)
                {
                    BoolVar boolVar = finiteStateMachine.GetCondition(ParseFiniteConditionConstant(finiteStateMachineConfig.transitions[i].conditionIdValues[j].conditionId));

                    transition.AddCondition(boolVar, finiteStateMachineConfig.transitions[i].conditionIdValues[j].conditionValue == 0 ? false : true);
                }

                if (!string.IsNullOrEmpty(finiteStateMachineConfig.transitions[i].fromStateId))
                {
                    transition.fromState = ParseFiniteStateConstant(finiteStateMachineConfig.transitions[i].fromStateId);

                    finiteStateMachine.AddTransition(transition.fromState, transition);
                }
                else
                {
                    finiteStateMachine.AddCommonTransition(transition);
                }
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

