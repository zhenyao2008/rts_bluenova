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
