using System.Collections.Generic;
using BlueNoah.AI.RTS;
using UnityEngine;

namespace BlueNoah.AI.FSM
{
    public delegate void FSMEventAction();

    [System.Serializable]
    public class FSMAction
    {
        public GameObject GO;
        public ActorCore actorCore;
        public FSMState state;
        [HideInInspector]
        public FiniteStateMachine finiteStateMachine;

        public float loopDuration = 3f;

        bool mIsEnable = true;
        bool mIsExcute;
        protected FiniteStateMachine mSubFinalStateMachine;

        public virtual void OnAwake()
        {

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
}
