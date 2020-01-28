/*
 *　2019.2.23 午後７時６分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using RTS;
using BlueNoah.Math.FixedPoint;
using BlueNoah.SceneControl.View;
using UnityEngine;
using System.Collections.Generic;
using BlueNoah.AI.View.RTS;

namespace BlueNoah.SceneControl
{
    public class RTSSceneController : SimpleSingleMonoBehaviour<RTSSceneController>
    {

        RTSPlayerController mRTSPlayerController;

        SceneCore mSceneCore;

        SceneViewer mSceneViewer;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        void Init()
        {
            mSceneCore = SceneCore.Instance;
            mSceneViewer = gameObject.GetOrAddComponent<SceneViewer>();
            mSceneCore.SetActorOnSpawn(mSceneViewer.SpawnActorView);
            mSceneCore.SetActorOnRemove(mSceneViewer.RemoveActorView);
            mSceneCore.OnAwake();
            InitInput();
        }

        void Start()
        {
            mSceneCore.OnStart();
        }

        void FixedUpdate()
        {
            mSceneCore.OnUpdate();
        }

        public Dictionary<long, ActorViewer> GetActorViewers(int playerId)
        {
            if (mSceneViewer.GetActors().ContainsKey(playerId))
            {
                return mSceneViewer.GetActors()[playerId];
            }
            return null;
        }

        public ActorViewer GetActorViewer(int playerId, long actorId)
        {
            if (mSceneViewer.GetActors().ContainsKey(playerId))
            {
                if (mSceneViewer.GetActors()[playerId].ContainsKey(actorId))
                {
                    return mSceneViewer.GetActors()[playerId][actorId];
                }
            }
            return null;
        }

        void InitBuildingGrid()
        {
            //throw new System.NotImplementedException();
        }

        void InitBuildings()
        {
            //throw new System.NotImplementedException();
        }

        void InitCamera()
        {
            //throw new System.NotImplementedException();
        }

        void InitEnviroment()
        {
            //throw new System.NotImplementedException();
        }

        void InitInput()
        {
            mRTSPlayerController = new RTSPlayerController();
            mRTSPlayerController.onCreateActor = SpawnActor;
        }

        void InitSmallObjects()
        {
            //throw new System.NotImplementedException();
        }

        void InitUI()
        {
            //throw new System.NotImplementedException();
        }

        void InitUnitGrid()
        {
            //throw new System.NotImplementedException();
        }

        void InitUnits()
        {
            //throw new System.NotImplementedException();
        }

        public void SpawnActor(int playerId, int actorId, Vector3 targetPosition, Vector3 eulerAngle)
        {
            mSceneCore.SpawnActor(playerId, actorId, targetPosition);
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

