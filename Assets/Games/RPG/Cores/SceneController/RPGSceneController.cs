using System.Collections.Generic;
using UnityEngine;
using BlueNoah.RPG.View;
using RTS;

namespace BlueNoah.RPG.SceneControl
{
    public class RPGSceneController : SimpleSingleMonoBehaviour<RPGSceneController> {

        RTSPlayerController mRTSPlayerController;

        SceneCore mSceneCore;

        SceneViewer mSceneViewer;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        void Start()
        {
            mSceneCore.OnStart();
        }

        void FixedUpdate()
        {
            mSceneCore.OnUpdate();
        }

        private void Update()
        {
            mRTSPlayerController.OnUpdate();
        }

        private void OnGUI()
        {
            mRTSPlayerController.OnGUI();
        }

        void InitInput()
        {
            mRTSPlayerController = new RTSPlayerController();
            mRTSPlayerController.onCreateActor = SpawnActor;
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
      

        void Init()
        {
            mSceneCore = SceneCore.Instance;
            mSceneViewer = gameObject.GetOrAddComponent<SceneViewer>();
            mSceneCore.SetActorOnSpawn(mSceneViewer.SpawnActorView);
            mSceneCore.SetActorOnRemove(mSceneViewer.RemoveActorView);
            mSceneCore.OnAwake();
            InitInput();
        }

        public void SpawnActor(int playerId, int actorId, Vector3 targetPosition, Vector3 eulerAngle)
        {
            //mSceneCore.SpawnActor(playerId, actorId, targetPosition.ToFixedPointVector3(), eulerAngle.ToFixedPointVector3());
        }
    }
}
