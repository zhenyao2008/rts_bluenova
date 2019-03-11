using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.View.RTS
{
    public class ActionManager
    {

        public void DoMotionAction(int actorId, short motionId)
        {
            //TODO get actor view by actorId;

            ActorViewer actorViewer = null;

            switch (motionId)
            {
                case ActionMotionConstant.STANDBY:
                    actorViewer.actorAnimation.Play("");
                    break;
            }
        }

    }
}
