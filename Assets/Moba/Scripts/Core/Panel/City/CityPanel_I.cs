using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
public class CityPanel_I : CityBasePanel_I {

	public Button shopButton;
	public Button battleButton;
	public Button buildingInfoButton;

	public Transform confirmBtns;
	public Button confirmYes;
	public Button confirmNo;
	public Button settingBtn;

	public List<Button> msgButtons;
	public Transform msgButtonCenter;
	public Vector2 msgButtonSize = new Vector2(110,110);
	public Vector3 msgTweenOffset = new Vector3(0,170,0);
	public float msgIntervalDelay = 0.2f;

	public UserInfo userInfo;

	public Text userName;
	public Text userLevel;
	public Text userExp;
	public Text workNum;
	public Text protectTime;
	public Text maxCorn;
	public Text currentCorn;
	public Text maxWood;
	public Text currentWood;
	public Text maxStone;
	public Text currentStone;
	public Text currentBaoShi;

	public TextAsset cityPanelLua;

	public static CityPanel_I instance;
	public static CityPanel_I SingleTon(){
		return instance;
	}

	void Awake(){
        //LuaScriptMgr mgr = new LuaScriptMgr();

        //luascriptmgr mgr = new luascriptmgr();
        //		mgr.Start();
        //		mgr.DoString(cityPanelLua.text);
        //		settingButtonClick = mgr.lua;
        ////		settingButtonClick.DoString(cityPanelLua.text);
        //		settingButtonClick ["CityPanelGameObject"] = gameObject;
        //		settingButtonClick ["userInfo"] = userInfo;
        //		settingButtonClick ["CityShopPanelGameObject"] = CityShopPanel.SingleTon ().gameObject;
        //
        //
        //		LuaFunction f = mgr.GetLuaFunction("Init");
        //		f.Call();

        //		Debug.Log(transform.Find ("root/UserInfo/Slider/UserName").name);
        //		if(shopButton)
        //			shopButton.onClick.AddListener(new UnityEngine.Events.UnityAction(OnShopButtonClick));
        //		if (battleButton)
        //			battleButton.onClick.AddListener (new UnityEngine.Events.UnityAction(OnBattleButtonClick));
        //		if (buildingInfoButton)
        //			buildingInfoButton.onClick.AddListener (new UnityEngine.Events.UnityAction(OnBuildingInfoButton));
        //		if (settingBtn)
        //			settingBtn.onClick.AddListener (new UnityEngine.Events.UnityAction(OnSettingButtonClick));
        //		instance = this;
    }

	void Start(){
		if(confirmNo)
			confirmNo.onClick.AddListener(new UnityEngine.Events.UnityAction(BuildingController.SingleTon().CancelPlace));
		if (confirmYes)
			confirmYes.onClick.AddListener (new UnityEngine.Events.UnityAction (BuildingController.SingleTon().Place));

//		userName.text=userInfo.userName;
//		userLevel.text=userInfo.userLevel.ToString();
//		userExp.text=userInfo.userExp.ToString();
//		workNum.text=userInfo.workNum.ToString();
//		System.DateTime dt = new System.DateTime (userInfo.protectTime);
//
//
//		protectTime.text = userInfo.protectTime / 3600 + "小时" + userInfo.protectTime % 3600 / 60 + "分钟";
//		maxCorn.text=userInfo.maxCorn.ToString();
//		currentCorn.text=userInfo.currentCorn.ToString();
//		maxWood.text = userInfo.maxWood.ToString();
//		currentWood.text=userInfo.currentWood.ToString();
//		maxStone.text=userInfo.maxStone.ToString();
//		currentStone.text=userInfo.currentStone.ToString();
//		currentBaoShi.text=userInfo.currentBaoShi.ToString();
	}


	Vector3 mPos;
	public Vector3 posOffset;
	void LateUpdate(){
		if (confirmBtns && BuildingController.SingleTon ().currentBuilding) {
			Vector3 screenPos = Camera.main.WorldToScreenPoint(BuildingController.SingleTon ().currentBuilding.transform.position);
			mPos = CityController.SingleTon().cameraUI.ScreenToWorldPoint (screenPos) + new Vector3(posOffset.x * (screenPos.x-Screen.width/2) / Screen.width/2, posOffset.y * Mathf.Cos(CameraController.SingleTon().rtsCamera.Tilt / 90) , 0);
			mPos.z = 0;
			confirmBtns.position = mPos;
		}
	}

	LuaState settingButtonClick;
	public void OnSettingButtonClick(){
//		LuaFunction f = settingButtonClick.GetFunction("OnSettingButtonClick");
//		object[] r = f.Call(gameObject);
	}

	public void OnShopButtonClick()
	{
		CityShopPanel.SingleTon ().root.SetActive (true);
		root.SetActive (false);
	}

	public void OnBattleButtonClick()
	{
		MoveOutMsgButtons ();
	}

	public void OnBuildingInfoButton()
	{
		root.SetActive (false);
		CityBuildingInfoPanel_I.SingleTon ().root.SetActive (true);
		CityBuildingInfoPanel_I.SingleTon ().SetBuildingInfo (BuildingController.SingleTon().currentBuilding.GetComponent<CityBuilding>());
	}

	public void ShowBuildingButtons(){
		RepositionMsgButtons ();
	}


	List<Button> availMsgButtons;
	public void RepositionMsgButtons()
	{
		for(int i=0;i<msgButtons.Count;i++)
		{
//			msgButtons[i].rectTransform(). transform.position = new Vector3(msgButtonCenter.position.x + (msgButtonSize.x * (1-(msgButtons.Count-1)/2)),  msgButtonCenter.position.y,0);
			msgButtons[i].transform.position= new Vector3(msgButtonCenter.position.x + (msgButtonSize.x * (i-(msgButtons.Count-1)/2)),  msgButtonCenter.position.y,msgButtonCenter.position.z);
			UTweenPosition tp = msgButtons[i].GetComponent<UTweenPosition>();
			if(tp)
			{
				tp.delay = msgIntervalDelay * i;
				tp.startPos = msgButtons[i].transform.localPosition;
				tp.endPos = msgButtons[i].transform.localPosition + msgTweenOffset;
				tp.PlayForward();
			}
		}
	}

	public void MoveOutMsgButtons()
	{
		for(int i=0;i<msgButtons.Count;i++)
		{
			UTweenPosition tp = msgButtons[i].GetComponent<UTweenPosition>();
			if(tp)
			{
				tp.PlayRevert();
			}
		}
	}




}
