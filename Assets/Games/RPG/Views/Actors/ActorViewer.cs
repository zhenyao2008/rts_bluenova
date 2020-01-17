/*
 *　2019.2.23 午後７時４分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using BlueNoah.AI.RTS;
using BlueNoah.PathFinding.FixedPoint;
using UnityEngine;

namespace BlueNoah.RPG.View
{
    //Main class for actor view. 
    //Don't control the core class.
    //Controlled by actorCore.
    //For act, not calculation.
    public class ActorViewer : MonoBehaviour
    {
        public ActorCore actorCore;
        public ActorAnimation actorAnimation;
        public ActorAsset actorAsset;
        public ActorEffect actorEffect;
        public ActorHeadUI actorHeadUI;
        public Vector3 screenPosition;

        void Awake()
        {
            actorAnimation = gameObject.GetOrAddComponent<ActorAnimation>();

            actorAsset = gameObject.GetOrAddComponent<ActorAsset>();

            actorEffect = gameObject.GetOrAddComponent<ActorEffect>();
        }
        public void Init(ActorCore actorCore)
        {
#if UNITY_EDITOR
            if (actorCore == null)
            {
                Debug.LogError("UnitCore is Null.");
            }
#endif
            this.actorCore = actorCore;
            //actorCore.ActorMove.FixedPointMoveAgent.onMove = actorAnimation.Run;
            //actorCore.ActorMove.FixedPointMoveAgent.onStop = actorAnimation.Idle;
            actorCore.ActorMove.FixedPointMoveAgent.onPositionChange = (FixedPointTransform pointTransform) =>
            {
                transform.position = pointTransform.position.ToVector3();
            };
            actorCore.ActorMove.FixedPointMoveAgent.onNodeChange = (FixedPointTransform pointTransform) =>
            {
                transform.position = pointTransform.position.ToVector3();
                transform.forward = pointTransform.forward.ToVector3();
            };
            actorCore.onFSMAction = DoFSMAction;
            UpdateTransform();
        }
        void Update()
        {
            //UpdateTransform();
        }

        public void UpdateTransform()
        {
            if (actorCore != null)
            {
                transform.position = actorCore.transform.position.ToVector3();
                transform.eulerAngles = actorCore.transform.eulerAngles.ToVector3();
            }
        }
        //Selection condition.
        void LateUpdate()
        {
            if (actorCore != null)
            {
                screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            }
        }
        public bool IsInScreen()
        {
            return screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < 1 && screenPosition.y > 0 && screenPosition.y < 1;
        }

        public void DoFSMAction(short actionId)
        {
            switch (actionId)
            {
                case ActionMotionConstant.STANDBY:
                    actorAnimation.Play(AnimationConstant.STANDBY, 1, true, true);
                    break;
                case ActionMotionConstant.WALK:
                    actorAnimation.Play(AnimationConstant.WALK);
                    break;
                case ActionMotionConstant.RUN:
                    actorAnimation.Play(AnimationConstant.RUN, this.actorCore.ActorMove.FixedPointMoveAgent.Speed.AsFloat() * 2, true);
                    break;
                case ActionMotionConstant.ATTACK:
                    actorAnimation.Play(AnimationConstant.ATTACK, this.actorCore.attackInterval / 60f, false);
                    break;
                case ActionMotionConstant.DEATH:
                    actorAnimation.Play(AnimationConstant.DEATH, 1, false, true);
                    break;
                default:
                    actorAnimation.Play(AnimationConstant.STANDBY, 1, true, true);
                    break;
            }
        }
    }
}
