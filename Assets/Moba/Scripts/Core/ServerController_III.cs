using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
public enum ApplicationType{Server,Client,None};
public class ServerController_III : NetworkManager {

	public ApplicationType appType = ApplicationType.None;

	public bool isAIMode;
	public static bool isAutoStartServer;
	public static bool isAutoClient;
	public static string targetIP;

	public List<GameObject> buildPrefabs;
	public List<GameObject> buildPrefabs1;
	public Dictionary<int,List<GameObject>> builds = new Dictionary<int, List<GameObject>>();


	public List<Transform> planes0;
	public List<Transform> spawnPoints0; 
	public List<Transform> availablePlanes0; 

	public List<Transform> planes1;
	public List<Transform> spawnPoints1;
	public List<Transform> availablePlanes1;

	public List<SpawnPoint> spawners0;
	public List<SpawnPoint> spawners1;

	public Transform defaultTarget0;//这两个最好在可寻路网格上面
	public Transform defaultTarget1;

	public UnitBase boss0;
	public UnitBase boss1;

	public int layer0 = 16;
	public int layer1 = 17;

	public int buildingLayer0 = 18;
	public int buildingLayer1 = 19;

	public List<PlayerController_III> players;
	public List<PlayerAttribute> playerAttributes;


	public AIController_III aiController;
	public static ServerController_III instance;

	public bool isBattleBegin = false;
	public int battleBeginDelay = 5;
	public float cameraHandleSpeed = 20;
	public float cameraHandleSpeedForMobile = 1;

	const string  KEY_IP_ADRESS = "address";

	void Awake()
	{
		instance = this;
		players.Add (null);
		players.Add (null);
		playerAttributes = new List<PlayerAttribute> ();
		playerAttributes.Add (new PlayerAttribute());
		playerAttributes.Add (new PlayerAttribute());
		availablePlanes0.AddRange (planes0);
		availablePlanes1.AddRange (planes1);
		if (boss0 != null)
			boss0.onDead += PlayerWin;
		if(boss1!=null)
			boss1.onDead += PlayerWin;

		builds.Add (0, buildPrefabs);
		builds.Add (1, buildPrefabs1);
		if(PlayerPrefs.HasKey(KEY_IP_ADRESS)){
			this.networkAddress = PlayerPrefs.GetString(KEY_IP_ADRESS);
		}
	}

	void Start(){
		switch(appType)
		{
			case ApplicationType.Server:
			GetComponent<NetworkManagerHUD>().enabled = false;
			this.StartServer();
			break;
			case ApplicationType.Client:
			GetComponent<NetworkManagerHUD>().enabled = false;
			this.StartClient();
			break;
		}
		if (isAutoStartServer) {
			StartHost ();
		} else if(isAutoClient){
			this.networkAddress = targetIP;
			StartClient();
		}	
	}


#if ServerDefine
	public GameObject groundPrefab;
	public GameObject planePrefab;
	public GameObject treePrefab;
		 
	void LoadSenceObjects(){

	}
#endif


	float mNextMoneyTime = 0;//下一次加钱时间
	float mMoneyInterval = 1;//加钱时间间隔
	public int moneyPerTips = 36;//每一跳加钱数
	void Update(){
		if(NetworkServer.active)
		{
			if(!isBattleBegin)
			{
				if(isAIMode)
				{
					if(players[0]!=null && players[0].isReady)
					{
						BattleBegin();
					}
				}
				else
				{
					if(players[0]!=null && players[0].isReady && players[1]!=null && players[1].isReady )
					{
						BattleBegin();
					}
				}
				return;
			}
			else
			{
				//加钱
				if(mNextMoneyTime < Time.time)
				{
					foreach(PlayerAttribute pa in playerAttributes)
					{
						pa.corn += moneyPerTips;
					}
					foreach(PlayerController_III pc in players)
					{
						if(pc!=null)pc.RefreshCorn();
					}

					mNextMoneyTime = Time.time + mMoneyInterval;
				}
			}
			if(nextSpawnTime < Time.time)
			{
				nextSpawnTime = Time.time + spawnInterval;
				SpawnSoilders (spawners0,spawnPoints0,defaultTarget1,layer0,layer1,playerAttributes[0],0);
				SpawnSoilders (spawners1,spawnPoints1,defaultTarget0,layer1,layer0,playerAttributes[1],1);
				for(int i=0;i<players.Count;i++)
				{
					if(players[i]!=null)players[i].ChangeTimeLimit(spawnInterval);
				}
			}
		}
	}

