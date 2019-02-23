using UnityEngine;

namespace BlueNoah.AI.View.RTS
{

    public class ActorAnimation : MonoBehaviour
    {

        //Animator mAnimator;

        Animation mAnimation;

        //NavMeshAgent mNavMeshAgent;

        //AnimatorOverrideController mAnimatorOverrideController;

        //List<StateBehaviour> mStateCountBehaviours;

        //Dictionary<string, StateBehaviour> mStateBehaviourDic;

        const float DEFAULT_SPEED = 1;

        //UnitMotionLoadService mUnitMotionLoadService;

        void Awake()
        {
            //mStateCountBehaviours = new List<StateBehaviour>();
            //mNavMeshAgent = GetComponent<NavMeshAgent>();
            //Idle();
            //if (mStateBehaviourDic == null)
            //{
            //    mStateBehaviourDic = new Dictionary<string, StateBehaviour>();
            //}
            //mUnitMotionLoadService = new UnitMotionLoadService(UnitAnimator);
        }

        Animation GetAnimation
        {
            get
            {
                return mAnimation;
            }
        }

        //Animator 
        //{
        //    get
        //    {
        //        if (mAnimator == null)
        //            mAnimator = GetComponentInChildren<Animator>();
        //        return mAnimator;
        //    }
        //}

        //public void AddStateBehaviour(StateBehaviour stateBehaviour)
        //{
        //    if (!mStateBehaviourDic.ContainsKey(stateBehaviour.stateName))
        //    {
        //        mStateBehaviourDic.Add(stateBehaviour.stateName, stateBehaviour);
        //    }
        //}

        //public AnimatorStateInfo GetCurentStateInfo()
        //{
        //    AnimatorStateInfo animatorStateInfo = UnitAnimator.GetCurrentAnimatorStateInfo(0);
        //    return animatorStateInfo;
        //}

        //public AnimatorTransitionInfo GetCurrentTransitionInfo()
        //{
        //    AnimatorTransitionInfo animatorTransitionInfo = UnitAnimator.GetAnimatorTransitionInfo(0);
        //    return animatorTransitionInfo;
        //}

        //public void OnStateComplete(string stateName)
        //{
        //    Debug.Log(stateName);
        //}

