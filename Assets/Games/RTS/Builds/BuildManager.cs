using System.Collections.Generic;
using BlueNoah.AI.View;
using BlueNoah.CSV;
using BlueNoah.Event;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding;
using BlueNoah.PathFinding.FixedPoint;
using BlueNoah.SceneControl;
using UnityEngine;
using UnityEngine.EventSystems;
//パート、1.Grid;2.GridView.
namespace BlueNoah.Build
{
    public delegate GameObject SpawnActorGO(string path);
    //1.Click 选中。
    //2.再次TouchDown,如果目标是选中物体，开始拖动。
    //3.TouchUp，停止拖动。 期间对地点是否越界进行判断，只能在网格范围以内移动。对于不可放置的地点进行红色注明。
    //4.点击无建筑地点，建筑还原，
    public class BuildManager : SimpleSingleMonoBehaviour<BuildManager>
    {

        Material mMaterial;
        FixedPointGrid mFixedPointGrid;
        GameObject mSelectedBuilding;
        GridViewGroup mGridViewGroup;
        public FixedPoint64 nodeWidth = 1f;
        public FixedPoint64 diagonalPlus = 1f;
        public FixedPointVector3 startPos = FixedPointVector3.zero;
        public float padding = 0.04f;
        public int xCount = 100;
        public int zCount = 100;
        public int gridViewLayer;

        GameObject mSelectBuilding;

        ActorCSVStructure mBuildingCSVStructure;

        Vector3 mSelectBuildingOffset;

        public SpawnActorGO onSpawnActorGO;

        bool mIsDraging;
        bool mIsControllable;

        protected override void Awake()
        {
            base.Awake();

            InitInput();

            InitGrid();

            InitGridView();
        }

        void InitInput()
        {
            EasyInput.Instance.AddListener(Event.TouchType.TouchBegin, 0, OnTouchDown);

            EasyInput.Instance.AddListener(Event.TouchType.Touch, 0, OnTouch);

            EasyInput.Instance.AddListener(Event.TouchType.Click, 0, OnClick);

            EasyInput.Instance.AddListener(Event.TouchType.TouchEnd, 0, OnTouchUp);
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

            mGridViewGroup = new GridViewGroup();

            mGridViewGroup.InitGridViewGroup(xCount, zCount, nodeWidth.AsFloat(), mMaterial, gridViewLayer, transform);

            mGridViewGroup.ShowGrid();
        }

