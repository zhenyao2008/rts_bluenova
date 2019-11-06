using BlueNoah.AI.FSM;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    public class RunAction : FSMAction
    {

        int mScanInterval = 10;

        int mNextScanFrame;

        public override void OnAwake()
        {

        }

        public override void OnEnter()
        {
            mActorCore.DoAction(ActionMotionConstant.RUN);
            if (mActorCore.isForceMove)
            {
                mActorCore.ActorMove.MoveTo(mActorCore.targetActor.transform.position,()=> {
                    this.finiteStateMachine.SetCondition(FiniteConditionConstant.Run,false);
                });
            }
            else
            {
                if (mActorCore.targetActor != null)
                {
                    mActorCore.MoveTo(mActorCore.targetActor.transform.position, () => {
                        this.finiteStateMachine.SetCondition(FiniteConditionConstant.Run, false);
                    });
                }
            }
            mNextScanFrame = Time.frameCount + mScanInterval;
        }

        public override void OnUpdate()
        {
            if (mNextScanFrame <= Time.frameCount)
            {
                mNextScanFrame = Time.frameCount + mScanInterval;
                if (mActorCore.isScanMove)
                {
                    ActorCore nearestTargetActor = ScanUtility.Scan(mActorCore);

                    FixedPoint64 minDistance = (nearestTargetActor.transform.position - mActorCore.transform.position).sqrMagnitude;

                    if (minDistance <= mActorCore.attackRange * mActorCore.attackRange)
                    {
                        //Attack
                        mActorCore.targetActor = nearestTargetActor;
                        finiteStateMachine.SetCondition(FiniteConditionConstant.Attack, true);
                        mActorCore.isScanMove = false;
                    }
                    else if (minDistance <= mActorCore.scanRange * mActorCore.scanRange)
                    {
                        //Move
                        mActorCore.targetActor = nearestTargetActor;
                        mActorCore.MoveTo(mActorCore.targetActor.transform.position, () => {
                            this.finiteStateMachine.SetCondition(FiniteConditionConstant.Run, false);
                        });
                        mActorCore.isScanMove = false;
                    }
                }
                else if (mActorCore.targetActor != null)
                {
                    mActorCore.MoveTo(mActorCore.targetActor.transform.position, () => {
                        this.finiteStateMachine.SetCondition(FiniteConditionConstant.Run, false);
                    });
                }
            }
        }

        public override void OnExit()
        {

        }
    }
}
