using BlueNoah.AI.RTS;
using BlueNoah.AI.Spawn;
using BlueNoah.AI.Stage;
using BlueNoah.CSV;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using UnityEngine;

namespace BlueNoah.SceneControl
{
    public class SceneCore : SimpleSingleTon<SceneCore>
    {

        ActorCoreSpawnService mActorCoreSpawnService;
        PathFindingMananger mPathFindingMananger;
        StageService mStageService;
        AreaService mAreaService;

        public SceneCore()
        {
            mActorCoreSpawnService = new ActorCoreSpawnService();

            mPathFindingMananger = PathFindingMananger.NewInstance();

            mStageService = new StageService();

            mStageService.onSpawnActor = SpawnStageActor;

        }

        public void OnAwake()
        {
            mAreaService.OnAwake();
        }

        public void OnStart()
        {
            mAreaService.OnStart();

            mStageService.LoadStage(0);
        }

        //Core calculation function.
        public void OnUpdate()
        {
            mAreaService.OnUpdate();

            mActorCoreSpawnService.OnUpdate();

            mPathFindingMananger.OnUpdate();
        }

        public void OnDestory()
        {
            mAreaService.OnDestory();
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
            FixedPointVector3 position = new FixedPointVector3(mapMonster.pos_x, 0, mapMonster.pos_y);

            FixedPointVector3 eulerAngles = new FixedPointVector3(0, mapMonster.angle_y, 0);

            int playerId = mapMonster.alignment;

            int actorTypeId = mapMonster.unit_id;

            mActorCoreSpawnService.SpawnActor(playerId, actorTypeId, position, eulerAngles);
        }
    }
}