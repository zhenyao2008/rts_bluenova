/*
 *　2019.2.23 午後６時４９分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using BlueNoah.RTS.Constant;

namespace BlueNoah.AI.RTS
{
    //main class for actor logic.
    //controlled by FSM.
    public class ActorCore
    {
        public ActorAttribute actorAttribute;

        public FixedPointMoveAgent fixedPointMoveAgent;

        public FixedPointTransform transform;

        public ActorCore(int playerId, int actorTypeId, FixedPointVector3 position, FixedPointVector3 eulerAngles)
        {
            actorAttribute = new ActorAttribute();

            actorAttribute.playerId = playerId;

            actorAttribute.actorTypeId = actorTypeId;

            transform = new FixedPointTransform();

            transform.position = position;

            transform.eulerAngles = eulerAngles;
            //TODO Create move agent.
            fixedPointMoveAgent = PathFindingMananger.Single.CreateMoveAgent(transform,GameConstant.ACTOR_SPEED);

        }

        public void MoveTo(FixedPointVector3 fixedPointVector3)
        {
            fixedPointMoveAgent.SetDestination(fixedPointVector3);
        }
    }
}
