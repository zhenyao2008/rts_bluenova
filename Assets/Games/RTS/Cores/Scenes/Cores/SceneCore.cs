using BlueNoah.AI.RTS;
using BlueNoah.AI.Spawn;
using BlueNoah.AI.Stage;
using BlueNoah.CSV;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using System.Collections.Generic;
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

        }

        public void OnStart()
        {
            mStageService.LoadStage(0);
        }

        //Core calculation function.
        public void OnUpdate()
        {
            mActorCoreSpawnService.OnUpdate();
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

        void SpawnStageActor(MapMonster mapMonster)
        {
            mActorCoreSpawnService.SpawnActor(mapMonster);
        }

        public List<ActorCore> GetActors(int playerId)
        {
            if (mActorCoreSpawnService.PlayerActors.ContainsKey(playerId))
                return mActorCoreSpawnService.PlayerActors[playerId];
            else
                return new List<ActorCore>();
        }
    }
}