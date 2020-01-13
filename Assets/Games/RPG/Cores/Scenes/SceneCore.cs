using System.Collections.Generic;
using BlueNoah.AI.Stage;
using BlueNoah.CSV;
using BlueNoah.RPG.PathFinding;
using BlueNoah.RPG.Spawn;
using BlueNoah.SceneControl;
using UnityEngine;

namespace BlueNoah.RPG.SceneControl
{
    public class SceneCore : SimpleSingleTon<SceneCore>
    {
        ActorCoreSpawnService mActorCoreSpawnService;
        PathFindingManager mPathFindingManager;
        StageService mStageService;
        AreaService mAreaService;

        public ActorCoreSpawnService ActorCoreSpawnService
        {
            get
            {
                return mActorCoreSpawnService;
            }
        }

        public SceneCore()
        {
            mActorCoreSpawnService = new ActorCoreSpawnService();

            mPathFindingManager = PathFindingManager.NewInstance();

            mStageService = new StageService();

            //mStageService.onSpawnActor = SpawnStageActor;
        }
        public void OnAwake()
        {
            InitPathFinding();
        }

        public void OnStart()
        {
            mStageService.LoadStage(0);
        }

        //Core calculation function.
        public void OnUpdate()
        {
            mActorCoreSpawnService.OnUpdate();
            mPathFindingManager.OnUpdate();
        }

        void InitPathFinding()
        {
            BattleFieldDataSO battleFieldDataSO = new BattleFieldDataSO();
            battleFieldDataSO.TileMapCellSize = 1;
            battleFieldDataSO.TileMapHeight = 100;
            battleFieldDataSO.TileMapWidth = 100;
            battleFieldDataSO.TileMap = new BattleTileInfo[10000];
            //TODO need to load from file.
            for (int i = 0; i < 100 * 100; i++)
            {
                battleFieldDataSO.TileMap[i] = new BattleTileInfo();
                battleFieldDataSO.TileMap[i].Height = 0;
                battleFieldDataSO.TileMap[i].MainAttr = FieldMainAttr.Normal;
                battleFieldDataSO.TileMap[i].Normal = Vector3.zero;
                battleFieldDataSO.TileMap[i].SlopeAttr = FieldMainAttr.Normal;
                battleFieldDataSO.TileMap[i].SubAttr = FieldMainAttr.Normal;
            }
            PathFindingManager.Single.InitPathFinding(battleFieldDataSO);
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
