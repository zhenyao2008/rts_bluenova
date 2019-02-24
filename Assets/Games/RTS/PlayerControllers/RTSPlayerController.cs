using System.Collections.Generic;
using BlueNoah.AI.View.RTS;
using BlueNoah.Event;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
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
                        //FixedPointNode fixedPointNode = PathFindingMananger.Single.Grid.GetNode(raycastHit.point.ToFixedPointVector3());
                        List<FixedPointVector3> fixedPointVectors= GetNearPositions(raycastHit.point.ToFixedPointVector3(), mSelectedActors.Count);
                        int index = 0;
                        foreach (ActorViewer actorViewer in mSelectedActors.Values)
                        {
                            if (actorViewer != null)
                            {
                                actorViewer.actorCore.MoveTo(fixedPointVectors[index], actorViewer.actorAnimation.Idle);
                            }
                            index++;
                        }
                    }
                }
            }
        }

        List<FixedPointVector3> GetNearPositions(FixedPointVector3 center, int count)
        {
            List<FixedPointVector3> positions = new List<FixedPointVector3>();
            FixedPointNode fixedPointNode = PathFindingMananger.Single.Grid.GetNode(center);

            int size = Mathf.CeilToInt(Mathf.Pow(count,0.5f));
            int xStart = size / 2;
            int yStart = size / 2;
            int index = 0;
            for (int i = fixedPointNode.x - xStart; i < fixedPointNode.x - xStart + size; i++)
            {
                for (int j = fixedPointNode.z - yStart; j < fixedPointNode.z - yStart + size; j++)
                {
                    positions.Add(PathFindingMananger.Single.Grid.GetNode(i,j).pos);
                    index++;
                    if (positions.Count == count)
                    {
                        return positions;
                    }
                }
            }
            return positions;
        }


    }
}