	void BattleBegin(){
		isBattleBegin = true;
		if(players[0]!=null)
			players[0].SendBattleBegin();
		if(players[1]!=null)
			players[1].SendBattleBegin();
	}	

	public override void OnStartServer ()
	{
		base.OnStartServer ();
	}

	public override void OnStartClient (NetworkClient client)
	{
		base.OnStartClient (client);
		GetComponent<AudioSource> ().enabled = true;
		PlayerPrefs.SetString (KEY_IP_ADRESS,this.networkAddress);
		PlayerPrefs.Save ();
	}

	public override void OnStopClient ()
	{
		base.OnStopClient ();
//		UnityEngine.SceneManagement.SceneManager.LoadScene ("Battle");
//		Application.LoadLevel("Battle");
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		GameObject player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		PlayerController_III playerControll = player.GetComponent<PlayerController_III>();
		if (players [0] == null) {
			playerControll.buildingLayer = 18;
			playerControll.playerAttribute = playerAttributes[0];
			playerControll.SendPlayerIndex(0);
			players [0] = playerControll;
			if(players [1]!=null)
			{
				players [1].SendPlayerInfoMsg();
			}
		} else if (players [1] == null && !isAIMode) {
			playerControll.buildingLayer = 19;
			playerControll.playerAttribute = playerAttributes[1];
			playerControll.SendPlayerIndex(1);
			players[1] = playerControll;
			if(players [0]!=null)
			{
				players [0].SendPlayerInfoMsg();
			}
		}
	}

	public void SendChatMsg(string msg){
		if(players[0]!=null)
		{
			players [0].RecieveChat(msg);
		}
		if(players[1]!=null)
		{
			players [1].RecieveChat(msg);
		}
	}

	public void AddAI(int race){
		if (players [1] != null || isAIMode)
			return;
		isAIMode = true;
		GameObject go = new GameObject ("_AI");
		AIController_III ai = go.AddComponent<AIController_III> ();
		ai.race = race;
		this.aiController = ai;
		ai.playerIndex = 1;
		ai.playerAttribute = playerAttributes [1];
		foreach(PlayerController_III pc in players)
		{
			if(pc!=null)
				pc.SendPlayerInfoMsg();
		}
	}

	public override void OnStopServer ()
	{
		base.OnStopServer ();
		NetworkServer.Reset ();
		if(ServerController_III.instance!=null)
			Destroy (ServerController_III.instance.gameObject);
		Time.timeScale = 1;
		SceneUtility.LoadServerManage ();
	}

	public void PlayerWin(UnitBase ub){
		int winIndex = -1;
		if (ub == boss0) {
			winIndex = 1;
		} else if(ub==boss1){
			winIndex = 0;
		}
		Debug.Log (winIndex);
		if(winIndex!=-1)
		{
			if(players[0]!=null)
			{
				players[0].SendPlayerWin(winIndex);
			}
			if(players[1]!=null)
			{
				players[1].SendPlayerWin(winIndex);
			}
		}
		Time.timeScale = 0;
	}

	IEnumerator _RestartServer(){
		yield return new WaitForSeconds (4);
		StopServer ();
	}

	public float spawnInterval = 10;
	float nextSpawnTime = 0;
	void SpawnSoilders(List<SpawnPoint> spawners,List<Transform> spTrans,Transform target,int layer,int targetLayer,PlayerAttribute playerAttribute,int playerIndex)
	{
		for(int i=0;i<spawners.Count;i++)
		{
			GameObject prefab = spawners[i].GetCurrentPrefab();
			if(prefab!=null)
			{
				prefab.SetActive (false);
				GameObject go = Instantiate(prefab,spawners[i].spawnPoint.position, spawners[i].spawnPoint.rotation) as GameObject;
				Enemy soilder = go.GetComponent<Enemy>();
				soilder.defaultTarget = target;
				soilder.pos = spawners[i].spawnPoint.position;
				soilder.qua = spawners[i].spawnPoint.rotation;
				go.layer = layer;
				soilder.targetLayers.Add(targetLayer);
				soilder.playerAttribute = playerAttribute;
				soilder.playerIndex = playerIndex;
				go.SetActive (true);
				StartCoroutine(_DelayMove(soilder,target));
				NetworkServer.Spawn(go);
			}
		}
	}

