using System.Collections;
using System.Collections.Generic;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using BlueNoah.RTS.Constant;
using UnityEngine;


namespace BlueNoah.AI.RTS
{
    //主に移す行為が三つある：WayPoint,Grid,Navmesh。
    //これでGridを使っている。
    public class ActorMove
    {
        FixedPointMoveAgent mFixedPointMoveAgent;

        public FixedPointMoveAgent FixedPointMoveAgent
        {
            get
            {
                return mFixedPointMoveAgent;
            }
        }

        ActorCore mActorCore;

        public ActorMove(ActorCore actorCore)
        {
            this.mActorCore = actorCore;
            mFixedPointMoveAgent = PathFindingMananger.Single.CreateMoveAgent(mActorCore.transform, GameConstant.ACTOR_SPEED);
        }

        public void MoveTo(FixedPointVector3 fixedPointVector3, PathMoveAction onComplete)
        {
            mFixedPointMoveAgent.SetDestination(fixedPointVector3, onComplete);
        }

        public void OnUpdate()
        {

        }
    }
}
