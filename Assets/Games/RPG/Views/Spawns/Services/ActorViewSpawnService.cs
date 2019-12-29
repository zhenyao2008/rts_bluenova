/*
 *　2019.12.28 午後1時12分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using System.Collections.Generic;
using BlueNoah.AI.Spawn.View;
using UnityEngine;
namespace BlueNoah.RPG.View
{
    public class ActorViewSpawnService : ActorViewBaseSpawnService
    {

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
            if (!PlayerActors.ContainsKey(actorViewer.actorCore.actorAttribute.playerId))
            {
                PlayerActors.Add(actorViewer.actorCore.actorAttribute.playerId, new Dictionary<long, ActorViewer>());
            }
            PlayerActors[actorViewer.actorCore.actorAttribute.playerId].Add(actorViewer.actorCore.actorAttribute.actorId, actorViewer);
        }

        public ActorViewSpawnService()
        {
            mCachedActor = new Dictionary<int, GameObject>();
        }

        GameObject GetPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public GameObject SpawnActor(ActorCore actorCore)
        {
            if (!mCachedActor.ContainsKey(actorCore.actorAttribute.playerId))
            {
                if (actorCore.actorAttribute.playerId == 1)
                {
                    mCachedActor.Add(actorCore.actorAttribute.playerId, GetPrefab("Prefabs/Actors/Footman/Soldier_Militia"));
                }
                else
                {
                    mCachedActor.Add(actorCore.actorAttribute.playerId, GetPrefab("Prefabs/Actors/Footman/Soldier_Militia_Red"));
                }
            }
            GameObject go = GameObject.Instantiate(this.mCachedActor[actorCore.actorAttribute.playerId]);
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