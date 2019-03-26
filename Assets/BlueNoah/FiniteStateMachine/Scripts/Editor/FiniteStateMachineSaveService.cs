using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.FSM
{
    public class FiniteStateMachineSaveService
    {

        const string FSM_PATH = "fsms/j_fsm";

        TransitionData InitTransitionData(FSMTransition transition)
        {
            TransitionData transitionData = new TransitionData();
            transitionData.fromState = transition.fromState.ToString();
            transitionData.toState = transition.toState.ToString();
            transitionData.keyValueDatas = new List<TransitionConditionData>();
            for (int i = 0; i < transition.conditions.Count; i++)
            {
                TransitionConditionData transitionConditionData = new TransitionConditionData();
                transitionConditionData.targetValue = transition.conditions[i].targetValue;
                //TODO このプロパティー他の所に置きたい。
                //transitionConditionData.condtion = transition.conditions[i].boolVar.condition.ToString();
            }
            return transitionData;
        }

        public void Save(FiniteStateMachineWindowData finiteStateMachineWindowData)
        {
            //Temp comment.
            //Debug.LogError("Save Error");
            //return;

            FiniteStateMachineConfigs finiteStateMachineConfigs = new FiniteStateMachineConfigs();

            Dictionary<int, FiniteStateMachineConfig> finitStateDic = FiniteStateMachineLoader.finiteStateMachineConfigDic;

            SaveFSM(finiteStateMachineWindowData, finitStateDic);

            string path = EditorFileManager.ResourcesPathToFilePath(FSM_PATH);

            List<FiniteStateMachineConfig> finiteStateMachineConfigList = new List<FiniteStateMachineConfig>();

            finiteStateMachineConfigList.AddRange(finitStateDic.Values);

            finiteStateMachineConfigs.finiteStateMachineArray = finiteStateMachineConfigList.ToArray();

            string fsmConfig = JsonUtility.ToJson(finiteStateMachineConfigs, true);

            FileManager.WriteString(path, fsmConfig);
        }

        void SaveFSM(FiniteStateMachineWindowData finiteStateMachineWindowData, Dictionary<int, FiniteStateMachineConfig> finitStateDic)
        {
            FiniteStateMachineConfig finiteStateMachineConfig = finiteStateMachineWindowData.finiteStateMachine.finiteStateMachineConfig;

            for (int i = 0; i < finiteStateMachineConfig.states.Length; i++)
            {

                StateConfig stateConfig = finiteStateMachineConfig.states[i];

                DragableState dragableState = finiteStateMachineWindowData.dragableStateDic[stateConfig.stateId];

                Rect rect = dragableState.rect;

                stateConfig.position = new Vector3Int((int)rect.x, (int)rect.y, 0);

                if (dragableState.subFiniteStateMachineWindowData != null)
                {
                    SaveFSM(dragableState.subFiniteStateMachineWindowData, finitStateDic);
                }
            }
        }
    }
}
