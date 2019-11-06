
using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.AI.Spawn.View;
using BlueNoah.AI.View.RTS;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using UnityEngine;

namespace BlueNoah.SceneControl.View
{
    public class SceneViewer : SingleMonoBehaviour<SceneViewer>
    {

        ActorViewSpawnService mActorViewSpawnService;

        FixedPointGridDebuger mFixedPointGridDebuger;

        public SceneViewer()
        {
            mActorViewSpawnService = new ActorViewSpawnService();

            mFixedPointGridDebuger = new FixedPointGridDebuger(PathFindingMananger.Single.Grid);

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
            PathFindingMananger.Single.Grid.OnDrawGizmos();
        }

    }
}