        //public void ChangeToEditMode()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_GROUND, false);
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FITTING_01, false);
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_CHAIR, false);
        //    UnitAnimator.SetBool(AnimatorConditionConstant.WALK, false);
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RUN, false);
        //}

        //public void ResetParameters()
        //{
        //    AnimatorControllerParameter[] parameters = UnitAnimator.parameters;
        //    for (int i = 0; i < parameters.Length; i++)
        //    {
        //        UnitAnimator.SetBool(parameters[i].name, false);
        //    }
        //}

        //public void SetBool(string condition, bool value)
        //{
        //    UnitAnimator.SetBool(condition, value);
        //}

        //public void PlayState(string stateName)
        //{
        //    ResetParameters();
        //    UnitAnimator.Play(stateName);
        //}

        //public void Play(string motionCondition)
        //{
        //    ResetParameters();
        //    UnitAnimator.SetBool(motionCondition, true);
        //}

        //public void Play(string motionCondition, int loopCount)
        //{
        //    GDebug.Important.Log("Play " + motionCondition + " with loopCount: " + loopCount);
        //    ResetParameters();

        //    UnitAnimator.SetBool(motionCondition, true);
        //    //Debug.Log(montionCondition);
        //    StartCoroutine(_WaitForEnterBehaviour(motionCondition, loopCount));
        //}
        ////最初stateはDictionaryに入っていない。
        //IEnumerator _WaitForEnterBehaviour(string montionCondition, int loopCount)
        //{
        //    Debug.Log("_WaitForEnterBehaviour");
        //    bool entered = false;
        //    StateBehaviour stateBehaviour;
        //    while (!entered)
        //    {
        //        if (mStateBehaviourDic.TryGetValue(montionCondition, out stateBehaviour))
        //        {
        //            stateBehaviour.maxCount = loopCount;
        //            stateBehaviour.onComplete = () =>
        //            {
        //                Debug.Log("StateBehaviour");
        //            };
        //            entered = true;
        //        }
        //        //Debug.Log(mStateBehaviourDic.Count);
        //        yield return null;
        //    }
        //}

        //public void Run(float speed = DEFAULT_SPEED)
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RUN, true);
        //    UnitAnimator.speed = speed;
        //}

        //public void UnRun()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RUN, false);
        //    UnitAnimator.speed = DEFAULT_SPEED;
        //}

        //public void StopWalk()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.WALK, false);
        //    UnitAnimator.speed = DEFAULT_SPEED;
        //}

        //public void Walk(float speed = DEFAULT_SPEED)
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_GROUND, false);
        //    UnitAnimator.SetBool(AnimatorConditionConstant.WALK, true);
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RUN, false);
        //    UnitAnimator.speed = speed;
        //}

        //public void SitDownToGround()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_GROUND, true);
        //}

        //public void SitUpFromGround()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_GROUND, false);
        //}

        //public void SitDownToChair()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_CHAIR, true);
        //}

        //public void SitUpFromChair()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_CHAIR, false);
        //}

        //public void IdleImmediately()
        //{
        //    UnitAnimator.Play(AnimatorConditionConstant.IDLE);
        //}

        ////念のため、ブールパラメーター全てフォーラスに設定する
        //public void Idle()
        //{
        //    ResetParameters();
        //}

        //void StandUp()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_GROUND, false);
        //}

        //public void Fitting01()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FITTING_01, true);
        //}

        //public void UnFitting01()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FITTING_01, false);
        //}

        //public void Fitting02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FITTING_02, true);
        //}

        //public void UnFitting02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FITTING_02, false);
        //}

        //public void Delight()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.DELIGHT, true);
        //}

        //public void UnDelight()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.DELIGHT, false);
        //}

        //public void Angry()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.ANGRY, true);
        //}

        //public void UnAngry()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.ANGRY, false);
        //}

        //public void FeelDown()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FEEL_DOWN, true);
        //}

        //public void UnFeelDown()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FEEL_DOWN, false);
        //}

        //public void Pinch()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.PINCHED, true);
        //}

        //public void UnPinch()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.PINCHED, false);
        //}

        //public void HaveSMA()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.HAVE_SMA, true);
        //}

        //public void UnHaveSMA()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.HAVE_SMA, false);
        //}

        //public void Incline()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.INCLINE, true);
        //}

        //public void UnIncline()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.INCLINE, false);
        //}

        //public void Jump()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.JUMP, true);
        //}

        //public void UnJump()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.JUMP, false);
        //}

        //public void Secure()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SECURE, true);
        //}

        //public void UnSecure()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SECURE, false);
        //}

        //public void Release01()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE01, true);
        //}

        //public void UnRelease01()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE01, false);
        //}

        //public void Release02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE02, true);
        //}

        //public void UnRelease02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE02, false);
        //}

        //public void Release03()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE03, true);
        //}

        //public void UnRelease03()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE03, false);
        //}

        //public void Release04()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE04, true);
        //}

        //public void UnRelease04()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.RELEASE04, false);
        //}

        //public void SitEar()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_EAR, true);
        //}

        //public void UnSitEar()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_EAR, false);
        //}

        //public void ForceSitToChair()
        //{
        //    UnitAnimator.SetTrigger(AnimatorConditionConstant.FORCE_CHAIR);
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SIT_CHAIR, true);
        //}
        //public void HaveCamera()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.HAVE_CAMERA, true);
        //}
        //public void UnHaveCamera()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.HAVE_CAMERA, false);
        //}
        //public void Shooting()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SHOOTING, true);
        //}
        //public void UnShooting()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SHOOTING, false);
        //}

        //public void Idle02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.IDLE02, true);
        //}
        //public void UnIdle02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.IDLE02, false);
        //}

        //public void Angry02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.ANGRY02, true);
        //}
        //public void UnAngry02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.ANGRY02, false);
        //}
        //public void Feel_Down02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FEEL_DOWN02, true);
        //}
        //public void UnFeel_Down02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FEEL_DOWN02, false);
        //}

        //public void Pinch02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.PINCH02, true);
        //}
        //public void UnPinch02()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.PINCH02, false);
        //}

        //public void Stand_Fingers()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.STAND_FINGERS, true);
        //}
        //public void UnStand_Fingers()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.STAND_FINGERS, false);
        //}

        //public void Look_Around()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.LOOK_AROUND, true);
        //}
        //public void UnLook_Around()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.LOOK_AROUND, false);
        //}

        //public void Surprise()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SURPRISE, true);
        //}
        //public void UnSurprise()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.SURPRISE, false);
        //}

        //public void Nod()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.NOD, true);
        //}
        //public void UnNod()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.NOD, false);
        //}
        //public void Hand_Over()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.HAND_OVER, true);
        //}
        //public void UnHand_Over()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.HAND_OVER, false);
        //}

        //public void Applause()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.APPLAUSE, true);
        //}
        //public void UnApplause()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.APPLAUSE, false);
        //}

        //public void Wave_Hands()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.WAVE_HANDS, true);
        //}
        //public void UnWave_Hands()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.WAVE_HANDS, false);
        //}

        //public void Eat()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.EAT, true);
        //}

        //public void UnEat()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.EAT, false);
        //}


        ////ケロじゃん専用モーション
        //public void Fly()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FLY, true);
        //}
        ////ケロじゃん専用モーション
        //public void UnFly()
        //{
        //    UnitAnimator.SetBool(AnimatorConditionConstant.FLY, false);
        //}

        //public void ChangeFace(int faceId)
        //{
        //    GetComponent<UnitAsset>().ChangeFace(faceId);
        //}
    }

}
