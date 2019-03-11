using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    public class WalkAction : FSMAction
    {
        public override void OnAwake()
        {

        }

        public override void OnEnter()
        {
            this.actorCore.DoAction(ActionMotionConstant.STANDBY);
        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
}
