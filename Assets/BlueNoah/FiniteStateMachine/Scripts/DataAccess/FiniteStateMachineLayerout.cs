using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.FSM
{
    [System.Serializable]
    public class FiniteStateMachineLayerout
    {
        public List<FSMStateLayerout> stateLayers;

        public FiniteStateMachineLayerout()
        {
            stateLayers = new List<FSMStateLayerout>();
        }

    }
    [System.Serializable]
    public class FSMStateLayerout
    {
        public string state;

        public Vector2 position;

        public FiniteStateMachineLayerout subFiniteStateMachineLayerout;

        public FSMStateLayerout(string state, Vector2 position)
        {
            this.state = state;
            this.position = position;
        }
    }
}
