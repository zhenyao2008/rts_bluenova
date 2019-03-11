using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    //主に有限オートマトン　と　ビヘイビアツリー
    //今は有限オートマトン実装していた
    public class ActorAI
    {

        ActorCore mActorCore;

        FiniteStateMachine mFiniteStateMachine;

        public ActorAI(ActorCore actorCore)
        {
            this.mActorCore = actorCore;
            mFiniteStateMachine = new FiniteStateMachine(actorCore);
            FiniteStateMachineLoader.InitFSM(mFiniteStateMachine, actorCore.FSMId);
            mFiniteStateMachine.SetDefaultState(FiniteStateConstant.StandBy);
            mFiniteStateMachine.EnterDefaultState();
        }

        public void OnUpdate()
        {
            mFiniteStateMachine.OnUpdate();
        }

    }
}
