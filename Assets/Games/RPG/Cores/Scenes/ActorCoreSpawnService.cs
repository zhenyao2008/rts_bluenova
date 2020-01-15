﻿/*
 *　2019.12.28 午後0時54分
 *　應　彧剛(yingyugang@gmail.com)
*/

using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.CSV;

namespace BlueNoah.RPG.Spawn
{
    public delegate void ActorSpawnEventAction<T0>(T0 t0);

    public class ActorCoreSpawnService
    {
        public ActorSpawnEventAction<ActorCore> onActorCreate;

        public ActorSpawnEventAction<ActorCore> onActorRemove;

        Dictionary<int, List<ActorCore>> mPlayerActors;

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


        public ActorCore SpawnActor(MapMonster mapMonster)
        {
            ActorCore actorCore = new ActorCore(mapMonster);

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

        public void OnUpdate()
        {
            if (mPlayerActors == null)
                return;
            foreach (List<ActorCore> actors in mPlayerActors.Values)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].OnUpdate();
                }
            }
        }
    }
}