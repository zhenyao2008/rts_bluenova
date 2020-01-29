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
            mCachedActor = new Dictionary<string, GameObject>();
        }

        GameObject GetPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public GameObject SpawnActor(ActorCore actorCore)
        {
            string path = "";

            if (actorCore.actorAttribute.playerId == 1)
            {
                if (actorCore.actorAttribute.actorTypeId == 14)
                {
                    path = "Prefabs/Actors/Footman/Soldier_Militia";
                }
                else if (actorCore.actorAttribute.actorTypeId == 100)
                {
                    path = "Prefabs/Buildings/Building";
                }
            }
            else
            {
                if (actorCore.actorAttribute.actorTypeId == 14)
                {
                    path = "Prefabs/Actors/Footman/Soldier_Militia_Red";
                }
                else if (actorCore.actorAttribute.actorTypeId == 100)
                {
                    path = "Prefabs/Buildings/Building";
                }
            }
            if (!mCachedActor.ContainsKey(path))
            {
                mCachedActor.Add(path, GetPrefab(path));
            }
            GameObject go = GameObject.Instantiate(this.mCachedActor[path]);
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