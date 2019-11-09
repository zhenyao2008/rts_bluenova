using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using TD.Config;


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
            mFixedPointMoveAgent = PathFindingMananger.Single.CreateMoveAgent(mActorCore.transform, actorCore.actorAttribute.runSpeed / 100);
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
