using System.Collections.Generic;
using BlueNoah.AI.View.RTS;
using BlueNoah.Event;
using BlueNoah.SceneControl;
using UnityEngine;
using UnityEngine.Events;

namespace RTS
{
    public class RTSPlayerController
    {
        ScreenSelectService mScreenSelectService;

        public UnityAction<int, int, Vector3, Vector3> onCreateActor;

        public int playerId = 1;

        Dictionary<long, ActorViewer> mSelectedActors;

        public RTSPlayerController()
        {
            mSelectedActors = new Dictionary<long, ActorViewer>();

            mScreenSelectService = new ScreenSelectService();

            mScreenSelectService.onSelected = OnSeleced;

            EasyInput.Instance.AddListener(BlueNoah.Event.TouchType.Click, OnClick);
        }

        public void OnGUI()
        {
            mScreenSelectService.OnGUI();
        }

        void OnClick(EventData eventData)
        {
            if (mSelectedActors.Count > 0)
            {
                mSelectedActors.Clear();
            }
            RaycastHit raycastHit;
            if (BlueNoah.CameraControl.CameraController.Instance.GetWorldTransFromMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
            {
                //RTSSceneController.Instance.SpawnActorView(playerId, 1,raycastHit.point.ToFixedPointVector3());
                if (onCreateActor != null)
                {
                    Debug.Log(raycastHit.point);
                    onCreateActor(playerId, 1, raycastHit.point, Vector3.zero);
                }
            }
        }

        void OnSeleced(Rect rect)
        {
            Dictionary<long, ActorViewer> actorViewers = RTSSceneController.Instance.GetActorViewers(playerId);
            mSelectedActors.Clear();
            foreach (ActorViewer actorViewer in actorViewers.Values)
            {
                if (mScreenSelectService.IsInSelectRect(rect, actorViewer.screenPosition.x, actorViewer.screenPosition.y))
                {
                    Debug.Log(actorViewer);
                    mSelectedActors.Add(actorViewer.actorCore.actorAttribute.actorId, actorViewer);
                }
            }
        }

        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (mSelectedActors.Count > 0)
                {
                    RaycastHit raycastHit;
                    if (BlueNoah.CameraControl.CameraController.Instance.GetWorldTransFromMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
                    {
                        foreach (ActorViewer actorViewer in mSelectedActors.Values)
                        {
                            if (actorViewer != null)
                            {
                                actorViewer.actorCore.MoveTo(raycastHit.point.ToFixedPointVector3());
                            }
                        }
                    }
                }
            }
        }
    }
}