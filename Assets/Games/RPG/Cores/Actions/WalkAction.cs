using BlueNoah.AI.FSM;

namespace BlueNoah.RPG
{
    public class WalkAction : FSMAction
    {
        ActorCore mActorCore;

        public override void OnAwake()
        {
            mActorCore = (ActorCore)mActorCoreObj;
        }

        public override void OnEnter()
        {
            this.mActorCore.DoAction(ActionMotionConstant.STANDBY);
        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
}
