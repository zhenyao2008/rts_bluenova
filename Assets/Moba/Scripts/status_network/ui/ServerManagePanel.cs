using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerManagePanel : MonoBehaviour {

	public Button btn_create;
	public Button btn_join;

	public GameObject itemPrefab;
	public GameObject txt_search;
	public Transform listParent;

	Dictionary<string,GameObject> mServerBtns;
	float mCheckIpInterval = 1;
	float mNextCheckTime;
	string[] mServerNames;
	Button mCurrentBtn;
	public static string targetIp;

	void Awake ()
	{
		mServerBtns = new Dictionary<string, GameObject> ();
		mServerNames = GetServerNames ();
		DisableBtnJoin ();
		ServerController_III.isAutoStartServer = false;
		ServerController_III.isAutoClient = false;
		btn_create.onClick.AddListener (()=>{
			GetComponent<HostMessageReciever>().StopReceive();
			ServerController_III.isAutoStartServer = true;
			SceneUtility.LoadBattle();
		});
		btn_join.onClick.AddListener (()=>{
			GetComponent<HostMessageReciever>().StopReceive();
			ServerController_III.isAutoClient = true;
			ServerController_III.targetIP = targetIp;
			SceneUtility.LoadBattle();
		});
	}

	void DisableBtnJoin(){
		btn_join.GetComponent<Image> ().color = Color.gray;
		btn_join.GetComponentInChildren<Text>().color =  Color.gray;
		btn_join.enabled = false;
	}

	void EnableBtnJoin(){
		btn_join.GetComponent<Image> ().color = Color.white;
		btn_join.GetComponentInChildren<Text>().color =  Color.white;
		btn_join.enabled = true;
	}

	void Update ()
	{
		if (mNextCheckTime < Time.time) {
			mNextCheckTime = Time.time + mCheckIpInterval;
			foreach (string ip in HostMessageReciever.ips.Keys) {
				if (!mServerBtns.ContainsKey (ip)) {
					GameObject go = Instantiate (itemPrefab);
					Text text = go.GetComponentInChildren<Text> (true);
					string serverName = mServerNames[0];
					text.text = serverName;
					go.transform.SetParent (listParent);
					go.transform.localScale = Vector3.one;
					go.transform.localPosition = Vector3.zero;
					go.SetActive (true);
					mServerBtns.Add (ip, go);
					txt_search.SetActive (false);
					Button btn = go.GetComponent<Button> ();
					btn.onClick.AddListener (()=>{
						if(mCurrentBtn!=null){
							mCurrentBtn.GetComponent<Image>().color = Color.white;
						}
						mCurrentBtn = btn;
						mCurrentBtn.GetComponent<Image>().color = new Color(98f/255,1,98f/255);
						EnableBtnJoin();
						targetIp = ip;
						Debug.Log("targetIp:" + targetIp);
					});
				}
			}
		}
	}

	void OnDisable(){
		HostMessageReciever.Instance.StopReceive ();
	}

	string[] GetServerNames(){
		string[] serverNames = new string[]{
			"トゥルーマンの世界",//灵感来自于The Truman Show。
			"费尔威泽(Felwithe)",
			"自由港(Freeport)",
			"克勒辛(Kelethin)",
			"奎诺斯(Qeynos)",
			"大河谷(Rivervale)",
			"费尔威泽(Felwithe)",
			"自由港(Freeport)",
			"奎诺斯(Qeynos)",
			"卡比利斯(Cabilis)",
			"奥格克(Oggok)",
			"格洛波(Grobb)",
			"克勒辛(Kelethin)",
			"哈勒斯(Halas)",
			"尼瑞克(Neriak)",
			"卡拉丁(Kaladim)",
			"艾露丁(Erudin)",
			"阿克农(Ak'Anon)"};
		return serverNames;
	}
}
