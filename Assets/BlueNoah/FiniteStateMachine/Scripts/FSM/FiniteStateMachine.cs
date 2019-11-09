using UnityEngine;
using System.Collections.Generic;
using BlueNoah.AI.RTS;

namespace BlueNoah.AI.FSM
{
    //TODO Action.OnExit Action.OnEnter前に実行するかどうか、考えたい。（将来のロジックに基づいて変更したら）
    //Current Action --- Transition --- 
    //Actions Exit --- Transition Exit --- 
    //State Enter
    [System.Serializable]
    public class FiniteStateMachine
    {
        GameObject GO;

        ActorCore mActorCore;

        public bool isActive = false;

        public FiniteStateMachineConfig finiteStateMachineConfig;
        [SerializeField]
        FiniteStateConstant mCurrentStateName;

        FSMState mCurrentState;

        FSMState mPreState;

        FSMState mDefaultState;

        public int FSMId;

        public FSMState parentFSMState;
        //汎用のTransition,状態毎に使われる。
        List<FSMTransition> mCommonTransitions;

        public List<FSMTransition> CommonTransitions
        {
            get
            {
                if (mCommonTransitions == null)
                    mCommonTransitions = new List<FSMTransition>();
                return mCommonTransitions;
            }
        }

        public List<FiniteStateConstant> stateNameList;

        Dictionary<FiniteConditionConstant, BoolVar> mConditionDic;
        //見えるために 
        [SerializeField]
        List<BoolVar> mConditionList;

        Dictionary<FiniteStateConstant, FSMState> mStateDic;

        public BoolVar AddCondition(FiniteConditionConstant key, bool value = false)
        {
            if (!mConditionDic.ContainsKey(key))
            {
                BoolVar boolVar = new BoolVar(value);
                mConditionDic.Add(key, boolVar);
                mConditionList.Add(boolVar);
            }
            return mConditionDic[key];
        }

        public bool TryGetCondition(FiniteConditionConstant key, out BoolVar boolVar)
        {
            return mConditionDic.TryGetValue(key, out boolVar);
        }

        public BoolVar GetCondition(FiniteConditionConstant key)
        {
            BoolVar boolVar;
            if (mConditionDic.TryGetValue(key, out boolVar))
            {
                return boolVar;
            }
            return null;
        }

        public void SetCondition(FiniteConditionConstant key, bool value)
        {
            BoolVar boolVar = GetCondition(key);
            if (boolVar != null)
            {
                boolVar.value = value;
            }
        }

        public List<BoolVar> ConditionList
        {
            get
            {
                return mConditionList;
            }
        }

        public Dictionary<FiniteConditionConstant, BoolVar> ConditionDic
        {
            get
            {
                return mConditionDic;
            }
        }

        public void ResetConditionToDefault()
        {
            for (int i = 0; i < mConditionList.Count; i++)
            {
                mConditionList[i].ResetToDefault();

                if (mConditionList[i].value)
                {
                    Debug.LogError(mConditionList[i].value);
                }
            }
        }

        public FSMState GetState(FiniteStateConstant stateConstant)
        {
            FSMState state;
            if (!mStateDic.TryGetValue(stateConstant, out state))
            {
                Debug.LogError(string.Format("{0} is not exsiting.", stateConstant.ToString()));
            }
            return state;
        }

        public void SetDefaultState(FiniteStateConstant state)
        {
            mDefaultState = GetState(state);
        }

        public FSMState TryAddState(FiniteStateConstant state)
        {
            if (!mStateDic.ContainsKey(state))
            {
                mStateDic.Add(state, new FSMState(GO, mActorCore, this, state));
                stateNameList.Add(state);
            }
            return mStateDic[state];
        }

        public FiniteStateMachine(GameObject gameObject)
        {
            Init(gameObject);
        }

        void Init(GameObject gameObject)
        {
            GO = gameObject;
            stateNameList = new List<FiniteStateConstant>();
            mStateDic = new Dictionary<FiniteStateConstant, FSMState>();
            mConditionDic = new Dictionary<FiniteConditionConstant, BoolVar>();
            mConditionList = new List<BoolVar>();
        }

        public FiniteStateMachine(ActorCore actorCore)
        {
            Init(actorCore);
        }

        void Init(ActorCore actorCore)
        {
            this.mActorCore = actorCore;
            stateNameList = new List<FiniteStateConstant>();
            mStateDic = new Dictionary<FiniteStateConstant, FSMState>();
            mConditionDic = new Dictionary<FiniteConditionConstant, BoolVar>();
            mConditionList = new List<BoolVar>();
        }

        public void AddState(FiniteStateConstant state, List<FSMAction> actions, List<FSMTransition> transitions)
        {
            if (actions != null && actions.Count > 0)
            {
                AddActions(state, actions);
            }
            if (transitions != null && transitions.Count > 0)
            {
                AddTransitions(state, transitions);
            }
        }

        public void AddActions(FiniteStateConstant state, List<FSMAction> actions)
        {
            if (actions == null)
            {
                Debug.LogError(state + " is Null.");
                return;
            }
            FSMState finalState = TryAddState(state);
            finalState.AddActions(actions);
        }

        public void AddAction(FiniteStateConstant state, FSMAction action)
        {
            if (action == null)
            {
                Debug.LogError(state + " is Null.");
                return;
            }
            FSMState finalState = TryAddState(state);
            finalState.AddAction(action);
        }

        public void AddTransitions(FiniteStateConstant state, List<FSMTransition> transitions)
        {
            if (transitions == null)
            {
                Debug.LogError(state + " is Null.");
                return;
            }
            FSMState finalState = TryAddState(state);
            finalState.AddTransitions(transitions);
        }

        public void AddTransition(FiniteStateConstant state, FSMTransition transition)
        {
            if (transition == null)
            {
                Debug.LogError(string.Format("transition can't be null."));
            }
            else if (state == transition.toState)
            {
                Debug.LogError(string.Format("StartStateId:{0} == targetState:{1}", state, transition.fromState));
            }
            else
            {
                transition.fromState = state;
                FSMState finalState = TryAddState(state);
                finalState.AddTransition(transition);
            }
        }

        public void AddCommonTransition(FSMTransition transition)
        {
            CommonTransitions.Add(transition);
        }

        public void EnterDefaultState()
        {
            if (stateNameList.Count > 0)
            {
                EnterState(stateNameList[0]);
            }
        }

        public void EnterState(FiniteStateConstant state)
        {
            if (mStateDic.ContainsKey(state))
            {

                mCurrentStateName = state;

                if (mPreState != null)
                {
                    mPreState = mCurrentState;
                }

                mCurrentState = mStateDic[state];

                mCurrentState.OnEnter();

            }
            else
            {
                Debug.LogError(string.Format("EnterState:{0} is not ", state));
            }
        }

        public void OnUpdate()
        {
            if (mCurrentState != null)
            {
                mCurrentState.OnUpdate();
            }
            //else
            //{
            //    if (stateNameList != null && stateNameList.Count > 0)
            //    {
            //        EnterDefaultState();
            //    }
            //}
        }

        public FiniteStateConstant CurrentState
        {
            get
            {
                return mCurrentStateName;
            }
        }

    }
}
