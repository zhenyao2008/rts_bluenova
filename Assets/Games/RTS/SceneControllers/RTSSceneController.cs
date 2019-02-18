using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using BlueNoah.Math.FixedPoint;

namespace BlueNoah.SceneControl
{
    public class RTSSceneController : BaseSceneController<RTSSceneController>
    {

        RTSPlayerController mRTSPlayerController;

        RTSActorSpawnService mRTSActorSpawnService;

        protected override void InitBuildingGrid()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitBuildings()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitCamera()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitEnviroment()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitInput()
        {
            mRTSPlayerController = new RTSPlayerController();

            mRTSActorSpawnService = new RTSActorSpawnService();

        }

        protected override void InitSmallObjects()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitUI()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitUnitGrid()
        {
            //throw new System.NotImplementedException();
        }

        protected override void InitUnits()
        {
            //throw new System.NotImplementedException();
        }

        public void SpawnActor(int playerId,int actorId,FixedPointVector3 targetPosition)
        {
            GameObject go = mRTSActorSpawnService.SpawnActor(actorId);
            go.transform.position = targetPosition.ToVector3();
        }

        private void OnGUI()
        {
            mRTSPlayerController.OnGUI();
        }

        private void Update()
        {
            mRTSPlayerController.OnUpdate();
        }
    }

}

