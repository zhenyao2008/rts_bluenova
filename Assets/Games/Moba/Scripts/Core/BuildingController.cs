using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class BuildingController : MonoBehaviour
{

    public static BuildingController instance;
    public static BuildingController SingleTon()
    {
        return instance;
    }
	#region Hanlde
	public float minPressDuration = 1;

	float mDownTime;

	float mPressDuration;

	float mActiveDragTime;

	Vector3 mSelectOffset;
	#endregion

    public List<GameObject> allPlanes;

    public List<GameObject> allBuildings;

    public List<CityBuilding> cityBuildings;

    public GameObject currentBuilding;

	public GameObject currentPlane;

	public bool isNewBuilding = false;

	public bool isNewBuildingFirstClick = true;

	public bool isDraging;

	public bool isClick;

	RaycastHit mRaycastHit;

	GameObject mDownBuilding;

	GameObject mUpBuilding;

	Vector3 mSelectBuildingDefaultPos;

	DataCenter mDataCenter;

	bool mEnableBilding = false;

	UICamera mCurrentUICamera;

	public float buildingSize = 10;

	public int gridSize = 2;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        mDataCenter = DataCenter.Instance();
        mDataCenter.LoadCityBuilding();
        List<string> buildingRes = mDataCenter.buildingRes;
        if (buildingRes != null)
        {
            List<Vector3> buildingPos = mDataCenter.buildingPos;
            for (int i = 0; i < buildingRes.Count; i++)
            {
                GameObject prefab = GetPrefab(buildingRes[i]);
                GameObject go = Instantiate(prefab) as GameObject;
                go.transform.position = buildingPos[i];
                go.GetComponent<Collider>().enabled = true;
                this.allBuildings.Add(go);
            }
        }
    }

    GameObject GetPrefab(string bName)
    {
        GameObject prefab = null;
        foreach (CityBuilding cb in cityBuildings)
        {
            if (bName == cb.buildingName)
            {
                prefab = cb.gameObject;
                break;
            }
        }
        return prefab;
    }

    void OnApplicationQuit()
    {
        mDataCenter.SaveCityBuilding(allBuildings);
        mDataCenter.SaveUserInfo(CityPanel_I.SingleTon().userInfo);
    }

    void Update()
    {
        if (UICamera.isOverUI)
        {
            return;
        }
#if IPHONE || ANDROID
		if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
        if (EventSystem.current.IsPointerOverGameObject())
#endif
        {
            return;
        }

        if (CameraController.SingleTon().isMoving)
        {
            return;
        }

        if (mEnableBilding)
            HandleBuilding();
    }

    void HandleBuilding()
    {
        //以下のコード使わない。（調べたい）
        if (Input.GetMouseButtonDown(0))
        {
            isNewBuildingFirstClick = false;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mRaycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND | 1 << LayerConstant.playerLayer0))
            {
                mDownBuilding = mRaycastHit.transform.gameObject;
                mDownTime = Time.time;
                mActiveDragTime = Time.time + minPressDuration;
                if (currentBuilding != null)
                {
                    if (currentBuilding == mDownBuilding)
                    {
                        mSelectBuildingDefaultPos = currentBuilding.transform.position;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mRaycastHit, Mathf.Infinity, 1 << 15))
                        {
                            mSelectOffset = mRaycastHit.point - currentBuilding.transform.position;
                            mSelectOffset.y = 0;
                        }
                        isDraging = true;
                        CameraController.SingleTon().enabled = false;
                        EasyTouch.instance.enable = false;
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mRaycastHit, Mathf.Infinity, 1 << 15 | 1 << 18))
            {
                if (mRaycastHit.transform.gameObject.layer == 18 && mDownBuilding == mRaycastHit.transform.gameObject)
                {
                    if (!isDraging && Time.time > mActiveDragTime)
                    {
                        currentBuilding = mRaycastHit.transform.gameObject;
                        mSelectBuildingDefaultPos = currentBuilding.transform.position;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mRaycastHit, Mathf.Infinity, 1 << 15))
                        {
                            mSelectOffset = mRaycastHit.point - currentBuilding.transform.position;
                            mSelectOffset.y = 0;

                        }
                        CityPanel_I.SingleTon().ShowBuildingButtons();
                        currentBuilding.GetComponent<CityBuilding>().Select();
                        isDraging = true;
                        CameraController.SingleTon().enabled = false;
                        EasyTouch.instance.enable = false;
                    }
                }

                if (currentBuilding && isDraging)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mRaycastHit, Mathf.Infinity, 1 << 15))
                    {
                        Vector3 hitPos = mRaycastHit.point - mSelectOffset;
                        currentBuilding.transform.position = hitPos - new Vector3(hitPos.x % gridSize, hitPos.y % gridSize, hitPos.z % gridSize) + new Vector3(0, 0.16f, 0);
                        if (CheckPlaceAble())
                        {
                            currentBuilding.GetComponent<CityBuilding>().ShowDefault();
                        }
                        else
                        {
                            currentBuilding.GetComponent<CityBuilding>().ShowDisable();
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mRaycastHit, Mathf.Infinity, 1 << 15 | 1 << 18))
            {
                if (isDraging)
                {
                    if (!isNewBuilding)
                    {
                        DeSelect();
                    }
                }
                if (!isDraging)
                {
                    mUpBuilding = mRaycastHit.transform.gameObject;
                    if (!isNewBuilding && mUpBuilding.layer == 18 && mUpBuilding == mDownBuilding && Time.time - mDownTime < 0.3f)
                    {
                        if (currentBuilding != null)
                        {
                            currentBuilding.GetComponent<CityBuilding>().DeSelect();
                        }
                        currentBuilding = mRaycastHit.transform.gameObject;
                        CityBuilding cb = currentBuilding.GetComponent<CityBuilding>();
                        cb.Select();
                        CityPanel_I.SingleTon().ShowBuildingButtons();
                        mSelectBuildingDefaultPos = currentBuilding.transform.position;
                    }
                    else
                    {
                        if (currentBuilding != null && !isNewBuildingFirstClick && !isNewBuilding)
                        {
                            DeSelect();
                        }
                    }
                }
            }
            isDraging = false;
            CameraController.SingleTon().enabled = true;
            EasyTouch.instance.enable = true;
        }
    }

    public void SelectBuilding(GameObject selectBuilding)
    {
        SpawnPoint sp = selectBuilding.GetComponent<SpawnPoint>();
        sp.OnSelected();
    }

    public void DeSelect()
    {
        if (currentBuilding)
        {
            if (!CheckPlaceAble())
            {
                currentBuilding.transform.position = mSelectBuildingDefaultPos;
            }
            else
            {
                currentBuilding.GetComponent<CityBuilding>().Place();
            }
            currentBuilding.GetComponent<CityBuilding>().DeSelect();
            currentBuilding.GetComponent<CityBuilding>().ShowDefault();
            CityPanel_I.SingleTon().MoveOutMsgButtons();
            currentBuilding = null;
        }
    }


    public void SetPreBuilding(GameObject go)
    {
        currentBuilding = go;
    }

    public void Place()
    {
        if (CheckPlaceAble())
        {
            CameraController.SingleTon().enabled = true;
            EasyTouch.instance.enable = true;
            currentBuilding.GetComponent<CityBuilding>().Place();
            if (!isNewBuilding)
            {
                CityPanel_I.SingleTon().MoveOutMsgButtons();
            }
            else
            {
                currentBuilding.transform.position = new Vector3(currentBuilding.transform.position.x, 0.15f, currentBuilding.transform.position.z);
                allBuildings.Add(currentBuilding);
                CityPanel_I.SingleTon().confirmBtns.gameObject.SetActive(false);
                isNewBuilding = false;
            }

            currentBuilding = null;
        }
        else
        {
            if (!isNewBuilding)
            {
                currentBuilding.transform.position = mSelectBuildingDefaultPos;
                CameraController.SingleTon().enabled = true;
                EasyTouch.instance.enable = true;
            }
        }
    }

    public void CancelPlace()
    {
        CameraController.SingleTon().enabled = true;
        EasyTouch.instance.enable = true;
        if (isNewBuilding)
        {
            if (currentBuilding)
            {
                isNewBuilding = false;
                Destroy(currentBuilding);
            }
        }
        CityPanel_I.SingleTon().confirmBtns.gameObject.SetActive(false);
    }

    public bool CheckPlaceAble()
    {
        bool result = false;
        foreach (GameObject go in allPlanes)
        {
            if (go.GetComponent<Collider>().bounds.Intersects(currentBuilding.GetComponent<Collider>().bounds))
            {
                Bounds bounds = go.GetComponent<Collider>().bounds;
                Vector3 pos0 = currentBuilding.transform.position + currentBuilding.transform.forward * buildingSize + currentBuilding.transform.right * buildingSize;
                if (!bounds.Contains(pos0))
                {
                    break;
                }
                pos0 = currentBuilding.transform.position + currentBuilding.transform.forward * buildingSize - currentBuilding.transform.right * buildingSize;
                if (!bounds.Contains(pos0))
                {
                    break;
                }
                pos0 = currentBuilding.transform.position - currentBuilding.transform.forward * buildingSize - currentBuilding.transform.right * buildingSize;
                if (!bounds.Contains(pos0))
                {
                    break;
                }
                pos0 = currentBuilding.transform.position - currentBuilding.transform.forward * buildingSize + currentBuilding.transform.right * buildingSize;
                if (!bounds.Contains(pos0))
                {
                    break;
                }
                result = true;
                break;
            }
        }
        if (!result)
        {
            return false;
        }

        foreach (GameObject go in allBuildings)
        {
            if (go == null || currentBuilding == go)
                continue;
            if (currentBuilding.GetComponent<Collider>().bounds.Intersects(go.GetComponent<Collider>().bounds))
            {
                return false;
            }
        }
        return true;
    }
}
