using BlueNoah.Math.FixedPoint;
using BlueNoah.SceneControl;
using System.Collections.Generic;

namespace BlueNoah.AI.RTS
{
    public static class ScanUtility
    {

        public static ActorCore Scan(ActorCore actor)
        {
            int playerId = actor.actorAttribute.playerId;
            List<ActorCore> targetActors = SceneCore.Instance.GetActors(playerId);
            if (playerId == 1)
            {
                targetActors = SceneCore.Instance.GetActors(2);
            }
            else
            {
                targetActors = SceneCore.Instance.GetActors(1);
            }
            //TODO Need to amend performance.
            FixedPoint64 minDistance = FixedPoint64.MaxValue;
            ActorCore nearestTargetActor = null;
            for (int i = 0; i < targetActors.Count; i++)
            {
                FixedPointVector3 distance = actor.transform.position - targetActors[i].transform.position;
                if (distance.sqrMagnitude <= minDistance)
                {
                    minDistance = distance.sqrMagnitude;
                    nearestTargetActor = targetActors[i];
                }
            }
            return nearestTargetActor;
        }

        public static bool IsInAttackRange(ActorCore attacker,ActorCore target)
        {
            FixedPoint64 minDistance = (target.transform.position - attacker.transform.position).sqrMagnitude;

            return (minDistance <= attacker.attackRange * attacker.attackRange);
        }

        public static bool IsInScanRange(ActorCore attacker, ActorCore target)
        {
            FixedPoint64 minDistance = (target.transform.position - attacker.transform.position).sqrMagnitude;

            return (minDistance <= attacker.scanRange * attacker.scanRange);
        }
    }
}
