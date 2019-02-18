using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.Spawn
{
    public abstract class ActorSpawnService 
    {
        protected Dictionary<int, GameObject> mCachedActor;

        public void Init()
        {
            mCachedActor = new Dictionary<int, GameObject>();
        }

        public abstract GameObject SpawnActor(int actorId);
    }
}

