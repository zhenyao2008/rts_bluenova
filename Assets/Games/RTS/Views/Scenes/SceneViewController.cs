
using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.AI.Spawn.View;
using BlueNoah.AI.View.RTS;
using UnityEngine;

namespace BlueNoah.SceneControl.View
{
    public class SceneViewer : SingleMonoBehaviour<SceneViewer>
    {

        ActorViewSpawnService mActorViewSpawnService;

        public SceneViewer()
        {
            mActorViewSpawnService = new ActorViewSpawnService();
        }

        public void SpawnActorView(ActorCore actorCore)
        {
            GameObject go = mActorViewSpawnService.SpawnActor(actorCore);
            ActorViewer actorViewer = go.GetOrAddComponent<ActorViewer>();
            actorViewer.UpdateTransform();
        }

        public void RemoveActorView(ActorCore actorCore)
        {
            mActorViewSpawnService.RemoveActor(actorCore);
        }

        public Dictionary<int, Dictionary<long, ActorViewer>> GetActors()
        {
            return mActorViewSpawnService.PlayerActors;
        }

    }
}

