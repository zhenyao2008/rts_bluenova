using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public class LoginController : MonoBehaviour {

	public Button confirmButton;
	public InputField userName;
	public InputField userPassword;

	public static string urlBase =  "http://localhost:8084/MobaWebServer";
	public static string cityLevel = "City";


	void Awake()
	{
		confirmButton.onClick.AddListener (Login);
		userName.text = "Tester";
		userPassword.text = "123456";
//		if (ES2.Exists ("userName")) {
//			DataCenter.Instance ().LoadUserInfo ();
//			Application.LoadLevel("City");			
//		}
	}

	void Init()
	{
		MobaNetworkManager.SingleTon ().GetBuildingInfos (GetBuildingInfoCallBack);
		MobaNetworkManager.SingleTon ().GetUserBuilding (GetUserBuildingCallBack);
	}

	void GetBuildingInfoCallBack(WWW www){
		DataCenter.Instance().buildingInfoList = JsonConvert.DeserializeObject<List<BuildingInfo>>(www.text);
		Debug.Log (DataCenter.Instance().buildingInfoList.Count);
	}

	void GetUserBuildingCallBack(WWW www){
		DataCenter.Instance().userBuildingList = JsonConvert.DeserializeObject<List<UserBuilding>>(www.text);
		Debug.Log (DataCenter.Instance().userBuildingList.Count);
	}

	void LoginCallBack(WWW www){
		UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(www.text);
		MobaNetworkManager.sessionId = www.responseHeaders["SET-COOKIE"].Substring(0,www.responseHeaders["SET-COOKIE"].IndexOf(";"));

		Debug.Log (MobaNetworkManager.sessionId );
		DataCenter.Instance().userInfo = userInfo;
		Init ();
//		Application.LoadLevel(cityLevel);
	}

	void Login()
	{
		MobaNetworkManager.SingleTon ().Login (userName.text,userPassword.text,LoginCallBack);
	}

}
