using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.FSM;
using BlueNoah.Math.FixedPoint;
using BlueNoah.SceneControl;
using UnityEngine;

namespace BlueNoah.AI.RTS
{
    //射线检测原理
    //TODO 需要让范围检查更加优化，而不是遍历。
    public class ScanAction : FSMAction
    {

        FixedPoint64 mScanRange = 20;

        int mScanInterval = 10;

        int mNextScanFrame;

        public override void OnAwake()
        {

        }

        public override void OnEnter()
        {
            mNextScanFrame = Time.frameCount + mScanInterval;
        }

        public override void OnUpdate()
        {
            if (mNextScanFrame <= Time.frameCount)
            {
                ActorCore actorCore = Scan();
                if (actorCore != null)
                {

                }
                mNextScanFrame = Time.frameCount + mScanInterval;
            }
        }

        public override void OnExit()
        {

        }

        ActorCore Scan()
        {
            int playerId = mActorCore.actorAttribute.playerId;
            List<ActorCore> targetActors = SceneCore.Instance.GetActors(playerId);
            if (playerId == 0)
            {
                targetActors = SceneCore.Instance.GetActors(1);
            }
            else
            {
                targetActors = SceneCore.Instance.GetActors(0);
            }
            for (int i=0;i<targetActors.Count;i++)
            {
                FixedPointVector3 distance = mActorCore.transform.position - targetActors[i].transform.position;
                if(distance.sqrMagnitude <= mScanRange * mScanRange)
                {
                    return targetActors[i];
                }
            }
            return null;
        }

    }
}
