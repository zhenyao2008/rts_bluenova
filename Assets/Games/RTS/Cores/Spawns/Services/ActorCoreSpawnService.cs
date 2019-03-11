/*
 *　2019.2.23 午後６時４９分
 *　應　彧剛(yingyugang@gmail.com)
*/

using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.Math.FixedPoint;

namespace BlueNoah.AI.Spawn
{
    public delegate void ActorSpawnEventAction<T0>(T0 t0);

    public class ActorCoreSpawnService
    {
        public ActorSpawnEventAction<ActorCore> onActorCreate;

        public ActorSpawnEventAction<ActorCore> onActorRemove;

        Dictionary<int, List<ActorCore>> mPlayerActors;

        public ActorCoreSpawnService()
        {
            mPlayerActors = new Dictionary<int, List<ActorCore>>();
        }

        public Dictionary<int, List<ActorCore>> PlayerActors
        {
            get
            {
                if (mPlayerActors == null)
                {
                    mPlayerActors = new Dictionary<int, List<ActorCore>>();
                }
                return mPlayerActors;
            }
        }

        void AddActor(ActorCore actorCore)
        {
            if (!PlayerActors.ContainsKey(actorCore.actorAttribute.playerId))
            {
                PlayerActors.Add(actorCore.actorAttribute.playerId, new List<ActorCore>());
            }
            PlayerActors[actorCore.actorAttribute.playerId].Add(actorCore);
        }

        public ActorCore SpawnActor(int playerId, int actorTypeId, FixedPointVector3 position, FixedPointVector3 eulerAngle)
        {
            //TODO
            ActorCore actorCore = new ActorCore(playerId, actorTypeId, 1, position, eulerAngle);

            AddActor(actorCore);

            if (onActorCreate != null)
                onActorCreate(actorCore);

            return actorCore;
        }

        public void RemoveActor(ActorCore actorCore)
        {
            if (PlayerActors.ContainsKey(actorCore.actorAttribute.playerId))
            {
                PlayerActors[actorCore.actorAttribute.playerId].Remove(actorCore);
                if (onActorRemove != null)
                    onActorRemove(actorCore);
            }
        }
    }
}