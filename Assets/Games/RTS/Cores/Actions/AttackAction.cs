using BlueNoah.AI.FSM;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    public class AttackAction : FSMAction
    {

        int mNextAttackFrame;

        public override void OnAwake()
        {

        }

        public override void OnEnter()
        {
            mActorCore.DoAction(ActionMotionConstant.ATTACK);
            mNextAttackFrame = Time.frameCount + mActorCore.attackInterval;
        }

        public override void OnUpdate()
        {
            if (mNextAttackFrame <= Time.frameCount)
            {
                mNextAttackFrame = Time.frameCount + mActorCore.attackInterval;
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
                }
            }
        }

        public override void OnExit()
        {

        }
    }
}
