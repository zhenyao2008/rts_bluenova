using System;
using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;


namespace RVO
{
    public class RVOController : SimpleSingleMonoBehaviour<RVOController>
    {

        List<Transform> agents;

        /* Store the goals of the agents. */
        IList<Vector2> goals;

        // Start is called before the first frame update
         protected override void Awake()
        {
            base.Awake();
            goals = new List<Vector2>();
            agents = new List<Transform>();

            /* Specify the global time step of the simulation. */
            Simulator.Instance.setTimeStep(0.25f);
            /*
             * Specify the default parameters for agents that are subsequently
             * added.
             */
            Simulator.Instance.setAgentDefaults(4.0f, 10, 10.0f, 10.0f, 0.5f, 2.0f, new Vector2(0.0f, 0.0f));
        }

        private void Start()
        {
            StartRVO();
        }

        public int AddAgent(Transform trans)
        {
            if(goals == null)
            goals = new List<Vector2>();
            if(agents == null)
            agents = new List<Transform>();

            goals.Add(new Vector2(trans.position.x, trans.position.z));
            agents.Add(trans);
            return Simulator.Instance.addAgent(new Vector2(trans.position.x, trans.position.z));
        }

        public void Move(int index,Vector3 goal)
        {
            goals[index] = new Vector2(goal.x, goal.z);
        }

        void updateVisualization()
        {
            /* Output the current position of all the agents. */
            for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
            {
                // Debug.Log(Simulator.Instance.getAgentPosition(i));
                Vector2 vector2 = Simulator.Instance.getAgentPosition(i);
                agents[i].transform.position = new Vector3(vector2.x_, 0, vector2.y_);
            }
        }

        void setPreferredVelocities()
        {
            /*
             * Set the preferred velocity to be a vector of unit magnitude
             * (speed) in the direction of the goal.
             */
            for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
            {
                Vector2 goalVector = goals[i] - Simulator.Instance.getAgentPosition(i);

                if (RVOMath.absSq(goalVector) > 1.0f)
                {
                    goalVector = RVOMath.normalize(goalVector);
                }

                Simulator.Instance.setAgentPrefVelocity(i, goalVector);
            }
        }

        void StartRVO()
        {

            /* Set up the scenario. */
            //  setupScenario();

            StartCoroutine(_Move());

        }

        IEnumerator  _Move()
        {
            do
            {
                updateVisualization();
                setPreferredVelocities();
                Simulator.Instance.doStep();
                yield return null;
            }
            while (true);

        }
    }
}
