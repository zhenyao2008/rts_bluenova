using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public delegate void WWWCallBack(WWW www);
public class MobaNetworkManager : MonoBehaviour {

	public static string urlBase =  "http://localhost:8084/MobaWebServer";
	public static string sessionId = "";

	static MobaNetworkManager instance;
	public static MobaNetworkManager SingleTon(){
		if(instance == null)
		{
			GameObject go = new GameObject("_NetworkManager");
			instance = go.AddComponent<MobaNetworkManager>();
		}
		return instance;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F))
		{
			GetBuildingInfos(CommonCallBack);
		}
	}

	public void GetUserBuilding(WWWCallBack callBack){
		string url = urlBase + "/UserBuildingServlet" + sessionId;
		Debug.Log (url);
		WWW www = new WWW (url);
		WWWForm form = new WWWForm ();
	
		StartCoroutine (WaitWWW(www,callBack));
	}

	public void GetBuildingInfos(WWWCallBack callBack){
		string url = urlBase + "/BuildInfoServlet" + sessionId;
		WWW www = new WWW (url);
		WWWForm form = new WWWForm ();
		StartCoroutine (WaitWWW(www,callBack));
	}

	public void CommonCallBack(WWW www)
	{
		Debug.Log ("Error: " + www.error);
		Debug.Log ("Text: " + www.text);
	}

	public void Login(string userName,string password,WWWCallBack callBack)
	{
		UserInfo userInfo = new UserInfo ();
		userInfo.userName = userName;
		userInfo.password = password;
		string json = JsonConvert.SerializeObject(userInfo);
		Debug.Log (json);
		WWWForm form = new WWWForm ();
		form.AddField ("user",json);
		string url = urlBase + "/LoginServlet";
		WWW www = new WWW (url,form);
		StartCoroutine (WaitWWW(www,callBack));
	}

	IEnumerator WaitWWW(WWW www,WWWCallBack callBack)
	{
		yield return www;
		Debug.Log(www.text);
		if (www.error != null) {
			Debug.LogError(www.error);
		} else {
			callBack(www);
		}
	}

//	IEnumerator WaitForUserLogin(WWW www,WWWCallBack callBack)
//	{
//
//		yield return www;
//		Debug.Log(www.text);
//		if (www.error != null) {
//			Debug.LogError(www.error);
//		} else {
//			UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(www.text);
//			if(userInfo!=null)
//			{
////				Debug.Log("Login Success");
////				DataCenter.Instance().userInfo = userInfo;
////				Application.LoadLevel(cityLevel);
//			}
//			else
//			{
//				Debug.LogError("Login Fail");
//			}
//		}
//	}


}
