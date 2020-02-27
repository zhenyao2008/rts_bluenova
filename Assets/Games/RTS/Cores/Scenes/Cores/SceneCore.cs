using BlueNoah.AI.FSM;
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

        //TODO available unit data list.
        List<MapMonster> mMonsterDataList;

        public SceneCore()
        {
            FiniteStateMachineLoader.LoadAIConfig("configs/fsms/j_fsm_rts_normal_actor");

            mActorCoreSpawnService = new ActorCoreSpawnService();

            mPathFindingMananger = PathFindingMananger.NewInstance();

            mStageService = new StageService();

            mStageService.onSpawnActor = SpawnStageActor;

            mStageService.onSpawnBuildingActor = SpawnStageActor;

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

        public void SpawnActor(int playerId,int  actorId, Vector3 targetPosition)
        {
            MapMonster mapMonster = new MapMonster();
            mapMonster.alignment = playerId;
            mapMonster.angle_y = 0;
            mapMonster.unit_id = actorId;
            mapMonster.name = "a";
            mapMonster.pos_x = (int)(targetPosition.x * 1000);
            mapMonster.pos_y = (int)(targetPosition.z * 1000);
            mActorCoreSpawnService.SpawnActor(mapMonster);
        }

        public void SpawnBuilding(int playerId, int actorId, Vector3 targetPosition)
        {
            MapMonster mapMonster = new MapMonster();
            mapMonster.alignment = playerId;
            mapMonster.angle_y = 0;
            mapMonster.unit_id = actorId;
            mapMonster.pos_x = (int)(targetPosition.x * 1000);
            mapMonster.pos_y = (int)(targetPosition.z * 1000);
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