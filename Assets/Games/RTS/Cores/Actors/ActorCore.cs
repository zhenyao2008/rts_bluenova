/*
 *　2019.2.23 午後６時４９分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using System;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding.FixedPoint;

namespace BlueNoah.AI.RTS
{

    public delegate void ActorAction();

    public delegate void ActorAction<T0>(T0 t);

    //main class for actor logic.
    //controlled by FSM.
    public class ActorCore
    {
        public ActorAttribute actorAttribute;

        public FixedPointTransform transform;

        public ActorAction<short> onFSMAction;

        public int x;

        public int z;

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

        public void DoAction(short actionId)
        {
            if (onFSMAction != null)
            {
                onFSMAction(actionId);
            }
        }

        public void OnUpdate()
        {
            try
            {
                mActorAI.OnUpdate();
                mActorMove.OnUpdate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
