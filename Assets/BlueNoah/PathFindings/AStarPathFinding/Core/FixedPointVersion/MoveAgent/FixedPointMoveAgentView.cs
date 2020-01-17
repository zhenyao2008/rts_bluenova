using UnityEngine;

namespace BlueNoah.PathFinding.FixedPoint
{
    public class FixedPointMoveAgentView : MonoBehaviour
    {
        public Vector3 targetPosition;

        UnityEngine.Transform mTrans;

        void Awake()
        {
            mTrans = transform;
        }

        void Update()
        {
            mTrans.position = targetPosition;
        }
    }
}