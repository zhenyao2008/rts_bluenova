/*
 *　2019.2.23 午後６時４９分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using System;
using BlueNoah.CSV;
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

        public FixedPoint64 scanRange = 20;

        public FixedPoint64 attackRange = 10;

        public int scanInterval = 10;

        public int attackInterval = 10;

        public ActorCore targetActor;

        public FixedPointVector3 targetPos;
        //强制移动，类似星际的直接点鼠标。
        public bool isForceMove;
        //移动中可以查找敌人，类似星际的A加鼠标。
        public bool isScanMove;

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

        public ActorCore(MapMonster mapMonster)
        {
            FixedPointVector3 position = new FixedPointVector3(mapMonster.pos_x / 1000f, 0, mapMonster.pos_y / 1000f);

            FixedPointVector3 eulerAngles = new FixedPointVector3(0, mapMonster.angle_y, 0);

            int playerId = mapMonster.alignment;

            int actorTypeId = mapMonster.unit_id;

            int[] layers = mapMonster.LayerInt;

            actorAttribute = new ActorAttribute();

            actorAttribute.playerId = playerId;

            actorAttribute.actorTypeId = actorTypeId;

            actorAttribute.runSpeed = mapMonster.move_speed;

            transform = new FixedPointTransform();

            transform.position = position;

            transform.eulerAngles = eulerAngles;

            transform.forward = FixedPointQuaternion.Euler(eulerAngles) * FixedPointVector3.forward;

            transform.layerMask = 0;

            for (int i=0 ; i< layers.Length ; i++)
            {
                transform.layerMask.AddLayer((uint)layers[i]);
            }

            mFSMId = mapMonster.action;

            mActorAI = new ActorAI(this);

            mActorMove = new ActorMove(this);
        }

        public void DoAction(short actionId)
        {
            if (onFSMAction != null)
            {
                onFSMAction(actionId);
            }
        }

        public void OnDamage(FixedPoint64 damage)
        {
            if (!actorAttribute.IsDead)
            {
                actorAttribute.OnDamage(damage);
                if(actorAttribute.IsDead)
                    ActorAI.ChangeCondition(FiniteConditionConstant.Death,true);
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
