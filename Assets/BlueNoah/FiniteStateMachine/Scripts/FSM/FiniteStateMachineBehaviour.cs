using UnityEngine;

namespace BlueNoah.AI.FSM
{
    //策略模式だけ
    public class FiniteStateMachineBehaviour : MonoBehaviour
    {
        [SerializeField]
        FiniteStateMachine mFinalStateMachine;

        public FiniteStateMachine MainFiniteStateMachine{
            get{
                if(mFinalStateMachine==null){
                    mFinalStateMachine = new FiniteStateMachine(gameObject);
                    mFinalStateMachine.isActive = true;
                }
                return mFinalStateMachine;
            }
            set{
                mFinalStateMachine = value;
            }
        }

		void Update()
		{
            mFinalStateMachine.OnUpdate();
		}
	}
}