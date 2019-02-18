using System.Collections.Generic;
using BlueNoah.Spawn;
using UnityEngine;

namespace RTS
{
    public class RTSActorSpawnService : ActorSpawnService
    {

        public RTSActorSpawnService()
        {
            mCachedActor = new Dictionary<int, GameObject>();
        }

        GameObject GetPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public override GameObject SpawnActor(int actorId)
        {
            //TODO
            if (!mCachedActor.ContainsKey(actorId))
            {
                mCachedActor.Add(actorId,GetPrefab("Prefabs/Actors/Footman/Soldier_Militia"));
            }
            return GameObject.Instantiate(this.mCachedActor[actorId]); ;
        }
    }
}
