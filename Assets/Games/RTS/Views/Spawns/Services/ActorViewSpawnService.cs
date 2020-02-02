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

        public GameObject SpawnActor(ActorCore actorCore)
        {
            string path = actorCore.MapMonster.resource_path;
            if (!mCachedActor.ContainsKey(path))
            {
                mCachedActor.Add(path,GetPrefab(path));
            }
            Debug.Log(path);
            GameObject go = GameObject.Instantiate(mCachedActor[path]);
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
