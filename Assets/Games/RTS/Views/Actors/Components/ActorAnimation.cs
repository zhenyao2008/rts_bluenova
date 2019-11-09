using BlueNoah.RTS.Constant;
using TD.Config;
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

        public void Play(string stateName, float speed = 1,bool isLoop = true,bool isNormalSpeed = false)
        {
            if (isNormalSpeed)
                mAnimation[stateName].speed = 1;
            else
                mAnimation[stateName].speed = mAnimation[stateName].length / speed;
            if(isLoop)
                mAnimation.wrapMode = WrapMode.Loop;
            else
                mAnimation.wrapMode = WrapMode.Once; ;
            mAnimation.Stop();
            mAnimation.Play(stateName, PlayMode.StopAll);
        }

        public void Run()
        {
            mAnimation["Run01"].speed = InGameConfig.Single.actorSpeed / 100f;
            mAnimation.Play("Run01");
            mAnimation.wrapMode = WrapMode.Loop;
        }

        public void Idle()
        {
            mAnimation.Play("Idle01");
        }
    }
}
