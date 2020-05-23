using System;
using BlueNoah.CSV;
using RVO;
using UnityEngine;
using UnityEngine.AI;

namespace BlueNoah.SimpleRTS
{
    public class Actor : MonoBehaviour
    {

        Action currentAction;

        MapMonster mapMonster;

        int enmeyLayer;

        int selfLayer;

        //NavMeshAgent navMeshAgent;

        Vector3 destination;

        Animation actorAnimation;

        int scanRadiu = 2;

        Transform trans;

        float currentHP = 100;

        float maxHP = 100;

        float attack = 10;

        float interval = 1;

        float attackDistance = 3;

        int rvoId;

        public void Init(MapMonster mapMonster)
        {
            trans = transform;
            this.mapMonster = mapMonster;
            if (mapMonster.alignment == 1)
            {
                enmeyLayer = 17;
                selfLayer = 16;
            }
            else
            {
                enmeyLayer = 16;
                selfLayer = 17;
            }
            gameObject.layer = selfLayer;
            destination = transform.position + transform.forward * 40;

            //navMeshAgent = GetComponent<NavMeshAgent>();

            //navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;

            actorAnimation = GetComponentInChildren<Animation>();

            rvoId = RVOController.Instance.AddAgent(trans);

           // navMeshAgent.speed = 5;
        }

        public void Begin()
        {
            currentAction = EnterRunAction;
        }

        public void GetDamage(float damage)
        {
            currentHP -= damage;

            if (currentHP <= 0)
            {
                EnterDeathAction();
            }
        }

        void Update()
        {
            currentAction?.Invoke();
        }

        float nextScanTime;

        float scanInterval = 0.5f;

        Actor target;

        void Scan()
        {
            if (nextScanTime < Time.realtimeSinceStartup)
            {
                nextScanTime = Time.realtimeSinceStartup + scanInterval;

                var targets = Physics.OverlapSphere(trans.position, scanRadiu, 1 << enmeyLayer);

                Actor targetActor = null;

                float distance = float.MaxValue;

                foreach (Collider collider in targets)
                {
                    float currentDis = Vector3.SqrMagnitude(trans.position - collider.transform.position);

                    if (currentDis <= attackDistance * attackDistance)
                    {
                        target = collider.GetComponent<Actor>();
                        EnterAttackAction();
                        return;
                    }
                    else
                    {
                        if (currentDis < distance)
                        {
                            targetActor = collider.GetComponent<Actor>();
                            distance = currentDis;
                        }
                    }
                }

                if (targetActor != null)
                {
                    target = targetActor;

                    if (currentAction == IdleAction)
                    {
                        EnterRunAction();
                    }
                    return;
                }
                scanRadiu = Mathf.Min(10, scanRadiu + 1);
            }
        }

        void EnterRunAction()
        {
            //NavMeshPath navMeshPath = new NavMeshPath();
            //navMeshAgent.CalculatePath(destination, navMeshPath);
            // navMeshAgent.SetPath(navMeshPath);
            // navMeshAgent.SetDestination(destination);
            RVOController.Instance.Move(rvoId,destination);
            if (actorAnimation)
            {
                actorAnimation.wrapMode = WrapMode.Loop;
                actorAnimation.Play("Run01");
            }
            currentAction = RunAction;
            //navMeshAgent.isStopped = false;
            RunAction();
        }

        void RunAction()
        {
            //if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= 0.5f)
            {
            //    EnterIdleAction();
            }

            //Scan();
        }

        void EnterIdleAction()
        {
            if (actorAnimation)
            {
                actorAnimation.wrapMode = WrapMode.Loop;
                actorAnimation.Play("Idle01");
            }
            currentAction = IdleAction;

            IdleAction();
        }

        void IdleAction()
        {
            Scan();
        }

        float nextAttackTime;

        float attackInterval = 0.6f;

        void EnterAttackAction()
        {
            currentAction = AttackAction;
            //navMeshAgent.isStopped = true;
            AttackAction();
        }

        void AttackAction()
        {
            if (nextAttackTime < Time.realtimeSinceStartup)
            {
                nextAttackTime = Time.realtimeSinceStartup + attackInterval;

                if (actorAnimation)
                {
                    actorAnimation.wrapMode = WrapMode.Once;
                    actorAnimation.Stop();
                    actorAnimation.Play("Attack01");
                }

                target.GetDamage(attack);

                if (target.currentHP <= 0)
                {
                    EnterIdleAction(); ;
                }
            }
        }

        void EnterDeathAction()
        {
            if (actorAnimation)
            {
                actorAnimation.wrapMode = WrapMode.Once;
                actorAnimation.Play("Death01");
            }
            //navMeshAgent.isStopped = true;
            //navMeshAgent.enabled = false;
            GetComponent<Collider>().enabled = false;
            currentAction = DeathAction;
        }

        void DeathAction()
        {

        }
    }
}
