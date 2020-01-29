using System.Collections.Generic;
using BlueNoah.CSV;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.AI.Stage
{
    // actors in the scene.
    public class StageService
    {
        List<MapMonster> mMonsterDataList;
        List<MapMonster> mMapBuildingDataList;
        int mStageId;
        public UnityAction<MapMonster> onSpawnActor;
        public UnityAction<MapMonster> onSpawnBuildingActor;

        public void LoadStage(int stageId)
        {
            mStageId = stageId;

            mMonsterDataList = CSVManager.Instance.LoadMapMonsterCSV(stageId);

            Debug.Log("mMonsterDataList:" + mMonsterDataList.Count);

            for (int i = 0; i < mMonsterDataList.Count; i++)
            {
                if (onSpawnActor != null)
                {
                    onSpawnActor(mMonsterDataList[i]);
                }
            }

            mMapBuildingDataList = CSVManager.Instance.LoadMapBuildingCSV(stageId);

            for (int i = 0; i < mMapBuildingDataList.Count; i++)
            {
                if (onSpawnBuildingActor != null)
                {
                    onSpawnBuildingActor(mMapBuildingDataList[i]);
                }
            }

        }
    }
}
