using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.RPG
{
    //主に有限オートマトン　と　ビヘイビアツリー
    //今は有限オートマトン実装していた
    public class ActorAI
    {

        ActorCore mActorCore;

        FiniteStateMachine mFiniteStateMachine;

        public ActorAI(ActorCore actorCore)
        {
            mActorCore = actorCore;
            //mFiniteStateMachine = new FiniteStateMachine(actorCore);
            FiniteStateMachineLoader.InitFSM(mFiniteStateMachine, actorCore.FSMId);
            mFiniteStateMachine.SetDefaultState(FiniteStateConstant.StandBy);
            mFiniteStateMachine.EnterDefaultState();
        }

        public void OnUpdate()
        {
            mFiniteStateMachine.OnUpdate();
        }

        //Control from outside
        public void MoveTo(ActorCore targetActor, FixedPointVector3 targetPos, bool isForceMove, bool isScanMove)
        {
            mActorCore.targetActor = targetActor;
            mActorCore.targetPos = targetPos;
            mActorCore.isForceMove = isForceMove;
            mActorCore.isScanMove = isScanMove;
            mFiniteStateMachine.SetCondition(FiniteConditionConstant.Run, true);
            if (isForceMove)
            {
                mActorCore.ActorMove.MoveTo(mActorCore.targetPos, () => {
                    Debug.Log("ActionMotionConstant Done");
                    this.mFiniteStateMachine.SetCondition(FiniteConditionConstant.Run, false);
                    mActorCore.isForceMove = false;
                });
            }
        }

        public void ChangeCondition(FiniteConditionConstant condition,bool value)
        {
            mFiniteStateMachine.SetCondition(condition, value);
        }
    }
}
