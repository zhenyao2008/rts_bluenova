using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.Flocking
{
    [RequireComponent(typeof(SphereCollider))]
    public class FlockComponent1 : MonoBehaviour
    {

        public float separationRadius = 2f;

        public float cohesionRadius = 5f;

        const int layer = 19;

        Collider[] colliders;
        void Start()
        {
            // mSphereCollider = GetComponent<SphereCollider>();
            // mSphereCollider.radius = 0.5f;
            gameObject.layer = layer;
        }

        void Update()
        {
            colliders = Physics.OverlapSphere(transform.position, cohesionRadius, 1 << layer);

            Vector3 separation = Vector3.zero;

            Vector3 cohesion = Vector3.zero;

            int seprationCount = 0;

            int cohesionCount = 0;

            for (int i = 0; i < colliders.Length; i++)
            {
                //Separation
                if (colliders[i].transform != transform)
                {
                    float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                    if (distance < 2)
                    {
                        // if (distance < 1)
                        // {
                        //     separation += (transform.position - colliders[i].transform.position).normalized * (2 - distance) * 20;
                        // }
                        // else
                        // {
                        separation += (transform.position - colliders[i].transform.position).normalized * (2 - distance);
                        // }
                        seprationCount++;
                    }


                    float distance1 = Vector3.Distance(transform.position, colliders[i].transform.position);

                    if (distance1 > 10)
                    {
                        separation += (transform.position - colliders[i].transform.position).normalized * (2 - distance);
                    }
                    else if (distance > 3)
                    {
                        cohesion += (transform.position - colliders[i].transform.position).normalized * (distance);
                        cohesionCount++;
                    }
                    Vector3 speed = Vector3.zero;// = separation / seprationCount - cohesion / cohesionCount;
                    if (seprationCount > 0)
                        speed += separation / seprationCount;
                    // if (cohesionCount > 0)
                    //     speed -= cohesion / cohesionCount;
                    transform.position += speed.normalized * Time.deltaTime;
                }
            }
        }
    }
}