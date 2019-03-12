using BlueNoah.RTS.Constant;
using UnityEngine;

namespace BlueNoah.AI.View.RTS
{

    public class ActorAnimation : MonoBehaviour
    {

        Animation mAnimation;

        const float DEFAULT_SPEED = 1;

        void Awake()
        {
            mAnimation = GetComponentInChildren<Animation>();
        }

        Animation GetAnimation
        {
            get
            {
                return mAnimation;
            }
        }

        public void DoMotionAction(short actionId)
        {
            switch (actionId)
            {
                case ActionMotionConstant.STANDBY:
                    Play(AnimationConstant.STANDBY);
                    break;
                case ActionMotionConstant.WALK:
                    Play(AnimationConstant.WALK);
                    break;
                case ActionMotionConstant.RUN:
                    Play(AnimationConstant.RUN);
                    break;
                case ActionMotionConstant.ATTACK:
                    Play(AnimationConstant.ATTACK);
                    break;
                case ActionMotionConstant.DEATH:
                    Play(AnimationConstant.DEATH);
                    break;
                default:
                    Play(AnimationConstant.STANDBY);
                    break;
            }
        }

        public void Play(string stateName, float speed = 1)
        {
            mAnimation[stateName].speed = speed;
            mAnimation.Play(stateName);
        }

        public void Run()
        {
            mAnimation["Run01"].speed = GameConstant.ACTOR_SPEED / 10;
            mAnimation.Play("Run01");
            mAnimation.wrapMode = WrapMode.Loop;
        }

        public void Idle()
        {
            mAnimation.Play("Idle01");
        }
    }
}
