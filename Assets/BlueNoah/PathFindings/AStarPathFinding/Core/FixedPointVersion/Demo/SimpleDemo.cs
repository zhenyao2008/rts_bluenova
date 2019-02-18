using UnityEngine;
using System.Collections.Generic;
using BlueNoah.Math.FixedPoint;

namespace BlueNoah.PathFinding.FixedPoint
{

    public class SimpleDemo : MonoBehaviour
    {

        public Material material;

        FixedPointPathAgent mPathAgent;

        public Transform target;

        GridView mGridView;

        FixedPointGrid mGrid;

        const int FRAME_RATE = 60;

        FixedPointMoveAgent mMoveAgent;

        void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            material = Resources.Load<Material>("Materials/node");
            mGrid = new FixedPointGrid();
            FixedPointGridSetting gridSetting = new FixedPointGridSetting();
            gridSetting.nodeWidth = 0.5f;
            gridSetting.diagonalPlus = 1.4f;
            gridSetting.startPos = new FixedPointVector3(0, 0, 0);
            gridSetting.xCount = 150;
            gridSetting.zCount = 80;
            mGrid.Init(gridSetting);
            mPathAgent = new FixedPointPathAgent(mGrid);
            //CreateCharacter();
            mMoveAgent = CreateMoveAgent();
            //NodeEnableUtility.
            //mGrid.EnableNodes();
            NodeEnableUtility.BlockForSample(mGrid);
            //return;
            //mGridView = gameObject.AddComponent<GridView>();
            //mGridView.InitGridView((int)gridSetting.xCount, (int)gridSetting.zCount, gridSetting.nodeWidth.AsFloat(), 0.1f, material, 0);
            //mGridView.ShowGrid();
            //mGridView.gameObject.layer = 10;
            //mGridView.boxCollider.gameObject.layer = 10;
            CameraControl.CameraController.Instance.MoveSpeed = 8;
            CameraControl.CameraController.Instance.RotateSpeed = 40;
            //CameraControl.CameraController.Instance.IsVerticalRotateable
        }

        void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Vector3 viewPos = (Vector3)Input.mousePosition;
            //    viewPos.z = 10;
            //    //mPathAgent.transform.position = new FixedPointVector3(target.position.x, target.position.y, target.position.z);//  Vector3Int.RoundToInt(target.position * 100);
            //    Vector3 pos = Camera.main.ScreenToWorldPoint(viewPos);
            //    RaycastHit raycastHit;
            //    if (CameraController.Instance.RaycastByOrthographicCamera(out raycastHit, LayerConstant.LAYER_GROUND))
            //    {
            //        Vector3 hitPosition = raycastHit.point;
            //        FixedPointVector3 fixedPointVector3 = new FixedPointVector3(hitPosition.x, hitPosition.y, hitPosition.z);
            //        Vector3Int intPosition = Vector3Int.CeilToInt(hitPosition);
            //        List<FixedPointNode> nodeList = mPathAgent.StartFind(new FixedPointVector3(target.position.x, target.position.y, target.position.z), fixedPointVector3);
            //        //for (int i = 0; i < nodeList.Count; i++)
            //        //{
            //        //    Node node = nodeList[i];
            //        //    mGridView.HoverNodeColorByWorldPosition(node.pos.ToVector3());
            //        //}
            //        //mGridView.ApplyColors();
            //        mMoveAgent.transform.position = new FixedPointVector3(target.position.x, target.position.y, target.position.z);
            //        mMoveAgent.StartMove(nodeList,null);
            //    }
            //}
            //if (mMoveAgent != null)
            //mMoveAgent.OnUpdate();

            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit raycastHit;

                if (CameraControl.CameraController.Instance.GetWorldTransFromMousePosition(out raycastHit, LayerConstant.LAYER_GROUND))
                {
                    //GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //GO.transform.position = raycastHit.point;
                    //Debug.Log(raycastHit.point);
                }

            }
        }

        //void CreateCharacter()
        //{
        //    UnitSpawnService unitSpawnService = new UnitSpawnService();
        //    target = unitSpawnService.CreateCharacter(1).transform;
        //    target.gameObject.SetActive(true);
        //    target.localScale = Vector3.one * 0.4f;
        //    target.position = new Vector3(-2.65f, 0, -8.32f);
        //    target.GetComponent<NavMeshAgent>().enabled = false;
        //}

        private void FixedUpdate()
        {
            if (mMoveAgent != null)
            {
                mMoveAgent.OnUpdate();
            }
        }

        void DoMove(List<FixedPointNode> searchComplete)
        {
            mMoveAgent = CreateMoveAgent();
            foreach (FixedPointNode node in searchComplete)
            {
                Debug.Log(node.x + "==" + node.z);
            }
            mMoveAgent.StartMove(searchComplete, null);
        }

        FixedPointMoveAgent CreateMoveAgent()
        {

            GameObject modelGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);

            GameObject GO = new GameObject("Unit");

            modelGO.transform.SetParent(GO.transform);

            modelGO.transform.localPosition = Vector3.zero;

            modelGO.transform.localEulerAngles = Vector3.zero;

            modelGO.transform.localScale = Vector3.one;

            target = GO.transform;

            mMoveAgent = new FixedPointMoveAgent();

            mMoveAgent.transform = new FixedPointTransform();

            mMoveAgent.viewTransform = target;

            mMoveAgent.deltaDistancePerFrame = 2 * Time.fixedDeltaTime;

            return mMoveAgent;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (mPathAgent != null)
                mPathAgent.OnDrawGizmos();
            if (mGrid != null)
                mGrid.OnDrawGizmos();
        }
#endif
    }
}
