using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class BuildingController : MonoBehaviour {

	public static BuildingController instance;
	public static BuildingController SingleTon(){
		return instance;
	}

	public List<GameObject> allPlanes;
	public List<GameObject> allBuildings;
	public List<CityBuilding> cityBuildings;
	public GameObject currentBuilding;
	public int gridSize = 2;

	void Awake(){
		instance = this;
	}

	RaycastHit hit;

	GameObject downBuilding;
	GameObject upBuilding;
	float downTime;

	public bool isNewBuilding = false;
	public bool isNewBuildingFirstClick = true;
	Vector3 selectBuildingDefaultPos;

	public bool isDraging;
	public bool isClick;

	public float minPressDuration = 1;
	float pressDuration;
	float activeDragTime;
	Vector3 selectOffset;

	DataCenter mDataCenter;
	void Start(){
		Init ();
	}

	void Init()
	{
		mDataCenter = DataCenter.Instance ();
		mDataCenter.LoadCityBuilding ();
	
		List<string> buildingRes = mDataCenter.buildingRes;
		if(buildingRes!=null)
		{
			List<Vector3> buildingPos = mDataCenter.buildingPos;
			for(int i=0;i<buildingRes.Count;i++)
			{
				GameObject prefab = GetPrefab(buildingRes[i]);
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.position = buildingPos[i];
				go.GetComponent<Collider>().enabled = true;
				this.allBuildings.Add(go);
			}
		}

		
	}

	GameObject GetPrefab(string bName){
		GameObject prefab = null;
		foreach(CityBuilding cb in cityBuildings)
		{
			if(bName == cb.buildingName)
			{
				prefab=cb.gameObject;
				break;
			}
		}
		return prefab;
	}
	
	void OnApplicationQuit()
	{
		mDataCenter.SaveCityBuilding (allBuildings);
		mDataCenter.SaveUserInfo (CityPanel_I.SingleTon().userInfo);
	}

	void Update()
	{
		if(UICamera.isOverUI)
		{
			return;
		}
#if IPHONE || ANDROID
		if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
		if (EventSystem.current.IsPointerOverGameObject())
#endif 
		{
//			Debug.Log("当前触摸在UI上");
			return;
		}

		if(CameraController.SingleTon().isMoving)
		{
			return;
		}


		if(Input.GetMouseButtonDown(0))
		{
//			Debug.Log("GetMouseButtonDown");
			isNewBuildingFirstClick = false;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<15 | 1<<18))
			{
				downBuilding = hit.transform.gameObject;
				downTime = Time.time;
				activeDragTime = Time.time + minPressDuration;
				if(currentBuilding!=null)
				{
					if(currentBuilding == downBuilding)
					{
						selectBuildingDefaultPos = currentBuilding.transform.position;
						if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<15))
						{
							selectOffset = hit.point - currentBuilding.transform.position;
							selectOffset.y = 0;
						}
						isDraging = true;
						CameraController.SingleTon().enabled = false;
						EasyTouch.instance.enable =false;
					}
				}
			}
		}
		if(Input.GetMouseButton(0) )
		{
//			Debug.Log("GetMouseButton");
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<15 | 1 << 18))
			{
				if(hit.transform.gameObject.layer == 18 && downBuilding == hit.transform.gameObject)
				{
					if(!isDraging && Time.time > activeDragTime)
					{
						currentBuilding = hit.transform.gameObject;
						selectBuildingDefaultPos = currentBuilding.transform.position;
						if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<15))
						{
							selectOffset = hit.point - currentBuilding.transform.position;
							selectOffset.y = 0;

						}
//						CityPanel.SingleTon().ShowBuildingTips(currentBuilding.GetComponent<CityBuilding>());
						CityPanel_I.SingleTon().ShowBuildingButtons();
						currentBuilding.GetComponent<CityBuilding>().Select();
						isDraging = true;
						CameraController.SingleTon().enabled = false;
						EasyTouch.instance.enable =false;
					}
				}

				if(currentBuilding && isDraging)
				{
					if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<15 ))
					{
						Vector3 hitPos = hit.point - selectOffset;
						currentBuilding.transform.position = hitPos - new Vector3(hitPos.x % gridSize,hitPos.y % gridSize,hitPos.z % gridSize) + new Vector3(0,0.16f,0);
						if(CheckPlaceAble())
						{
							currentBuilding.GetComponent<CityBuilding>().ShowDefault();
//							CityPanel.SingleTon().buildConfirmYesBtn.isEnabled = true;
						}
						else
						{
							currentBuilding.GetComponent<CityBuilding>().ShowDisable();
//							CityPanel.SingleTon().buildConfirmYesBtn.isEnabled = false;
						}
					}
				}
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
//			Debug.Log("GetMouseButtonUp");
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<15 | 1<<18))
			{
				if(isDraging)
				{
					if(!isNewBuilding)
					{
						//已有的建筑才会返回到初始位置
						DeSelect();
					}
				}
				if(!isDraging)
				{
					upBuilding = hit.transform.gameObject;
					if(!isNewBuilding && upBuilding.layer == 18 && upBuilding == downBuilding && Time.time - downTime < 0.3f)
					{
//						Debug.Log("1");
						if(currentBuilding!=null)
						{
							currentBuilding.GetComponent<CityBuilding>().DeSelect();
						}
						currentBuilding = hit.transform.gameObject;
						CityBuilding cb = currentBuilding.GetComponent<CityBuilding>();
						cb.Select();
//						CityPanel.SingleTon().ShowBuildingTips(cb);
						CityPanel_I.SingleTon().ShowBuildingButtons();
						ShowPlane();
						selectBuildingDefaultPos = currentBuilding.transform.position;
					}
					else
					{
						if(currentBuilding!=null && !isNewBuildingFirstClick && !isNewBuilding)
						{
//							Debug.Log("2");
							//已有的建筑才会返回到初始位置
							DeSelect();
						}
					}
				}
			}
			isDraging = false;
			CameraController.SingleTon().enabled = true;
			EasyTouch.instance.enable =true;
		}

