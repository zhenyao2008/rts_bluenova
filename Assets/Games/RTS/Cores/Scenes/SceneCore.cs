using BlueNoah.AI.RTS;
using BlueNoah.AI.Spawn;
using BlueNoah.AI.Stage;
using BlueNoah.CSV;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;

namespace BlueNoah.SceneControl
{
    public class SceneCore
    {

        ActorCoreSpawnService mActorCoreSpawnService;

        PathFindingMananger mPathFindingMananger;

        StageService mStageService;

        public SceneCore()
        {
            mActorCoreSpawnService = new ActorCoreSpawnService();

            mPathFindingMananger = PathFindingMananger.NewInstance();

            mStageService = new StageService();

            mStageService.onSpawnActor = SpawnStageActor;

            mStageService.LoadStage(0);

        }
        //Core calculation function.
        public void OnUpdate()
        {
            mPathFindingMananger.OnUpdate();
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

        void SpawnStageActor(MapMonster mapMonster)
        {
            FixedPointVector3 position = new FixedPointVector3();

            FixedPointVector3 eulerAngles = new FixedPointVector3();

            int playerId = mapMonster.alignment;

            int actorTypeId = mapMonster.unit_id;

            mActorCoreSpawnService.SpawnActor(playerId, actorTypeId, position, eulerAngles);
        }
    }
}

