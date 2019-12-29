using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.SceneControl;
using UnityEngine;

namespace BlueNoah.AI.FSM
{
    public delegate void FSMEventAction();

    [System.Serializable]
    public class FSMAction
    {
        public GameObject GO;
        protected object mActorCoreObj;

        public object ActorCore
        {
            set
            {
                mActorCoreObj = value;
            }
        }
        public FSMState state;
        [HideInInspector]
        public FiniteStateMachine finiteStateMachine;

        public float loopDuration = 3f;

        bool mIsEnable = true;
        bool mIsExcute;
        protected FiniteStateMachine mSubFinalStateMachine;

        protected List<ExcuteInterval> mExcuteList;

        public virtual void OnAwake()
        {
            mExcuteList = new List<ExcuteInterval>();
        }

        public void OnSubFinalStateMachineUpdate()
        {
            if (mSubFinalStateMachine != null)
                mSubFinalStateMachine.OnUpdate();
        }

        public virtual void OnEnter()
        {
            mIsExcute = true;
            //Debug.Log("OnEnter:" + GO + "¥¥" + this);
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnExit()
        {
            mIsExcute = false;
            //Debug.Log("-----> OnExit:" + GO + "¥¥" + this);
        }

        protected void AddExcute(ExcuteInterval ExcuteInterval)
        {
            mExcuteList.Add(ExcuteInterval);
        }

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

        public bool IsExcute
        {
            set
            {
                mIsExcute = value;
            }
            get
            {
                return mIsExcute;
            }
        }
    }

    public class ExcuteInterval
    {
        int mInterval;

        bool mLoop;

        long mNextExcuteFrame;

        bool mExcuteable;

        FSMEventAction mOnExcute;

        public ExcuteInterval(int interval, bool loop, FSMEventAction onExcute)
        {
            mInterval = interval;
            mLoop = loop;
            mOnExcute = onExcute;
        }

        public void OnEnter()
        {
            mNextExcuteFrame = Framer.Instance.CurrentFrame + mInterval;
            mExcuteable = true;
        }

        public void OnUpdate()
        {
            if (mExcuteable && Framer.Instance.CurrentFrame > mNextExcuteFrame)
            {
                if (mOnExcute != null)
                {
                    mOnExcute();
                }
                if (mLoop)
                {
                    mNextExcuteFrame = Framer.Instance.CurrentFrame + mInterval;
                }
                else
                {
                    mExcuteable = false;
                }
            }
        }
    }
}
