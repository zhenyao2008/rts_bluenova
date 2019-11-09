using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;
using BlueNoah.Math.FixedPoint;
using BlueNoah.SceneControl;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    //射线检测原理
    //TODO 需要让范围检查更加优化，而不是遍历。
    public class ScanAction : FSMAction
    {

        int mNextScanFrame;

        public override void OnAwake()
        {

        }

        public override void OnEnter()
        {
            mNextScanFrame = Time.frameCount + mActorCore.scanInterval;
        }

        public override void OnUpdate()
        {
            if (mNextScanFrame <= Time.frameCount)
            {
                //if(mActorCore.targetActor==null)
                    //Scan();
                mNextScanFrame = Time.frameCount + mActorCore.scanInterval;
            }
        }

        public override void OnExit()
        {

        }

        void Scan()
        {
            ActorCore nearestTargetActor = ScanUtility.Scan(mActorCore);
            if (nearestTargetActor != null)
            {
                if (ScanUtility.IsInAttackRange(mActorCore, nearestTargetActor))
                {
                    //Attack
                    Debug.Log("Find:" + nearestTargetActor.actorAttribute.actorId);
                    mActorCore.targetActor = nearestTargetActor;
                    finiteStateMachine.SetCondition(FiniteConditionConstant.Attack, true);
                }
                else if (ScanUtility.IsInScanRange(mActorCore, nearestTargetActor))
                {
                    Debug.Log("Find:"+ nearestTargetActor.actorAttribute.actorId);
                    //Move
                    mActorCore.ActorAI.MoveTo(nearestTargetActor, FixedPointVector3.zero, false, false);
                }
            }
        }
    }
}
