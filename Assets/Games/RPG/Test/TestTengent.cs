using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG
{

    public class TestTengent : MonoBehaviour {

        public Transform circle;

        public Transform point;

        public float radius;


        private void OnDrawGizmos()
        {
            if (circle && point)
            {
                Vector3[] interections = RVOUtility.CalculateTangent(circle.position,radius,point.position);
                if (interections != null)
                {
                    Gizmos.DrawWireSphere(circle.position,radius);
                    Gizmos.DrawWireSphere(point.position,0.5f);
                    Gizmos.DrawWireSphere(interections[0], 0.5f);
                    Gizmos.DrawLine(point.position,interections[0] );
                    Gizmos.DrawWireSphere(interections[1], 0.5f);
                    Gizmos.DrawLine(point.position,interections[1] );
                }
            }
        }

    }
}

