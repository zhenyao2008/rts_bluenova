using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;

namespace BlueNoah.RPG
{
    public class DeathAction : FSMAction
    {
        ActorCore mActorCore;

        public override void OnAwake()
        {
            mActorCore = (ActorCore)mActorCoreObj;
        }

        public override void OnEnter()
        {
            this.mActorCore.DoAction(ActionMotionConstant.DEATH);
        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
}
