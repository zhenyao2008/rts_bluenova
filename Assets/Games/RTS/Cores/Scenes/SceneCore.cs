using BlueNoah.AI.RTS;
using BlueNoah.AI.Spawn;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;

namespace BlueNoah.SceneControl
{
    public class SceneCore
    {

        ActorCoreSpawnService mActorCoreSpawnService;

        PathFindingMananger mPathFindingMananger;

        public SceneCore()
        {
            mActorCoreSpawnService = new ActorCoreSpawnService();

            mPathFindingMananger =  PathFindingMananger.NewInstance();
        }
        //Core calculation function.
        public void OnUpdate()
        {
            //TODO
        }

        public void SetActorOnSpawn(ActorSpawnEventAction<ActorCore> onActorCreate)
        {
            mActorCoreSpawnService.onActorCreate = onActorCreate;
        }

        public void SetActorOnRemove(ActorSpawnEventAction<ActorCore> onActorRemove)
        {
            mActorCoreSpawnService.onActorRemove = onActorRemove;
        }

        public void SpawnActor(int playerId, int actorTypeId, FixedPointVector3 position, FixedPointVector3 eulerAngles)
        {
            mActorCoreSpawnService.SpawnActor(playerId, actorTypeId, position, eulerAngles);
        }

        public void MoveTo(ActorCore actorCore,FixedPointVector3 targetPosition)
        {
            actorCore.MoveTo(targetPosition);
        }
    }
}

