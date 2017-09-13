using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ServerController_II : NetworkManager {

	public static ServerController_II instance;
	
	public static ServerController_II GetInstance(){
		if(instance == null)
		{
			instance = FindObjectOfType<ServerController_II>();
		}
		return instance;
	}

//	public PlayerController playerController;

	public List<GameObject> bulletPrefabs;

	public GameObject soldierPrefab;
	public GameObject soldierArcherPrefab;
	public GameObject soldierBallistaPrefab;
	public GameObject soldierMagicPrefab;
	public GameObject soldierCavalryPrefab;
	public GameObject soldierSpearmenPrefab;

	public GameObject soldierPrefab1;
	public GameObject soldierArcherPrefab1;
	public GameObject soldierBallistaPrefab1;
	public GameObject soldierMagicPrefab1;
	public GameObject soldierCavalryPrefab1;
	public GameObject soldierSpearmenPrefab1;

	public Transform town0;
	public Transform town1;

	public Transform[] soldierSpawnPoints0;
	public Transform[] soldierArcherSpawnPoints0;
	public Transform[] soldierBallistaSpawnPoints0;
	public Transform[] soldierMagicPoints0;
	public Transform[] soldierCavalryPoints0;
	public Transform[] soldierSpearmenPoints0;


	public Transform[] soldierSpawnPoints1;
	public Transform[] soldierArcherSpawnPoints1;
	public Transform[] soldierBallistaSpawnPoints1;
	public Transform[] soldierMagicPoints1;
	public Transform[] soldierCavalryPoints1;
	public Transform[] soldierSpearmenPoints1;

	public int soldierLevel0 = 1;
	public int soldierLevel1 = 10;


	public GameObject playerPrefabGo;
	public GameObject[] playerPrefabGo0;
	public GameObject[] playerPrefabGo1;


	public Transform[] playerSpawnPoints0;
	public Transform[] playerSpawnPoints1;

	public List<PlayerII> players0;
	public List<PlayerII> players1;

	public List<PlayerII> players;

	public PlayerSelect playerSelect;


	public float soldierSpawnInterval = 30;

	public bool isServer;
	public  bool isClient;

	public LoadTowers loadTowers0;
	public LoadTowers loadTowers1;

	public PlayerII coreHero0;
	public PlayerII coreHero1;


	public const int groupLayer0 = 16;
	public const int groupLayer1 = 17;

	void Awake()
	{
//		NetworkClient.ShutdownAll ();
	}

	void Start(){
//		if (NetworkBattleServer.instance != null) 
//		{
//			this.networkPort = NetworkBattleServer.instance.localServerPort;
//			isServer = true;
//			isClient = false;
//		}
		if (isServer) {
			//			InitializeClient();
			this.StartServer();
		}
		if (isClient) {
			//			this.networkPort = NetworkMasterClient.battleServerPort;
			this.StartClient();
			GetComponent<AudioSource>().enabled = true;
		}
	}





//	NetworkClient mMasterClient;
//	string mMasterServerIp = "127.0.0.1";
//	int mMasterServerPort=43333;
//	public void InitializeClient()
//	{
//		mMasterClient = new NetworkClient();
//		mMasterClient.Connect(mMasterServerIp, mMasterServerPort);
//
//		mMasterClient.RegisterHandler(MsgType.Connect, OnClientConnect);
//		mMasterClient.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
//		mMasterClient.RegisterHandler(MsgType.Error, OnClientError);

//	}
//
//	public void OnClientConnect(NetworkMessage netMsg)
//	{
//		Debug.Log("Master received client");
//		RequestBattleServerPort ();
//	}
//
//	public void OnLaunchedBattle(NetworkMessage netMsg){
//		var msg = netMsg.ReadMessage<MasterMsgTypes.LaunchedBattleMessage>();
//		this.networkPort = msg.port;
//		Application.LoadLevel ("BattlePVE");
//	}
//
//	public void RequestBattleServerPort(){
//		mMasterClient.Send (MasterMsgTypes.RequestBattleServerPortMessageId, new  MasterMsgTypes.RequestBattleServerPortMessage ());
//	}
//
//	public void OnResponseBattleServerPort(NetworkMessage netMsg){
//		var msg = netMsg.ReadMessage<MasterMsgTypes.ResponseBattleServerPortMessage>();
//		this.networkPort = msg.port;
//		this.StartServer();
//		OnBattleServerReady ();
//	}
//
//	public void OnBattleServerReady(){
//		MasterMsgTypes.BattleServerReadyMessage msg = new MasterMsgTypes.BattleServerReadyMessage ();
//		msg.port = this.networkPort;
//		mMasterClient.Send (MasterMsgTypes.BattleServerReadyMessageId,msg);
//	}

	public PlayerII SelectPlayer(int index,int preIndex){
		if(preIndex!=-1)playerSelect.ActiveButton (preIndex);
		playerSelect.InActiveButton (index);

		return players [index];
	}

	public void OnPlayerDeSelect(PlayerII player){
		int index = players.IndexOf (player);
		playerSelect.actives.Remove (index);
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		Debug.Log ("OnStartServer");
		StartCoroutine (_Spawn());
		SpawnPlayers (playerPrefabGo0,playerSpawnPoints0,ref players0,groupLayer1,groupLayer0);
		SpawnPlayers (playerPrefabGo1,playerSpawnPoints1,ref players1,groupLayer0,groupLayer1);
		players.AddRange (players0);
		players.AddRange (players1);
		loadTowers0.Load ();
		loadTowers1.Load ();
		if (coreHero0 != null)
			coreHero0.onDead += OnGroup1Win;
		if (coreHero1 != null)
			coreHero1.onDead += OnGroup0Win;
		if (NetworkBattleServer.instance != null) {
//			NetworkBattleServer.instance.BattleServerReady();
		}
	}

	void OnGroup0Win(UnitBase ub){
		Debug.Log ("OnGroup0Win");
		BattleEnd (0);
	}

	void OnGroup1Win(UnitBase ub){
		Debug.Log ("OnGroup0Win");
		BattleEnd (1);
	}

	public void BattleEnd(int side){
		Debug.Log ("RpcBattleEnd");
		PlayerController[] pcs = FindObjectsOfType<PlayerController> ();
		Debug.Log (pcs.Length);
		foreach(PlayerController playerController in pcs)
		{
			if (playerController == null || playerController.player == null)
				return;
			if (side == 0 && playerController.player.gameObject.layer == 16) {
				playerController.ShowBattleEnd (0);
			} else if (side == 1 && playerController.player.gameObject.layer == 17) {
				playerController.ShowBattleEnd (0);
			} else {
				playerController.ShowBattleEnd (1);
			}
		}
		StartCoroutine (_ReStart());
	}

	IEnumerator _ReStart()
	{
		yield return new WaitForSeconds (5);
		NetworkServer.Shutdown ();
		NetworkServer.Reset ();
		Application.LoadLevel ("BattlePVE");
		Destroy (gameObject);
	}


	void SpawnPlayers(GameObject[] prefabs, Transform[] points,ref List<PlayerII> players,int targetLayer,int layer){
		for(int i = 0;i < points.Length;i ++)
		{
			GameObject go = Instantiate(prefabs[i],points[i].position,points[i].rotation) as GameObject;
			PlayerII playerII = go.GetComponent<PlayerII>();
			playerII.targetLayers.Add(targetLayer);
			playerII.gameObject.layer = layer;
			playerII.defaultSpawnPos = points[i].position;
			if(i<=1)
			{
				playerII.bulletPrefab = bulletPrefabs[0];
				playerII.mAttackInterval = 0.1f;
			}else if(i<=3)
			{
				playerII.bulletPrefab = bulletPrefabs[2];
				playerII.mAttackInterval = 0.8f;
			}else
			{
				playerII.bulletPrefab = bulletPrefabs[3];
				playerII.mAttackInterval = 0.4f;
			}

			players.Add(playerII);
			NetworkServer.Spawn(go);
		}

	}

	IEnumerator _Spawn(){
		while(true)
		{
			SpawnEnemy(soldierPrefab,soldierSpawnPoints0,town1,groupLayer1,groupLayer0,soldierLevel0);
			SpawnEnemy(soldierPrefab1,soldierSpawnPoints1,town0,groupLayer0,groupLayer1,soldierLevel1);

			SpawnEnemy(soldierArcherPrefab,soldierArcherSpawnPoints0,town1,groupLayer1,groupLayer0,soldierLevel0);
			SpawnEnemy(soldierArcherPrefab1,soldierArcherSpawnPoints1,town0,groupLayer0,groupLayer1,soldierLevel1);

			SpawnEnemy(soldierBallistaPrefab,soldierBallistaSpawnPoints0,town1,groupLayer1,groupLayer0,soldierLevel0);
			SpawnEnemy(soldierBallistaPrefab1,soldierBallistaSpawnPoints1,town0,groupLayer0,groupLayer1,soldierLevel1);

			SpawnEnemy(soldierCavalryPrefab,soldierCavalryPoints0,town1,groupLayer1,groupLayer0,soldierLevel0);
			SpawnEnemy(soldierCavalryPrefab1,soldierCavalryPoints1,town0,groupLayer0,groupLayer1,soldierLevel1);

			SpawnEnemy(soldierMagicPrefab,soldierMagicPoints0,town1,groupLayer1,groupLayer0,soldierLevel0);
			SpawnEnemy(soldierMagicPrefab1,soldierMagicPoints1,town0,groupLayer0,groupLayer1,soldierLevel1);

			SpawnEnemy(soldierSpearmenPrefab,soldierSpearmenPoints0,town1,groupLayer1,groupLayer0,soldierLevel0);
			SpawnEnemy(soldierSpearmenPrefab1,soldierSpearmenPoints1,town0,groupLayer0,groupLayer1,soldierLevel1);
			yield return new WaitForSeconds(soldierSpawnInterval);
		}
	}


	void SpawnEnemy(GameObject prefab,Transform[] spawnPoints,Transform moveTarget,int targetLayer,int layer,int level)
	{
		for(int i =0;i<spawnPoints.Length;i++)
		{
			GameObject go = Instantiate (prefab);
			go.transform.position = spawnPoints[i].position;
			Enemy enemy = go.GetComponent<Enemy> ();
			enemy.targetLayers.Add(targetLayer);
			enemy.pos = spawnPoints[i].position;
			enemy.gameObject.layer = layer;
			enemy.defaultTarget = moveTarget;
			UnitAttribute unitAttibute = go.GetComponent<UnitAttribute>();
			unitAttibute.currentHealth *= level;
			unitAttibute.maxHealth *= level;
			NetworkServer.Spawn (go);
			StartCoroutine(_DelayMove(enemy,moveTarget.position));
		}
	}

	IEnumerator _DelayMove(Enemy enemy,Vector3 targetPos)
	{
		UnityEngine.AI.NavMeshAgent nav = enemy.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		enemy.enabled = false;
		nav.enabled = false;
		yield return new WaitForSeconds(1);
		enemy.enabled = true;
		nav.enabled = true;
//		enemy.Move (targetPos);
	}

	void OnGUI(){
		if(NetworkServer.active)
		{
			if(GUI.Button(new Rect(10,10,100,30),"ReSet!"))
			{
				StartCoroutine(_ReStart());
			}
		}

	}


}
