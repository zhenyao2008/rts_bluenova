//using UnityEngine;
//using UnityEngine.AI;
//using System.Collections.Generic;
//using BlueNoah.AI.FSM;
//using BlueNoah.Math.FixedPoint;
//using BlueNoah.PathFinding.FixedPoint;
//using BlueNoah.Utility;
//using SWS;

//namespace BlueNoah.AI
//{
//    //Core Unit Component and data for FSM component.
//    [RequireComponent(typeof(UnitAnimation))]
//    [RequireComponent(typeof(UnitAsset))]
//    public class UnitCore : MonoBehaviour
//    {

//        protected Transform mTrans;

//        public UnitAnimation unitAnimation;

//        public UnitAsset unitAsset;

//        public UnitAttribute unitAttribute;

//        public UnitDataModel unitDataModel;

//        public NavMeshAgent navMeshAgent;

//        public FixedPointMoveAgent moveAgent;

//        public FixedPointTransform fixedPointTransform;

//        public bool hasConditionAfterMoveDone = false;

//        public FiniteConditionConstant conditionAfterMoveDone;

//        public FiniteConditionConstant conditionForMove;

//        public bool arriveMeetingArea;

//        public Transform seatTrans;
//        [HideInInspector]
//        public float defaulSpeed = 0.6f;
//        //distance per tips.
//        [HideInInspector]
//        public float runSpeed = 1.4f;

//        public Transform moveTargetTrans;

//        public Vector3 moveTargetPos;

//        public PathMoveAction onMoveDone;

//        public UnitCore meetUnit;

//        public List<Vector3> moveTargetPosList;
//        //TODO できれば、この三つ変数がspeedみたいに変わると嬉。
//        public float sitRotateDuration = 0.5f;
//        [HideInInspector]
//        public float sitMoveBackDuration = 0.5f;

//        public float sitJumpDuration = 0.5f;

//        public float charaRadius = 0.5f;

//        public PathManager pathContainer;

//        public int unitId;

//        bool mMeetable = true;

//        float mNextMeetTime;

//        //20s
//        const int MEET_INTERVAL = 20;

//        Seat mSeat;

//        protected virtual void Awake()
//        {

//            mTrans = transform;

//            sitMoveBackDuration = 1.2f;

//            mMeetable = true;

//            Init();
//        }

//        public virtual void Init()
//        {
//            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

//            unitAnimation = gameObject.GetOrAddComponent<UnitAnimation>();

//            unitAsset = gameObject.GetOrAddComponent<UnitAsset>();
//        }

//        public bool IsMeetable
//        {
//            get
//            {
//                return mMeetable && mNextMeetTime < Time.time;
//            }
//            set
//            {
//                mMeetable = value;
//            }
//        }

//        public void Meet(UnitCore unitCore)
//        {
//            //Debug.Log("Meet");
//            IsMeetable = false;
//            this.meetUnit = unitCore;
//            mNextMeetTime = Time.time + MEET_INTERVAL;
//            this.conditionForMove = FiniteConditionConstant.MoveToMeet;
//            this.conditionAfterMoveDone = FiniteConditionConstant.Meet;
//            this.hasConditionAfterMoveDone = true;
//            GetComponent<UnitBaseAI>().SetCondition(FiniteConditionConstant.Travel, false);
//            GetComponent<UnitBaseAI>().SetCondition(FiniteConditionConstant.Meet, true);
//            //GetComponent<UnitBaseAI>().SetCondition(FiniteConditionConstant.MoveToMeet,true);
//        }

//        public void MoveTo(Vector3 targetPos, PathMoveAction onMoveDone)
//        {
//            GetComponent<UnitBaseAI>().SetCondition(FiniteConditionConstant.Run, true);
//            this.moveTargetPos = targetPos;
//            this.onMoveDone = onMoveDone;
//        }

//        public Vector3 MoveTargetPos
//        {
//            get
//            {
//                if (moveTargetTrans != null)
//                {
//                    return moveTargetTrans.position;
//                }
//                return moveTargetPos;
//            }
//        }

//        public Seat Seat
//        {
//            get
//            {
//                return this.mSeat;
//            }
//            set
//            {
//                if (mSeat != null)
//                {
//                    mSeat.SitUp();
//                }

//                if (value != null)
//                    seatTrans = value.transform;
//                else
//                    seatTrans = null;
//                this.mSeat = value;
//            }
//        }
//        //Clear the booked seat.
//        public void SitUp()
//        {
//            if (Seat != null)
//            {
//                Seat.targetUnit = null;
//                Seat = null;
//            }
//        }
//    }
//}

