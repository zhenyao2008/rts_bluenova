﻿using System.Collections.Generic;
using BlueNoah.Event;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using UnityEngine;
//パート、1.Grid;2.GridView.
namespace BlueNoah.Build
{
    //1.Click 选中。
    //2.再次TouchDown,如果目标是选中物体，开始拖动。
    //3.TouchUp，停止拖动。 期间对地点是否越界进行判断，只能在网格范围以内移动。对于不可放置的地点进行红色注明。
    //4.点击无建筑地点，建筑还原，
    public class BuildManager : SimpleSingleMonoBehaviour<BuildManager>
    {

        Material mMaterial;
        FixedPointGrid mFixedPointGrid;
        GameObject mSelectedBuilding;
        bool mIsControllable;
        GridView mGridView;
        public FixedPoint64 nodeWidth = 1f;
        public FixedPoint64 diagonalPlus = 1f;
        public FixedPointVector3 startPos = FixedPointVector3.zero;
        public float padding = 0.04f;
        public int xCount = 60;
        public int zCount = 30;
        public int gridViewLayer;

        GameObject mSelectBuilding;

        Vector3 mSelectBuildingOffset;

        bool mIsDraging;

        protected override void Awake()
        {
            base.Awake();

            InitInput();

            InitGrid();

            InitGridView();
        }

        void InitInput()
        {
            EasyInput.Instance.AddListener(Event.TouchType.TouchBegin, OnTouchDown);

            EasyInput.Instance.AddListener(Event.TouchType.Touch, OnTouch);

            EasyInput.Instance.AddListener(Event.TouchType.Click, OnClick);

            EasyInput.Instance.AddListener(Event.TouchType.TouchEnd, OnTouchUp);
        }

        void InitGrid()
        {

            mFixedPointGrid = new FixedPointGrid();

            FixedPointGridSetting gridSetting = new FixedPointGridSetting();

            gridSetting.nodeWidth = nodeWidth;

            gridSetting.diagonalPlus = diagonalPlus;

            gridSetting.startPos = startPos;

            gridSetting.xCount = (uint)xCount;

            gridSetting.zCount = (uint)zCount;

            mFixedPointGrid.Init(gridSetting);

            NodeEnableUtility.CheckNodeBlocking(mFixedPointGrid);
        }

        void InitGridView()
        {
            mMaterial = Resources.Load<Material>("Materials/node");

            gridViewLayer = LayerConstant.LAYER_GROUND;

            mGridView = gameObject.GetOrAddComponent<GridView>();

            mGridView.InitGridView(xCount, zCount, nodeWidth.AsFloat(), padding, mMaterial, gridViewLayer);

            mGridView.ShowGrid();
        }

        void OnTouchDown(EventData eventData)
        {
            if (mSelectBuilding != null)
            {
                RaycastHit raycastHit;
                if (CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_BUILDING))
                {
                    if (mSelectBuilding == raycastHit.collider.transform.gameObject)
                    {
                        if (CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
                        {
                            CameraControl.CameraController.Instance.IsControllable = false;
                            mSelectBuildingOffset = raycastHit.point - mSelectBuilding.transform.position;
                            mIsDraging = true;
                        }
                    }
                }
                else
                {
                    CameraControl.CameraController.Instance.IsControllable = true;
                }
            }
        }

        void OnTouch(EventData eventData)
        {
            if (mSelectBuilding != null && mIsDraging)
            {
                eventData.touchPos0.z = 10;

                Vector3 position = Camera.main.ScreenToWorldPoint(eventData.touchPos0);

                Vector3 forward = (position - Camera.main.transform.position).normalized;

                RaycastHit raycastHit;

                if (Physics.Raycast(Camera.main.transform.position, forward, out raycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
                {
                    //得到目标位置，根据这个位置取到对应node的位置。
                    Vector3 targetPos = raycastHit.point - mSelectBuildingOffset; //- new FixedPointVector3(this.nodeWidth / 2, 0, this.nodeWidth / 2).ToVector3();
                    Building building = mSelectBuilding.GetComponent<Building>();

                    Vector3 gridOffset = new Vector3((building.xSize / 2f - 0.5f) * this.nodeWidth.AsFloat(), 0, (building.ySize / 2f - 0.5f) * this.nodeWidth.AsFloat());
                    List <FixedPointNode> nodes =  mFixedPointGrid.GetNearestNodes((targetPos - gridOffset) .ToFixedPointVector3() , building.xSize , building.ySize);
                    if (nodes != null && nodes.Count > 0)
                    {
                        //collider的起始点其实是node中心，因为要从原点开始，所以实际位置需要减0.5个node宽度
                        building.transform.position = nodes[0].pos.ToVector3() + gridOffset;
                        if (building.currentNodes != null)
                        {
                            for (int i = 0; i < building.currentNodes.Count; i++)
                            {
                                building.currentNodes[i].IsBlock = false;
                                mGridView.ResetNodeColorByWorldPosition(building.currentNodes[i].pos.ToVector3());
                            }
                        }
                        building.currentNodes = nodes;
                        if (building.currentNodes != null)
                        {
                            for (int i = 0; i < building.currentNodes.Count; i++)
                            {
                                building.currentNodes[i].IsBlock = true;
                                mGridView.BlockNodeColorByWorldPosition(building.currentNodes[i].pos.ToVector3());
                            }
                        }
                        mGridView.ApplyColors();
                    }
                }
            }
        }

        void OnClick(EventData eventData)
        {
            if (mSelectBuilding != null)
            {
                mSelectBuilding = null;
            }
            else
            {
                RaycastHit raycastHit;
                if (CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_BUILDING))
                {

                    mSelectBuilding = raycastHit.collider.transform.gameObject;

                    mSelectBuilding.GetComponent<Building>().OnSpring();
                }
            }
        }

        void OnTouchUp(EventData eventData)
        {
            mIsDraging = false;
        }


        public void Create()
        {

        }

        public void Place()
        {

        }

        public void Cancel()
        {

        }

        public bool IsControllable
        {
            get
            {
                return mIsControllable;
            }
            set
            {
                mIsControllable = value;
            }
        }

        private void OnDrawGizmos()
        {
            if (mFixedPointGrid != null)
                mFixedPointGrid.OnDrawGizmos();
        }
    }
}
