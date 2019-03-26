using UnityEngine;
using System.Collections.Generic;
using BlueNoah.AI.RTS;
using System.Collections;

namespace BlueNoah.AI.FSM
{
    //TODO Action.OnExit Action.OnEnter前に実行するかどうか、考えたい。（将来のロジックに基づいて変更したら）
    //Current Action --- Transition --- 
    //Actions Exit --- Transition Exit --- 
    //State Enter

    //TODO Condition不用enum的理由，enum不能运行时动态添加，所以用short类型能更加灵活，可以通过json来动态增加状态机
    //TODO 同理Status也不用enum，用short，状态是不大可能超过。
    //TODO 缺点配置大时候condition参数名和status参数名为short，不可视，这需要用Editor来实现。
    [System.Serializable]
    public class FiniteStateMachine
    {
        GameObject GO;

        ActorCore mActorCore;

        public bool isActive = false;

        public FiniteStateMachineConfig finiteStateMachineConfig;

        short mCurrentStateId;

        FSMState mCurrentState;

        FSMState mPreState;

        FSMState mDefaultState;

        public int FSMId;

        public FSMState parentFSMState;
        //汎用のTransition,状態毎に使われる。
        List<FSMTransition> mCommonTransitions;

        Hashtable mParameters;

        public List<FSMTransition> CommonTransitions
        {
            get
            {
                if (mCommonTransitions == null)
                    mCommonTransitions = new List<FSMTransition>();
                return mCommonTransitions;
            }
        }

        public List<short> stateList;

        Dictionary<short, BoolVar> mConditionDic;
        //見えるために 
        [SerializeField]
        List<BoolVar> mConditionList;

        Dictionary<short, FSMState> mStateDic;

        public object GetParameter(object key)
        {
            if (mParameters.ContainsKey(key))
            {
                return mParameters[key];
            }
            return null;
        }

        public void SetParameter(object key, object value)
        {
            if (!mParameters.ContainsKey(key))
            {
                mParameters.Add(key, value);
            }
            else
            {
                mParameters[key] = value;
            }
        }

        public void RemoveParameter(object key)
        {
            if (mParameters.ContainsKey(key))
            {
                mParameters.Remove(key);
            }
        }

        public BoolVar AddCondition(short key, bool value = false)
        {
            if (!mConditionDic.ContainsKey(key))
            {
                BoolVar boolVar = new BoolVar(value);
                mConditionDic.Add(key, boolVar);
                mConditionList.Add(boolVar);
            }
            return mConditionDic[key];
        }

        public bool TryGetCondition(short key, out BoolVar boolVar)
        {
            return mConditionDic.TryGetValue(key, out boolVar);
        }

        public BoolVar GetCondition(short key)
        {
            BoolVar boolVar;
            if (mConditionDic.TryGetValue(key, out boolVar))
            {
                return boolVar;
            }
            return null;
        }

        public void SetCondition(short key, bool value)
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

        public Dictionary<short, BoolVar> ConditionDic
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

        public FSMState GetState(short stateConstant)
        {
            FSMState state;
            if (!mStateDic.TryGetValue(stateConstant, out state))
            {
                Debug.LogError(string.Format("{0} is not exsiting.", stateConstant.ToString()));
            }
            return state;
        }

        public void SetDefaultState(short state)
        {
            mDefaultState = GetState(state);
        }

        public FSMState TryAddState(short state)
        {
            if (!mStateDic.ContainsKey(state))
            {
                mStateDic.Add(state, new FSMState(GO, mActorCore, this, state));
                stateList.Add(state);
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
            mParameters = new Hashtable();
            stateList = new List<short>();
            mStateDic = new Dictionary<short, FSMState>();
            mConditionDic = new Dictionary<short, BoolVar>();
            mConditionList = new List<BoolVar>();
        }

        public FiniteStateMachine(ActorCore actorCore)
        {
            Init(actorCore);
        }

        void Init(ActorCore actorCore)
        {
            this.mActorCore = actorCore;
            stateList = new List<short>();
            mStateDic = new Dictionary<short, FSMState>();
            mConditionDic = new Dictionary<short, BoolVar>();
            mConditionList = new List<BoolVar>();
        }

        public void AddState(short state, List<FSMAction> actions, List<FSMTransition> transitions)
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

        public void AddActions(short state, List<FSMAction> actions)
        {
            if (actions == null)
            {
                Debug.LogError(state + " is Null.");
                return;
            }
            FSMState finalState = TryAddState(state);
            finalState.AddActions(actions);
        }

        public void AddAction(short state, FSMAction action)
        {
            if (action == null)
            {
                Debug.LogError(state + " is Null.");
                return;
            }
            FSMState finalState = TryAddState(state);
            finalState.AddAction(action);
        }

        public void AddTransitions(short state, List<FSMTransition> transitions)
        {
            if (transitions == null)
            {
                Debug.LogError(state + " is Null.");
                return;
            }
            FSMState finalState = TryAddState(state);
            finalState.AddTransitions(transitions);
        }

        public void AddTransition(short state, FSMTransition transition)
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
            if (stateList.Count > 0)
            {
                EnterState(stateList[0]);
            }
        }

        public void EnterState(short state)
        {
            if (mStateDic.ContainsKey(state))
            {

                mCurrentStateId = state;

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

        public short CurrentState
        {
            get
            {
                return mCurrentStateId;
            }
        }

    }
}