        void OnTouchDown(EventData eventData)
        {
            if (!eventData.currentTouch.isPointerOnGameObject && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
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
        }

        void OnTouch(EventData eventData)
        {
            if (mSelectBuilding != null && mIsDraging)
            {
                Vector3 touchPosition = eventData.currentTouch.touch.position;

                touchPosition.z = 10;

                Vector3 position = Camera.main.ScreenToWorldPoint(touchPosition);

                Vector3 forward = (position - Camera.main.transform.position).normalized;

                RaycastHit raycastHit;

               

                if (Physics.Raycast(Camera.main.transform.position, forward, out raycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
                {
                    //得到目标位置，根据这个位置取到对应node的位置。
                    Vector3 targetPos = raycastHit.point - mSelectBuildingOffset; //- new FixedPointVector3(this.nodeWidth / 2, 0, this.nodeWidth / 2).ToVector3();
                    //ActorBuilding building = mSelectBuilding.GetOrAddComponent<ActorBuilding>();
                    //building.xSize = 3;
                    //building.ySize = 3;
                    //Vector3 gridOffset = new Vector3((building.xSize / 2f - 0.5f) * this.nodeWidth.AsFloat(), 0, (building.ySize / 2f - 0.5f) * this.nodeWidth.AsFloat());
                    List<FixedPointNode> nodes = mFixedPointGrid.GetNearestNodes((targetPos).ToFixedPointVector3(), mBuildingCSVStructure.size_x ,mBuildingCSVStructure.size_y);
                    if (nodes != null && nodes.Count > 0)
                    {
                        //collider的起始点其实是node中心，因为要从原点开始，所以实际位置需要减0.5个node宽度
                        mSelectBuilding.transform.position = nodes[0].pos.ToVector3();
                        /*
                        if (building.currentNodes != null)
                        {
                            for (int i = 0; i < building.currentNodes.Count; i++)
                            {
                                //building.currentNodes[i].IsBlock = false;
                                //mGridViewGroup.SetNodeColor(building.currentNodes[i].x, building.currentNodes[i].z,Color.green);
                            }
                        }
                        building.currentNodes = nodes;
                        if (building.currentNodes != null)
                        {
                            for (int i = 0; i < building.currentNodes.Count; i++)
                            {
                                //building.currentNodes[i].IsBlock = true;
                                //mGridViewGroup.SetNodeColor(building.currentNodes[i].x, building.currentNodes[i].z, Color.red);
                            }
                        }
                        */
                        //mGridViewGroup.ApplyColors();
                    }
                }
            }
        }

        void OnClick(EventData eventData)
        {
            if (!eventData.currentTouch.isPointerOnGameObject && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())) {
                if (mSelectBuilding != null)
                {
                    //UnSelect.
                    mSelectBuilding.gameObject.GetOrAddComponent<ActorHighlighter>().HideHighlighter();

                    mSelectBuilding = null;
                }
                else
                {
                    //Select.
                    RaycastHit raycastHit;
                    if (CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.LAYER_BUILDING))
                    {

                        mSelectBuilding = raycastHit.collider.transform.gameObject;

                        //mSelectBuilding.GetComponent<ActorBuilding>().OnSpring();

                        //mSelectBuilding.gameObject.GetOrAddComponent<ActorHighlighter>().ShowHighlighter();
                    }
                }
            }
        }

        void OnTouchUp(EventData eventData)
        {
            if (!eventData.currentTouch.isPointerOnGameObject && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
            {
                if (mIsDraging)
                {
                    RTSSceneController.Instance.SpawnBuilding(1, mBuildingCSVStructure.id, mSelectBuilding.transform.position, mSelectBuilding.transform.eulerAngles);
                    GameObject.Destroy(mSelectBuilding);
                }
                CameraControl.CameraController.Instance.IsControllable = true;
                mIsDraging = false;
            }
        }

        public void Create(ActorCSVStructure buildingCSVStructure)
        {
            if (this.onSpawnActorGO!=null)
            {
                mSelectBuilding = onSpawnActorGO(buildingCSVStructure.resource_path);
                mSelectBuilding.transform.position = CameraControl.CameraController.Instance.GetCameraForwardPosition();
                mBuildingCSVStructure = buildingCSVStructure;
            }
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

        public void UpdateNodesColor(FixedPointNode node)
        {
            if (node.IsBlock)
            {
                mGridViewGroup.SetNodeColor(node.x, node.z, Color.red);
            }
            else if (node.isWall)
            {
                mGridViewGroup.SetNodeColor(node.x, node.z, Color.yellow);
            }
            else if (node.isStair)
            {
                mGridViewGroup.SetNodeColor(node.x, node.z, new Color(1,0.7f,0.1f,1));
            }
            else
            {
                mGridViewGroup.SetNodeColor(node.x, node.z, Color.green);
            }
        }

        public void UpdateNodesVertexs(FixedPointNode node,Vector3 normal)
        {
             mGridViewGroup.SetNodeHeight(node.x, node.z, node.pos.ToVector3(), normal);
        }

        public void ApplyColors()
        {
            mGridViewGroup.ApplyColors();
        }

        public void ApplyVertexs()
        {
            mGridViewGroup.ApplyVertexs();
        }

        private void OnDrawGizmos()
        {
            if (mFixedPointGrid != null)
                mFixedPointGrid.OnDrawGizmos();
        }
    }
}
