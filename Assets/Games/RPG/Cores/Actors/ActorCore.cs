using BlueNoah.PathFinding.FixedPoint;
using BlueNoah.RPG.PathFinding;
using UnityEngine;
using BlueNoah.CSV;
using BlueNoah.Math.FixedPoint;
using System;

namespace BlueNoah.RPG
{

    public delegate void ActorAction();

    public delegate void ActorAction<T0>(T0 t);

    public class ActorCore 
    {

        public string Name;

        public FixedPointTransform transform;

        public ActorAttribute actorAttribute;

        public BattleStatus BattleStatus;

        public GStarCornerMoveAgent MoveAgent;

        public TeamId TeamId;

        public RectInt rect;

        public SkillModel RegularAttack;

        public Node GetMainNode()
        {
            return PathFindingManager.Single.Grid.GetNode(BattleStatus.MinGridPosition);
        }

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
            FixedPointVector3 position = new FixedPointVector3(mapMonster.pos_x, 0, mapMonster.pos_y);

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

            transform.layerMask = 0;

            for (int i = 0; i < layers.Length; i++)
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
                if (actorAttribute.IsDead)
                    ActorAI.ChangeCondition(FiniteConditionConstant.Death, true);
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

    public class SkillModel
    {
        public int TargetRangeNear = 5;
        public int TargetRangeFar = 7;
    }

    public enum FieldMainAttr {Normal, OutOfField, NotWalkable , Water , WalkableWater }

    public enum TeamId { PlayerOne, PlayerTwo}

    public enum BattleDir { East ,West,North,South}

    public class BattleStatus
    {
        public RectInt GridRect;
        public BattleDir Dir;
        public Vector3Int MinGridPosition;

        public Vector3 Position;
        public int MoveSpeed;
        public Vector3Int MainGridPosition;

    }
}
