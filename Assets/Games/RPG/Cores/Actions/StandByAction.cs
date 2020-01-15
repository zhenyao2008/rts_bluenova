using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.RPG
{
    public class StandByAction : FSMAction
    {
        int mNextScanFrame;
        int mScanInterval = 10;
        ActorCore mActorCore;
        public override void OnAwake()
        {
            mActorCore = (ActorCore)mActorCoreObj;
        }

        public override void OnEnter()
        {
            this.mActorCore.DoAction(ActionMotionConstant.STANDBY);
            mNextScanFrame = Time.frameCount + mScanInterval;
        }

        public override void OnUpdate()
        {
            if (mNextScanFrame <= Time.frameCount)
            {
                mNextScanFrame = Time.frameCount + mScanInterval;
                //if(mActorCore.targetActor == null)
                    //mActorCore.targetActor = ScanUtility.Scan(mActorCore);

                if (mActorCore.targetActor != null)
                {
                    FixedPoint64 minDistance = (mActorCore.targetActor.transform.position - mActorCore.transform.position).sqrMagnitude;

                    if (minDistance <= mActorCore.attackRange * mActorCore.attackRange)
                    {
                        //Attack
                        finiteStateMachine.SetCondition(FiniteConditionConstant.Attack, true);
                        mActorCore.isScanMove = false;
                    }
                    else if (minDistance <= mActorCore.scanRange * mActorCore.scanRange)
                    {
                        //Move
                        mActorCore.ActorAI.MoveTo(mActorCore.targetActor, FixedPointVector3.zero,false,false);
                        mActorCore.isScanMove = false;
                    }
                }
            }
        }

        public override void OnExit()
        {

        }

    }
}
