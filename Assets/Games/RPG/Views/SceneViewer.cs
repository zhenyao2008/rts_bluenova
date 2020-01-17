using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.RPG.View
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

        private void OnDrawGizmos()
        {
            //PathFindingMananger.Single.Grid.OnDrawGizmos();
        }

    }
}
