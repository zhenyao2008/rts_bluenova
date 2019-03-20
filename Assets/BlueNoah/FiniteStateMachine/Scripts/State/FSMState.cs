using System.Collections.Generic;
using BlueNoah.AI.RTS;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.AI.FSM
{
    public class FSMState
    {
        public FiniteStateConstant state;

        public bool isLoop;

        public float length;

        public Vector3Int statePosition;

        public List<FSMAction> actions;

        public List<FSMTransition> transitions;

        FSMTransition mCurrentTransition;

        GameObject GO;
        ActorCore mActorCore;

        FiniteStateMachine mFiniteStateMachine;

        FiniteStateMachine mSubFiniteStateMachine;

        public UnityAction<FSMState> onEnter;

        public UnityAction<FSMState> onUpdate;

        public UnityAction<FSMState> onExit;

        public FSMState(GameObject gameObject, ActorCore actorCore, FiniteStateMachine finiteStateMachine, FiniteStateConstant state)
        {
            this.state = state;
            this.GO = gameObject;
            this.mActorCore = actorCore;
            this.mFiniteStateMachine = finiteStateMachine;
            this.actions = new List<FSMAction>();
            this.transitions = new List<FSMTransition>();
            InitSubFiniteStateMachine(gameObject);
        }

        void InitSubFiniteStateMachine(GameObject gameObject)
        {
            mSubFiniteStateMachine = new FiniteStateMachine(gameObject);
            mSubFiniteStateMachine.parentFSMState = this;
            mSubFiniteStateMachine.isActive = false;
        }

        public bool HasSubState()
        {
            return mSubFiniteStateMachine.stateNameList.Count > 0;
        }

        internal void OnEnter()
        {
            mCurrentTransition = null;

            //全部パラメーターをリセットする。
            for (int i = 0; i < mSubFiniteStateMachine.ConditionList.Count; i++)
            {
                BoolVar boolVar = mSubFiniteStateMachine.ConditionList[i];

                boolVar.value = false;
            }

            EnterActions();

            ValidateTransitions();

            mSubFiniteStateMachine.isActive = true;

            mSubFiniteStateMachine.EnterDefaultState();

            if (onEnter != null)
            {
                onEnter(this);
            }
        }

        internal void OnUpdate()
        {
            if (mCurrentTransition != null)
            {
                mCurrentTransition.OnTransit();
            }
            else
            {
                UpdateActions();
                mSubFiniteStateMachine.OnUpdate();
                ValidateTransitions();
            }
            if (onUpdate != null)
            {
                onUpdate(this);
            }
        }

        internal void OnExit()
        {
            ExitActions();
            mSubFiniteStateMachine.isActive = false;
            if (onExit != null)
            {
                onExit(this);
            }
        }

        public FiniteStateMachine SubFiniteStateMachine
        {
            get
            {
                return mSubFiniteStateMachine;
            }
        }

        void UpdateActions()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (!actions[i].IsEnable && actions[i].IsExcute)
                {
                    actions[i].IsExcute = false;
                    actions[i].OnExit();
                }
                if (actions[i].IsEnable)
                {
                    actions[i].OnUpdate();
                    actions[i].OnSubFinalStateMachineUpdate();
                }
            }
        }

        void EnterActions()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].IsEnable)
                {
                    actions[i].OnEnter();
                }
            }
        }

        void EnterTransitions()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].IsEnable)
                {
                    transitions[i].OnEnter();
                }
            }
        }

        void ExitActions()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].IsEnable)
                    actions[i].OnExit();
            }
        }

        void ValidateTransitions()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].IsEnable)
                {
                    if (transitions[i].OnUpdate())
                    {
                        mCurrentTransition = transitions[i];
                        OnExit();
                        mCurrentTransition.OnBeginTransit();
                        return;
                    }
                }
            }
            for (int i = 0; i < mFiniteStateMachine.CommonTransitions.Count; i++)
            {
                if (mFiniteStateMachine.CommonTransitions[i].IsEnable)
                {
                    if (mFiniteStateMachine.CommonTransitions[i].OnUpdate())
                    {
                        if (mFiniteStateMachine.CommonTransitions[i].toState != state)
                        {
                            mCurrentTransition = mFiniteStateMachine.CommonTransitions[i];
                            mCurrentTransition.fromState = state;
                            OnExit();
                            mCurrentTransition.OnBeginTransit();
                            return;
                        }
                        else
                        {
                            //優先順位に沿って
                            return;
                        }
                    }
                }
            }
        }

        internal void AddTransitions(List<FSMTransition> transitions)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                AddTransition(transitions[i]);
            }
        }

        internal void AddTransition(FSMTransition transition)
        {
            transition.fromState = state;
            transitions.Add(transition);
            transition.GO = GO;
            if (Application.isPlaying)
                transition.OnAwake();
        }

        internal void AddActions(List<FSMAction> actions)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                AddAction(actions[i]);
            }
        }

        internal void AddAction(FSMAction action)
        {
            actions.Add(action);
            action.GO = GO;
            action.ActorCore = this.mActorCore;
            action.finiteStateMachine = mFiniteStateMachine;
            if (Application.isPlaying)
                action.OnAwake();
        }
    }
}