//		if(currentBuilding)
//		{
//			if(isNewBuilding)
//			{
//				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<15))
//				{
//					currentBuilding.transform.position = hit.point - new Vector3(hit.point.x % gridSize,hit.point.y % gridSize,hit.point.z % gridSize) + new Vector3(0,0.15f,0);
//					
//					if(CheckPlaceAble())
//					{
//						currentBuilding.GetComponent<CityBuilding>().ShowDefault();
//					}
//					else
//					{
//						currentBuilding.GetComponent<CityBuilding>().ShowDisable();
//					}
//				}
//			}
//		}
	}

	public void ShowPlane(){
//		foreach(GameObject pGo in allPlanes)
//		{
//			pGo.GetComponent<Renderer>().enabled = true;
//		}
	}

	public void HidePlane(){
//		foreach(GameObject pGo in allPlanes)
//		{
//			pGo.GetComponent<Renderer>().enabled = false;
//		}
	}



	public void Select(){
	
	}

	public void DeSelect(){
		if (currentBuilding) {
			if(!CheckPlaceAble())
			{
				currentBuilding.transform.position = selectBuildingDefaultPos;
			}
			else
			{
				currentBuilding.GetComponent<CityBuilding> ().Place ();
			}
			currentBuilding.GetComponent<CityBuilding>().DeSelect();
			currentBuilding.GetComponent<CityBuilding>().ShowDefault();
//			CityPanel.SingleTon().HideBuildingTips();
			CityPanel_I.SingleTon().MoveOutMsgButtons();
			currentBuilding = null;
			HidePlane();
		}
	}


	public void SetPreBuilding(GameObject go)
	{
		currentBuilding = go;
	}

	UICamera mCurrentUICamera;
	public void Place()
	{
		if (CheckPlaceAble ()) {
			CameraController.SingleTon ().enabled = true;
			EasyTouch.instance.enable =true;
			currentBuilding.GetComponent<CityBuilding> ().Place ();
			if(!isNewBuilding){
				
//				CityPanel.SingleTon ().HideBuildingTips ();
				CityPanel_I.SingleTon().MoveOutMsgButtons();
			}
			else
			{
				currentBuilding.transform.position = new Vector3(currentBuilding.transform.position.x,0.15f,currentBuilding.transform.position.z);
				allBuildings.Add (currentBuilding);
				CityPanel_I.SingleTon().confirmBtns.gameObject.SetActive(false);
//				CityPanel.SingleTon ().buildConfirm.SetActive (false);
				isNewBuilding = false;
			}
				
			currentBuilding = null;
		} else {
			if(!isNewBuilding)
			{
				currentBuilding.transform.position = selectBuildingDefaultPos;
				CameraController.SingleTon ().enabled = true;
				EasyTouch.instance.enable =true;
			}
		}
		HidePlane ();
	}

	public void CancelPlace(){
		CameraController.SingleTon ().enabled = true;
		EasyTouch.instance.enable =true;
		if (isNewBuilding) {
			if(currentBuilding)
			{
				isNewBuilding = false;
				Destroy(currentBuilding);
			}
		}
		HidePlane ();
//		CityPanel.SingleTon ().buildConfirm.SetActive (false);
		CityPanel_I.SingleTon ().confirmBtns.gameObject.SetActive (false);
	}

	public float buildingSize = 10;
	public bool CheckPlaceAble()
	{
		bool result = false;
		foreach(GameObject go in allPlanes)
		{
			if(go.GetComponent<Collider>().bounds.Intersects(currentBuilding.GetComponent<Collider>().bounds))
			{
				Bounds bounds = go.GetComponent<Collider>().bounds;
				Vector3 pos0 = currentBuilding.transform.position + currentBuilding.transform.forward * buildingSize + currentBuilding.transform.right * buildingSize;
				if(!bounds.Contains(pos0))
				{
					break;
				}
				pos0 = currentBuilding.transform.position + currentBuilding.transform.forward * buildingSize - currentBuilding.transform.right * buildingSize;
				if(!bounds.Contains(pos0))
				{
					break;
				}
				pos0 = currentBuilding.transform.position - currentBuilding.transform.forward * buildingSize - currentBuilding.transform.right * buildingSize;
				if(!bounds.Contains(pos0))
				{
					break;
				}
				pos0 = currentBuilding.transform.position - currentBuilding.transform.forward * buildingSize + currentBuilding.transform.right * buildingSize;
				if(!bounds.Contains(pos0))
				{
					break;
				}
				result = true;
				break;
			}
		}
		if(!result)
		{
			return false;
		}

		foreach (GameObject go in allBuildings) {
			if (go==null || currentBuilding == go)
				continue;
			if (currentBuilding.GetComponent<Collider> ().bounds.Intersects (go.GetComponent<Collider> ().bounds)) {
				return false;
			}
		}
		return true;
	}
}
