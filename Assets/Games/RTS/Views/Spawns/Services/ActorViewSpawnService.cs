/*
 *　2019.2.23 午後７時６分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.AI.View.RTS;
using UnityEngine;

namespace BlueNoah.AI.Spawn.View
{
    public class ActorViewSpawnService : ActorViewBaseSpawnService
    {

        //int: playerId;
        //long: actorCore.actorId;
        Dictionary<int, Dictionary<long, ActorViewer>> mPlayerActors;

        public Dictionary<int, Dictionary<long, ActorViewer>> PlayerActors
        {
            get
            {
                if (mPlayerActors == null)
                {
                    mPlayerActors = new Dictionary<int, Dictionary<long, ActorViewer>>();
                }
                return mPlayerActors;
            }
        }

        void AddActor(ActorViewer actorViewer)
        {
            if (!PlayerActors.ContainsKey(actorViewer.ActorCore.actorAttribute.playerId))
            {
                PlayerActors.Add(actorViewer.ActorCore.actorAttribute.playerId, new Dictionary<long, ActorViewer>());
            }
            PlayerActors[actorViewer.ActorCore.actorAttribute.playerId].Add(actorViewer.ActorCore.actorAttribute.actorId, actorViewer);
        }

        public ActorViewSpawnService()
        {
            mCachedActor = new Dictionary<int, GameObject>();
        }

        GameObject GetPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public override GameObject SpawnActor(ActorCore actorCore)
        {
            if (!mCachedActor.ContainsKey(actorCore.actorAttribute.actorTypeId))
            {
                mCachedActor.Add(actorCore.actorAttribute.actorTypeId, GetPrefab("Prefabs/Actors/Footman/Soldier_Militia"));
            }
            GameObject go = GameObject.Instantiate(this.mCachedActor[actorCore.actorAttribute.actorTypeId]);
            ActorViewer actorViewer = go.GetOrAddComponent<ActorViewer>();
            actorViewer.Init(actorCore);
            AddActor(actorViewer);
            return go;
        }

        public void RemoveActor(ActorCore actorCore)
        {
            ActorViewer actorViewer = PlayerActors[actorCore.actorAttribute.playerId][actorCore.actorAttribute.actorId];
            PlayerActors[actorCore.actorAttribute.playerId].Remove(actorCore.actorAttribute.actorId);
        }
    }
}
