using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.Constant;
using BlueNoah.AI.FSM;
using BlueNoah.Math.FixedPoint;
using BlueNoah.SceneControl;

namespace BlueNoah.AI.RTS
{
    //射线检测原理
    //TODO 需要让范围检查更加优化，而不是遍历。
    public class ScanAction : FSMAction
    {

        public FixedPoint64 scaneRadius;

        ExcuteInterval mScanInterval;

        public override void OnAwake()
        {
            finiteStateMachine.SetParameter(ActorActionParameterConstant.PARAM_ATTACK_TARGET, null);
            mScanInterval = new ExcuteInterval(ActorConstant.SCANE_INTERVAL, ActorConstant.SCANE_LOOP, Scan);
        }

        public override void OnEnter()
        {
            mScanInterval.OnEnter();
        }

        public override void OnUpdate()
        {
            mScanInterval.OnUpdate();
        }

        public override void OnExit()
        {

        }

        void Scan()
        {
            List<ActorCore> actorCores = AreaService.Instance.ScanActors(this.mActorCore.transform.position, ActorConstant.scanRadius);
            ActorCore actorCore = null;
            for (int i = 0; i < actorCores.Count; i++)
            {
                if (actorCores[i] != this.mActorCore && actorCores[i].actorAttribute.alignmentId != this.mActorCore.actorAttribute.alignmentId)
                {
                    actorCore = actorCores[i];
                    finiteStateMachine.SetParameter(ActorActionParameterConstant.PARAM_ATTACK_TARGET, actorCore);
                    finiteStateMachine.SetCondition((int)ActorFSMConditionConstant.RUN, true);
                }
            }
        }

    }
}
