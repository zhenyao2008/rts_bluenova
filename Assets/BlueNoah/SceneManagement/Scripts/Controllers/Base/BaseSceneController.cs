using UnityEngine;
using UnityEngine.EventSystems;

namespace BlueNoah.SceneControl
{

    public abstract class BaseSceneController<T> : SimpleSingleMonoBehaviour<T> where T : MonoBehaviour
    {

        protected Transform mCameraMoveArea;

        protected bool mIsStoryPlaying;

        protected bool mIsLocalMode;

        protected override void Awake()
        {
            base.Awake();

            InitData();

            InitEnviroment();

            InitBuildingGrid();

            InitBuildings();

            InitUnitGrid();

            InitInput();

            InitUnits();

            InitSmallObjects();

            InitCameraMoveArea();

            InitCamera();

            InitUI();

            InitImpact();
        }

        protected virtual void InitData()
        {
            if (mIsLocalMode)
            {
                //TODO load datas
                //MasterDataManage.InitAllCSV();
            }
        }

        protected abstract void InitEnviroment();

        protected abstract void InitBuildingGrid();

        protected abstract void InitBuildings();

        protected abstract void InitUnitGrid();

        protected abstract void InitUnits();

        protected abstract void InitSmallObjects();

        protected abstract void InitInput();

        protected abstract void InitCamera();

        protected virtual void InitCameraMoveArea()
        {
            //GameObject moveAreaGo = GameObject.Find("CameraMoveArea");
            //if (moveAreaGo != null)
            //{
            //    BoxCollider boxCollider = moveAreaGo.GetComponent<BoxCollider>();
            //    if (boxCollider != null)
            //    {
            //        mCameraMoveArea = moveAreaGo.transform;
            //        CameraController.Instance.SetCameraMoveArea(boxCollider);
            //    }
            //}
        }

        protected virtual void InitImpact()
        {

        }

        protected abstract void InitUI();

        public bool IsLocalMode
        {
            get
            {
                return mIsLocalMode;
            }
        }


        public bool IsBlock
        {
            get
            {
                return IsBlockByUI || InBlockByStory;
            }
        }

        bool IsBlockByUI
        {
            get
            {
#if UNITY_EDITOR
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return true;
                }
#else
            if (EventSystem.current != null && Input.touchCount > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }
#endif
                return false;
            }
        }

        bool InBlockByStory
        {
            get
            {
                return mIsStoryPlaying;
            }
        }
    }
}
