using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CityPanel : PanelBase {

	public UIButton buildBtn;
	public PanelBase buildingPanel;

	public UIButton battleBtn;
	public PanelBase battlePanel;

	public UIGrid btnGrid;

	public GameObject buildConfirm;
	public UIButton buildConfirmYesBtn;
	public UIButton buildConfirmNoBtn;

	public static CityPanel instance;
	public static CityPanel SingleTon()
	{
		return instance;
	}

	void Awake(){
		current = this;
		instance = this;
		List<Transform> items = btnGrid.GetChildList ();
		items [0].GetComponent<UIButton> ().onClick.Add (new EventDelegate(ShowBuildingInfo));
		shopButton.onClick.AddListener(new UnityEngine.Events.UnityAction(OnShopButtonClick));
	}

	protected override void Start()
	{
		base.Start ();
		if (buildBtn)
			buildBtn.onClick.Add (new EventDelegate(ShowBuildingPanel));
//		battleBtn.onClick.Add (new EventDelegate(ShowBuildingTips));
		if (buildConfirmYesBtn)
			buildConfirmYesBtn.onClick.Add (new EventDelegate(BuildingController.SingleTon().Place));
		if (buildConfirmNoBtn)
			buildConfirmNoBtn.onClick.Add (new EventDelegate(BuildingController.SingleTon().CancelPlace));
		btnGrid.Reposition ();
		List<Transform> childs = btnGrid.GetChildList ();
		for(int i=0;i<childs.Count;i++)
		{
			Transform t = childs[i];
			CityPanelItem cpi = t.GetComponent<CityPanelItem>();
			cpi.tp0.from = cpi.transform.localPosition;
			cpi.tp0.to = cpi.transform.localPosition + new Vector3 (0,140,0);
//			cpi.tp1.from = cpi.tp0.to;
//			cpi.tp1.to = cpi.tp0.from;
//			cpi.tc.ResetToBeginning ();
//			cpi.tp0.ResetToBeginning ();
		}
	}

	Vector3 mConfirmPos;
	public Vector3 confirmOffset = new Vector3(0,5,0);
	void LateUpdate(){
		if(buildConfirm.activeInHierarchy && BuildingController.SingleTon().currentBuilding)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint( BuildingController.SingleTon().currentBuilding.transform.position);
			mConfirmPos = UICamera.currentCamera.ScreenToWorldPoint (screenPos) + new Vector3(confirmOffset.x * (screenPos.x-Screen.width/2) / Screen.width/2, confirmOffset.y * Mathf.Cos(CameraController.SingleTon().rtsCamera.Tilt / 90) , 0);
			mConfirmPos.z = 0;
			buildConfirm.transform.position = mConfirmPos;
		}
	}

	public void ShowBuildingInfo(){
		BuildingController.SingleTon ().enabled = false;
		CityBuildingPanel.SingleTon ().preBuildingPanel = PanelBase.current;
		GameObject currentBuilding = BuildingController.SingleTon ().currentBuilding;
		CityBuilding cityBuilding = currentBuilding.GetComponent<CityBuilding> ();
		root.SetActive (false);
		CityBuildingPanel.SingleTon ().Active();
		CityBuildingPanel.SingleTon ().SetCurrentBuilding (cityBuilding);
		CityBuildingPanel.SingleTon ().returnButton.gameObject.SetActive (false);
		CityBuildingPanel.SingleTon ().SetBuildingInfo (BuildingController.SingleTon().currentBuilding.GetComponent<CityBuilding>());
	}

	public void ShowBuildingTips(CityBuilding cityBuilding)
	{
		List<Transform> items = btnGrid.GetChildList ();
		foreach(Transform t in items)
		{
			t.GetComponent<CityPanelItem>().Show();
		}
	}

	public void HideBuildingTips()
	{
		List<Transform> items = btnGrid.GetChildList ();
		foreach(Transform t in items)
		{
			t.GetComponent<CityPanelItem>().Hide();
		}
	}

	void ShowBuildingPanel()
	{
		if (buildingPanel) {
			buildingPanel.Active();
			BuildingController.SingleTon().enabled =false;
			CameraController.SingleTon().enabled = false;
			EasyTouch.instance.enable =false;
			root.SetActive(false);
		}
	}



	public Button shopButton;
	public Button battleButton;

	public void OnShopButtonClick()
	{
		Debug.Log ("OnShopButtonClick");
	}






}