	IEnumerator _DelayMove(Enemy enemy,Transform target)
	{
		UnityEngine.AI.NavMeshAgent nav = enemy.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		nav.enabled = false;
		yield return new WaitForSeconds(1);
		nav.enabled = true;
	}

	//建造建筑
	public void SpawnBuilding(int playerIndex,int buildIndex,int planeIndex,int race)
	{
		if (!isBattleBegin)
			return;
		List<GameObject> currentBuildPrefabs = builds[race];
	
		int price = currentBuildPrefabs[buildIndex].GetComponent<SpawnPoint>().GetCurrentPrice();
		if(playerAttributes[playerIndex].corn < price)
		{
			if(players[playerIndex]!=null)
				players[playerIndex].BuildFail();
		}
		else
		{
			playerAttributes[playerIndex].corn -= price;
			if(players[playerIndex]!=null)
				players[playerIndex].RefreshCorn();
			if (playerIndex == 0) {
				SpawnBuilding(planes0,availablePlanes0,planeIndex,buildIndex,spawners0,buildingLayer0,playerIndex,currentBuildPrefabs);
			}else if (playerIndex == 1) {
				SpawnBuilding(planes1,availablePlanes1,planeIndex,buildIndex,spawners1,buildingLayer1,playerIndex,currentBuildPrefabs);
			}
		}
	}

	public void UpgradeBuilding(int index,int order,int group){
		if (!isBattleBegin)
			return;
		SpawnPoint building = null;
		if (group == 0) {
			building = spawners0 [index];
		} else {
			building = spawners1[index];
		}
		if (building != null && building.level < 3) {

//			if(players[group]!=null)
//			{
				//验证一下是否有下一级的建筑
				if(building.leveledSoilderPrefabs.Count <= building.level + 1)
				{
					return;
				}
				int price;
				if(building.GetCurrentPrice(building.level + 1,order,out price))
				{
					if(playerAttributes[group].corn < price)
					{
						if(players[group]!=null)
							players[group].BuildFail();
					}
					else
					{
						playerAttributes[group].corn -= price;
						if(players[group]!=null)
							players[group].RefreshCorn();
						building.Upgrade();
					}
				}
//			}
		}
	}

	void SpawnBuilding(List<Transform> planes,List<Transform> availablePlanes,int planeIndex,int buildIndex,List<SpawnPoint> spawner,int buildingLayer,int group,List<GameObject> cBuildPrefabs)
	{
		Transform plane = planes[planeIndex];
		if(!availablePlanes.Contains(plane))
		{
			return;
		}
		availablePlanes.Remove (plane);
		Transform sp = plane.transform.Find("SpawnPoint");
		GameObject go = Instantiate(cBuildPrefabs[buildIndex],plane.position,plane.rotation) as GameObject;
		SpawnPoint spawnPoint = go.GetComponent<SpawnPoint> ();
		spawnPoint.spawnPoint = sp;
		spawnPoint.gameObject.layer = buildingLayer;
		spawnPoint.group = group;
		spawner.Add (spawnPoint);
		spawnPoint.index = spawner.IndexOf (spawnPoint);
		NetworkServer.Spawn (go);
	}

	public void ClientReady(PlayerController_III player,int race){
		int index = players.IndexOf (player);
		player.race = race;
		if(index!=-1)
		{
			foreach(PlayerController_III p in players)
			{
				if(p != null)
				{
					p.SendPlayerInfoMsg();
				}
			}
		}
	}

	public bool showGUI = false;
	void OnGUI()
	{
		if(!showGUI)
		{
			return;
		}
	}

}