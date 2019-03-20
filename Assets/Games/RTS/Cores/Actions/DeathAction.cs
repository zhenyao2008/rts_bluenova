﻿using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;

namespace BlueNoah.AI.RTS
{
    public class DeathAction : FSMAction
    {
        public override void OnAwake()
        {

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
