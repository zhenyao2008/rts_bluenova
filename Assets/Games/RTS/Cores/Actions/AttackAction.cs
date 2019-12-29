using BlueNoah.AI.FSM;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    public class AttackAction : FSMAction
    {

        int mNextAttackFrame;

        ActorCore mActorCore;

        public override void OnAwake()
        {
            mActorCore = (ActorCore)mActorCoreObj;
        }

        public override void OnEnter()
        {
            Debug.Log("AttackAction");
            mActorCore.ActorMove.FixedPointMoveAgent.Stop();
            mActorCore.DoAction(ActionMotionConstant.STANDBY);
            mNextAttackFrame = Time.frameCount + mActorCore.attackInterval;
        }

        public override void OnUpdate()
        {
            if (mNextAttackFrame <= Time.frameCount)
            {
                mNextAttackFrame = Time.frameCount + mActorCore.attackInterval;
                mActorCore.DoAction(ActionMotionConstant.ATTACK);
                if (mActorCore.targetActor!=null)
                {
                    mActorCore.targetActor.OnDamage(mActorCore.actorAttribute.currentAttack);

                    if (mActorCore.targetActor.actorAttribute.IsDead)
                    {
                        mActorCore.targetActor = null;
                    }
                }
                if (mActorCore.targetActor == null)
                {
                    this.finiteStateMachine.SetCondition(FiniteConditionConstant.Attack, false);
                    this.finiteStateMachine.SetCondition(FiniteConditionConstant.Run, false);
                }
            }
        }

        public override void OnExit()
        {

        }
    }
}
