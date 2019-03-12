/*
 *　2019.2.29 午後７時４分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/
using System.Collections.Generic;
using BlueNoah.AI.View;
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

            EasyInput.Instance.AddListener(BlueNoah.Event.TouchType.Click, 0, OnClick);

            BlueNoah.CameraControl.CameraController.Instance.MoveSpeed = TD.Config.InGameConfig.Single.cameraDragSpeed;
        }

        public void OnGUI()
        {
            mScreenSelectService.OnGUI();
        }

        void OnClick(EventData eventData)
        {
            UnSelectActors();
            RaycastHit raycastHit;
            if (BlueNoah.CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
            {
                if (onCreateActor != null)
                {
                    onCreateActor(playerId, 1, raycastHit.point, Vector3.zero);
                }
            }
        }

        void OnSeleced(Rect rect)
        {
            Dictionary<long, ActorViewer> actorViewers = RTSSceneController.Instance.GetActorViewers(playerId);
            UnSelectActors();
            if (actorViewers != null)
            {
                foreach (ActorViewer actorViewer in actorViewers.Values)
                {
                    if (mScreenSelectService.IsInSelectRect(rect, actorViewer.screenPosition.x, actorViewer.screenPosition.y))
                    {
                        Debug.Log(actorViewer);
                        mSelectedActors.Add(actorViewer.ActorCore.actorAttribute.actorId, actorViewer);
                        actorViewer.gameObject.GetOrAddComponent<ActorHighlighter>().ShowHighlighter();
                    }
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
                    if (BlueNoah.CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
                    {
                        //FixedPointNode fixedPointNode = PathFindingMananger.Single.Grid.GetNode(raycastHit.point.ToFixedPointVector3());
                        List<FixedPointVector3> fixedPointVectors = GetNearPositions(raycastHit.point.ToFixedPointVector3(), mSelectedActors.Count);
                        int index = 0;
                        foreach (ActorViewer actorViewer in mSelectedActors.Values)
                        {
                            if (actorViewer != null)
                            {
                                actorViewer.ActorCore.MoveTo(fixedPointVectors[index], actorViewer.actorAnimation.Idle);
                            }
                            index++;
                        }
                    }
                }
            }
        }

        void UnSelectActors()
        {
            foreach (ActorViewer actorViewer in mSelectedActors.Values)
            {
                actorViewer.gameObject.GetOrAddComponent<ActorHighlighter>().HideHighlighter();
            }
            mSelectedActors.Clear();
        }

        List<FixedPointVector3> GetNearPositions(FixedPointVector3 center, int count)
        {
            List<FixedPointVector3> positions = new List<FixedPointVector3>();
            FixedPointNode fixedPointNode = PathFindingMananger.Single.Grid.GetNode(center);
            int size = Mathf.CeilToInt(Mathf.Pow(count, 0.5f));
            int xStart = size / 2;
            int yStart = size / 2;
            int index = 0;
            for (int i = fixedPointNode.x - xStart; i < fixedPointNode.x - xStart + size; i++)
            {
                for (int j = fixedPointNode.z - yStart; j < fixedPointNode.z - yStart + size; j++)
                {
                    positions.Add(PathFindingMananger.Single.Grid.GetNode(i, j).pos);
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