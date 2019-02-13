using BlueNoah.AI.FSM;

namespace BlueNoah.AI
{

    public abstract class BaseUnitAIFSMInitService
    {
        public abstract void InitFiniteStateMachine(FiniteStateMachine finiteStateMachine, int targetAIId);
    }
}

