﻿using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    public class AttackAction : FSMAction
    {

        public override void OnAwake()
        {

        }

        public override void OnEnter()
        {
            this.mActorCore.DoAction(ActionMotionConstant.ATTACK);
        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }

    }
}
