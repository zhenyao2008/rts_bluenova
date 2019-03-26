using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.AI.FSM
{

    public enum Interruption
    {
        None,
        //NextState, 
        CurrentState
    }

    public class FSMTransition
    {
        public short fromState;

        public short toState;

        public bool useUnFixedTime;

        public bool hasExitTime;

        public float exitTime;
        //
        float mNextExitTime;
        //TODO if true , transitionDuration is second ,if false, transitionDuration is percent.
        public bool fixedDuration;

        public float transitionDuration;

        float mTransitionExitTime;

        public GameObject GO;

        public List<ConditionKeyValue> conditions;

        //List<BoolVar> mConditions;

        //List<bool> mTargetFactors;

        public Interruption interruption = Interruption.None;

        public bool orderedInterruption = true;

        public bool isFinish;

        public UnityAction onExit;

        public UnityAction onUpdate;

        public UnityAction onEnter;

        FiniteStateMachine mFinalStateMachine;

        public FSMTransition(FiniteStateMachine finiteStateMachine)
        {
            Init(finiteStateMachine);
        }

        public FSMTransition(FiniteStateMachine finiteStateMachine, short condition, bool targetConditionValue, short toState)
        {
            Init(finiteStateMachine);
            AddCondition(finiteStateMachine.GetCondition(condition), targetConditionValue);
            this.toState = toState;
        }

        void Init(FiniteStateMachine finiteStateMachine)
        {
            mFinalStateMachine = finiteStateMachine;
            conditions = new List<ConditionKeyValue>();
        }

        public void AddCondition(BoolVar boolVar, bool targetValue)
        {
            conditions.Add(new ConditionKeyValue(boolVar, targetValue));
        }

        public void OnAwake()
        {
            if (!hasExitTime && (conditions == null || conditions.Count == 0))
            {
                Debug.LogWarning(string.Format("there is no condition for {0} -> {1}.", fromState.ToString(), toState.ToString()));
            }
        }

        public void OnEnter()
        {
            if (hasExitTime)
            {
                mTransitionExitTime = Time.realtimeSinceStartup + exitTime;
            }
        }

        public bool OnUpdate()
        {
            return Validate();
        }

        public virtual bool Validate()
        {
            bool exitable = true;
            if (hasExitTime)
            {
                exitable = CheckExitTime();
            }
            if (exitable)
            {
                if (conditions == null || conditions.Count == 0)
                    return false;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (conditions[i].boolVar.value != conditions[i].targetValue)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        bool CheckExitTime()
        {
            if (mNextExitTime < Time.realtimeSinceStartup)
            {
                return true;
            }
            return false;
        }

        public void OnBeginTransit()
        {
            isFinish = false;
            if (onEnter != null)
            {
                onEnter();
            }
            if (!useUnFixedTime)
            {
                mTransitionExitTime = Time.time + transitionDuration;
            }
            else
            {
                mTransitionExitTime = Time.realtimeSinceStartup + transitionDuration;
            }

            //Exit duration = 0 の場合。
            OnExitTimeCheck();
        }

        public void OnTransit()
        {
            if (onUpdate != null)
            {
                onUpdate();
            }
            OnExitTimeCheck();
        }

        void OnExitTimeCheck()
        {
            if (!useUnFixedTime)
            {
                if (mTransitionExitTime <= Time.time)
                {
                    mFinalStateMachine.EnterState(toState);
                    isFinish = true;
                }
            }
            else
            {
                if (mTransitionExitTime <= Time.realtimeSinceStartup)
                {
                    mFinalStateMachine.EnterState(toState);
                    isFinish = true;
                }
            }
        }

        public float CurrentProgress
        {
            get
            {
                float currentProgressValue = 0;
                if (transitionDuration > 0 && mTransitionExitTime > Time.time)
                {
                    if (!useUnFixedTime)
                    {
                        currentProgressValue = Mathf.Max(mTransitionExitTime - Time.time, 0) / transitionDuration;
                    }
                    else
                    {
                        currentProgressValue = Mathf.Max(mTransitionExitTime - Time.realtimeSinceStartup, 0) / transitionDuration;
                    }
                }
                return currentProgressValue;
            }
        }

        public void OnExit()
        {
            if (onExit != null)
            {
                onExit();
            }
        }

        bool mIsEnable = true;

        public bool IsEnable
        {
            get
            {
                return mIsEnable;
            }
            set
            {
                mIsEnable = value;
            }
        }
    }
}


