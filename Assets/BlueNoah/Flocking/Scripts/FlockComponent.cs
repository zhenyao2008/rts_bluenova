using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.Flocking
{
    [RequireComponent(typeof(SphereCollider))]
    public class FlockComponent : MonoBehaviour
    {

        float scaneRadius = 2;

        const int layer = 19;

        SphereCollider mSphereCollider;

        Vector3 mSeparationOffset;

        Vector3 mCohesionOffset;

        Vector3 mTargetPosition;

        float lerp = 0.2f;

        float mSmoothOffset = 0.1f;

        bool moveable;

        void Awake()
        {
            mSphereCollider = GetComponent<SphereCollider>();
            mSphereCollider.radius = 0.5f;
            gameObject.layer = layer;
        }

        public void OnUpdate()
        {
            // Flock();
            // Move();
            Vector3 speed = Steer();
            if (speed.magnitude > 0.1f)
            {
                transform.position += Steer().normalized * 2 * Time.deltaTime;
            }
        }

        void Update()
        {
            OnUpdate();
        }

        Collider[] colliders;
        void Flock()
        {
            // colliders = Physics.OverlapSphere(transform.position, scaneRadius, 1 << layer);
            mSeparationOffset = Vector3.zero;
            mCohesionOffset = Vector3.zero;
            mTargetPosition = Vector3.zero;
            moveable = false;
            lerp = FlockManager.Instance.flockPercent;
            // if (colliders != null && colliders.Length > 1)
            // {
            for (int i = 0; i < FlockManager.Instance.members.Length; i++)
            {
                //Separation
                if (FlockManager.Instance.members[i].transform != transform)
                {
                    Vector3 direct = (transform.position - FlockManager.Instance.members[i].transform.position).normalized;
                    float distance = Vector3.Distance(transform.position, FlockManager.Instance.members[i].transform.position);
                    if (distance < 1.1f)
                    {
                        mSeparationOffset += direct * (scaneRadius - distance) * 2;
                        lerp = 0;
                    }
                    else
                    {
                        mSeparationOffset += direct * (scaneRadius - distance);
                    }
                }

                if (FlockManager.Instance.members[i].transform != transform)
                {
                    mCohesionOffset += FlockManager.Instance.members[i].transform.position;
                }
            }
            mSeparationOffset = mSeparationOffset / (FlockManager.Instance.members.Length - 1);
            mCohesionOffset = mCohesionOffset / (FlockManager.Instance.members.Length - 1);
            mTargetPosition = Vector3.Lerp(mSeparationOffset + transform.position, mCohesionOffset, lerp);
            moveable = true;
            // }
        }


        void Move()
        {
            //TODO
            if (moveable)
            {
                Vector3 direct = (mTargetPosition - transform.position).normalized;
                float distance = Vector3.Distance(mTargetPosition, transform.position);
                transform.position += direct * Mathf.Min(distance, 1) * Time.deltaTime;
            }
        }


        Vector3 mSpeeds = Vector3.zero;

        Vector3 Steer()
        {

            //Cohesion.
            Vector3 centerOffset = FlockManager.Instance.center.position - transform.position;

            Vector3 velocityOffset = FlockManager.Instance.speed - mSpeeds;

            colliders = Physics.OverlapSphere(transform.position, scaneRadius, 1 << layer);

            Vector3 separationOffset = Vector3.zero;

            bool doubleSeparation = false;
            //Separation
            // if (colliders != null && colliders.Length > 1)
            // {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform != transform)
                {
                    Vector3 direct = (transform.position - colliders[i].transform.position).normalized;
                    float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                    if (distance < 1.01f)
                    {
                        doubleSeparation = true;
                        separationOffset += direct * (scaneRadius - distance) * 10;
                    }
                    else
                    {
                        separationOffset += direct * (scaneRadius - distance);
                    }
                    //     lerp = 0;
                    // }
                    // else
                    // {
                    //     mSeparationOffset += direct * (scaneRadius - distance);
                    // }
                }
            }
            separationOffset = separationOffset / (colliders.Length - 1);
            // }
            // if (doubleSeparation)
            //     return separationOffset;
            // else
            return FlockManager.Instance.centerWeight * centerOffset + FlockManager.Instance.velocityWeight * velocityOffset + FlockManager.Instance.separationWeight * separationOffset;
        }
    }
}
