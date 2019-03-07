using System.Collections;
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
        int mStageId;
        public UnityAction<MapMonster> onSpawnActor;

        public void LoadStage(int stageId)
        {
            mStageId = stageId;

            mMonsterDataList = CSVManager.Instance.LoadMonsterCSV(stageId);

            for (int i = 0; i < mMonsterDataList.Count; i++)
            {
                if (onSpawnActor != null)
                {
                    onSpawnActor(mMonsterDataList[i]);
                }
            }
        }
    }
}
