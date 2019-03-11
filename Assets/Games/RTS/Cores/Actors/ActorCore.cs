/*
 *　2019.2.23 午後６時４９分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using BlueNoah.AI.FSM;
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

        public FixedPointTransform transform;

        ActorAI mActorAI;
        ActorMove mActorMove;

        int mFSMId;

        public int FSMId
        {
            get
            {
                return mFSMId;
            }
        }

        public ActorAI ActorAI
        {
            get
            {
                return mActorAI;
            }
        }

        public ActorMove ActorMove
        {
            get
            {
                return mActorMove;
            }
        }

        public ActorCore(int playerId, int actorTypeId, int FSMId, FixedPointVector3 position, FixedPointVector3 eulerAngles)
        {
            actorAttribute = new ActorAttribute();

            actorAttribute.playerId = playerId;

            actorAttribute.actorTypeId = actorTypeId;

            transform = new FixedPointTransform();

            transform.position = position;

            transform.eulerAngles = eulerAngles;

            mFSMId = FSMId;

            mActorAI = new ActorAI(this);

            mActorMove = new ActorMove(this);

        }

        public void MoveTo(FixedPointVector3 fixedPointVector3, PathMoveAction onComplete)
        {
            mActorMove.MoveTo(fixedPointVector3, onComplete);
        }

        public void OnUpdate()
        {
            mActorAI.OnUpdate();
            mActorMove.OnUpdate();
        }
    }
}
