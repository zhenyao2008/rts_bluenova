﻿/*
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
using UnityEngine.EventSystems;
using BlueNoah.RTS.UI;
using BlueNoah.Build;

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

            EasyInput.Instance.AddListener(BlueNoah.Event.TouchType.Click, 1, OnRightClick);

            BlueNoah.CameraControl.CameraController.Instance.MoveSpeed = TD.Config.InGameConfig.Single.cameraDragSpeed;
        }

        public void OnGUI()
        {
            mScreenSelectService.OnGUI();
        }

        void OnClick(EventData eventData)
        {
            if (!eventData.currentTouch.isPointerOnGameObject && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
            {
                UnSelectActors();
                RaycastHit raycastHit;
                if (BlueNoah.CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
                {
                    if (onCreateActor != null)
                    {
                        Debug.Log("1234");
                        onCreateActor(playerId, 1, raycastHit.point, Vector3.zero);
                    }
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
                        mSelectedActors.Add(actorViewer.actorCore.actorAttribute.actorId, actorViewer);
                        actorViewer.gameObject.GetOrAddComponent<ActorHighlighter>().ShowHighlighter();
                    }
                }
            }
        }

        void OnRightClick(EventData eventData)
        {
            if (!eventData.currentTouch.isPointerOnGameObject && !UICamera.isOverUI && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
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
                                if (Input.GetKey(KeyCode.A))
                                {
                                    //TODO let action system to do this,can not 操る　actorCore by Player directly.
                                    actorViewer.actorCore.ActorAI.MoveTo(null, fixedPointVectors[index], true, true);
                                }
                                else
                                {
                                    //TODO let action system to do this,can not 操る　actorCore by Player directly.
                                    actorViewer.actorCore.ActorAI.MoveTo(null, fixedPointVectors[index], true, false);
                                }
                            }
                            index++;
                        }
                    }
                }
            }
        }

        public void OnUpdate()
        {
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