using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
public class PlayerController_III :  NetworkBehaviour,IPlayerController {

	public int planeLayer = 22;
	public int groundLayer = 15;
	public int buildingLayer;
	public int race = 0;

	public bool isReady;
	public bool isBattleBegin;
	public bool isChating;

	ServerController_III mServerController_III;

	GameObject selectPreBuilding;
	GameObject selectBuilding;
	int mBuildOrder;


	int selectBuildingIndex;
	int mBuildingLayer;

	BuildingPanel mBuildingPanel;
	PlayerPanel_III mPlayerPanel;
	ServerMsgPanel mServerMsgPanel;
	ResultPanel mResultPanel;
	BuildInfoPanel mBuildInfoPanel;
	BuildUpgradePanel mBuildUpgradePanel;
	ChatPanel mChatPanel;
	RtsCamera rtsCamera;

	public int playerIndex = -1;

	public PlayerAttribute playerAttribute;

	void Start(){
		mServerController_III = ServerController_III.instance;
		if(isClient && isLocalPlayer)
		{
			rtsCamera = FindObjectOfType<RtsCamera>();
			mBuildingPanel = FindObjectOfType<BuildingPanel>();
			mPlayerPanel = FindObjectOfType<PlayerPanel_III>();
			mServerMsgPanel =  FindObjectOfType<ServerMsgPanel>();
			mResultPanel = FindObjectOfType<ResultPanel>();
			mBuildInfoPanel = FindObjectOfType<BuildInfoPanel>();
			mBuildUpgradePanel = FindObjectOfType<BuildUpgradePanel>();
			mChatPanel = FindObjectOfType<ChatPanel>();
			mBuildInfoPanel.upgradeBtn.onClick.Add(new global::EventDelegate(ShowUpgrade));
			mBuildInfoPanel.upgradeBtn1.onClick.Add(new global::EventDelegate(ShowUpgrade1));
			mBuildUpgradePanel.upgradeBtn.onClick.Add(new global::EventDelegate(UpgradeBuildingServer));
			mChatPanel.controllBtn.onClick.Add(new global::EventDelegate(ShowChat));

			mChatPanel.closeBtn.onClick.Add(new global::EventDelegate(CloseChat));
			mChatPanel.controllBtn.onClick.Add(new global::EventDelegate(ShowChat));
			mChatPanel.sendBtn.onClick.Add(new global::EventDelegate(SendChat));
			mChatPanel.chatInput.onSubmit.Add(new global::EventDelegate(SendChat));
		}
	}

	public void CloseChat(){
//		rtsCamera.enabled = true;
//		isChating = false;
		mChatPanel.Hide ();
	}

	public void ShowChat(){
//		rtsCamera.enabled = false;
//		isChating = true;
		mChatPanel.Show ();
	}

	public void SendChat(){
		string msg = mChatPanel.chatInput.value.Trim();
		if(msg!="")
		{
			CmdSendChat(msg);
			mChatPanel.chatInput.value = "";
		}
	}


	[Command]
	public void CmdSendChat(string charMsg){
		mServerController_III.SendChatMsg (charMsg);
	}

	public void RecieveChat(string chatMsg){
		RpcRecieveChat (chatMsg);
	}

	[ClientRpc]
	public void RpcRecieveChat(string chatMsg){
		if(isLocalPlayer)mChatPanel.AddMsg (chatMsg);
	}

	[Command]
	public void CmdAddCoin(int corn){
		playerAttribute.corn += corn;
	}

	[ClientRpc]
	public void RpcSentChat(string msg){
		if(isLocalPlayer)
		{
			mChatPanel.AddMsg(msg);
		}
	}

	public int GetPlayerIndex(){
		return playerIndex;
	}

	public void CloseBuildingPanel(){
		mBuildingPanel.root.SetActive(false);
	}

	[Client]
	public void SelectBuilding(){
		int index = mBuildingPanel.buildings.IndexOf (UIEventTrigger.current);
		if(selectPreBuilding!=null)
			Destroy(selectPreBuilding);
		selectBuildingIndex = index;
		if (race == 0) {
			selectPreBuilding = Instantiate<GameObject> (ServerController_III.instance.buildPrefabs [selectBuildingIndex]);
		} else {
			selectPreBuilding = Instantiate<GameObject> (ServerController_III.instance.buildPrefabs1 [selectBuildingIndex]);
		}
		Debug.Log ("SelectBuilding");
		Destroy (selectPreBuilding.GetComponent<SpawnPoint> ());
		Destroy (selectPreBuilding.GetComponent<Collider> ());
//		SpawnPoint sp = selectPreBuilding.GetComponent<SpawnPoint> ();
//		GameObject go = sp.GetCurrentPrefab ();
//		UnitAttribute ua = go.GetComponent<UnitAttribute> ();
//		mBuildInfoPanel.SetBuildInfo (ua);
		mBuildInfoPanel.root.SetActive (false);
		mBuildingPanel.root.SetActive (false);
	}

	[Client]
	public void ShowBuildingInfo(){

		UIEventTrigger current = UIEventTrigger.current;
		Debug.Log (current.name);
		current = current.transform.parent.GetComponent<UIEventTrigger> ();
		Debug.Log (current.name);
		int index = mBuildingPanel.buildings.IndexOf (current);
		GameObject building;
		if (race == 0) {
			building = ServerController_III.instance.buildPrefabs [index];
		} else {
			building = ServerController_III.instance.buildPrefabs1 [index];
		}

		SpawnPoint sp = building.GetComponent<SpawnPoint> ();

		GameObject go = sp.GetCurrentPrefab ();
		UnitAttribute ua = go.GetComponent<UnitAttribute> ();
		mBuildInfoPanel.SetBuildInfo (ua);
		mBuildInfoPanel.ShowUpgrade (sp);

		mBuildInfoPanel.root.SetActive (true);
		mBuildingPanel.root.SetActive (false);

		mBuildInfoPanel.returnBtn.onClick.Clear ();
		mBuildInfoPanel.returnBtn.onClick.Add (new global::EventDelegate (ShowBuildPanel));

	}

	void ShowBuildPanel(){
		mBuildInfoPanel.root.SetActive (false);
		mBuildingPanel.root.SetActive (true);
	}


	public void ChangeTimeLimit(float timeLimit){
		RpcChangeTimeLimit (timeLimit);
	}

	[ClientRpc]
	public void RpcChangeTimeLimit(float timeLimit){
		if (isLocalPlayer) {
			StopCoroutine ("_TimeLimit");
			StartCoroutine ("_TimeLimit",timeLimit);
		}
	}

	IEnumerator _TimeLimit(float timeLimit){
		float targetTime = Time.time + timeLimit;
		float timeTips = 0.1f;
		float t = timeLimit;
		while(targetTime>Time.time)
		{
			t -= timeTips;
			mPlayerPanel.timeLimit.text = "Time " + string.Format("{0:f1}", t); ;
			yield return new WaitForSeconds(timeTips);
		}
	}
//	float cameraSpeed = 100;
	void LateUpdate(){
		if (!isLocalPlayer) {
			return;
		}
		if(mBuildInfoPanel.root.activeInHierarchy)
		{
			return;
		}
		if(mBuildingPanel.root.activeInHierarchy)
		{
			return;
		}
		if(Input.GetMouseButton(0))
		{
			Vector3 forward = this.rtsCamera.transform.forward;
			Vector3 right = this.rtsCamera.transform.right;
			forward.y = 0;
			right.y = 0;
			
			this.rtsCamera.LookAt -=mServerController_III.cameraHandleSpeed * forward.normalized * Input.GetAxis("Mouse Y") * Time.deltaTime;
			
			this.rtsCamera.LookAt -=mServerController_III.cameraHandleSpeed * right.normalized * Input.GetAxis("Mouse X") * Time.deltaTime;
		}
//		Debug.Log(Input.GetAxis("Mouse X"));
//		Debug.Log(Input.GetAxis("Mouse Y"));



	}

	void Update()
	{
		if(!isBattleBegin)
			return;
		if (!isLocalPlayer)
			return;
//		if(Input.GetKeyDown(KeyCode.H))
//		{
//			if(selectBuilding!=null)
//				Destroy(selectBuilding);
//			selectBuildingIndex = Random.Range(0,ServerController_III.instance.buildPrefabs.Count);
//			selectBuilding = Instantiate<GameObject>( ServerController_III.instance.buildPrefabs[selectBuildingIndex]);
//		}
		isChating = mChatPanel.chatInput.isSelected;//聊天框是否选中
		rtsCamera.enabled = !isChating;
		if(isChating)
			return;



		if(Input.GetMouseButtonDown(0) && !UICamera.isOverUI)
		{
			if(selectBuilding!=null)
			{
				selectBuilding.GetComponent<SpawnPoint>().OnUnSelected();
			}
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<buildingLayer))
			{
				selectBuilding = hit.transform.gameObject;
				mBuildInfoPanel.root.SetActive(true);
				mBuildingPanel.root.SetActive(false);
				SpawnPoint sp = selectBuilding.GetComponent<SpawnPoint>();
				sp.OnSelected();
				LevelPrefabs nextPrefabs;
				UnitAttribute ua = sp.GetCurrentPrefab().GetComponent<UnitAttribute>();
				mBuildInfoPanel.SetBuildInfo (ua);
				mBuildInfoPanel.ShowUpgrade(sp);
				if(sp.GetNextPrefabs(out nextPrefabs))
				{
					if(nextPrefabs.soilderPrefabs.Count>0)
					{
						mBuildInfoPanel.upgradeBtn.gameObject.SetActive(true);
						mBuildInfoPanel.upgradeBtn.GetComponentInChildren <UILabel>().text = "To:" + nextPrefabs.soilderPrefabs[0].GetComponent<UnitAttribute>().unitName;
					}
					if(nextPrefabs.soilderPrefabs.Count>1)
					{
						mBuildInfoPanel.upgradeBtn1.gameObject.SetActive(true);
						mBuildInfoPanel.upgradeBtn1.GetComponentInChildren<UILabel>().text = "To:" + nextPrefabs.soilderPrefabs[1].GetComponent<UnitAttribute>().unitName;
					}
				}
			}
			else if(selectPreBuilding==null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<planeLayer))
			{
				mBuildingPanel.root.SetActive(true);
				mBuildInfoPanel.root.SetActive(false);
			}else{
				mBuildingPanel.root.SetActive(false);
				mBuildInfoPanel.root.SetActive(false);
			}
		}

		if(selectPreBuilding!=null)
		{
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<planeLayer))
			{
				selectPreBuilding.transform.position = hit.transform.position;
			}else if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<groundLayer))
			{
				selectPreBuilding.transform.position = hit.point;
			}
		}

		if(selectPreBuilding!=null && Input.GetMouseButtonDown(0) )
		{
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<planeLayer))
			{
//				selectPreBuilding.transform.position = hit.transform.position;

				int planeIndex = -1;
				if(playerIndex==0) 
					planeIndex = mServerController_III.planes0.IndexOf(hit.transform);
				else
					planeIndex = mServerController_III.planes1.IndexOf(hit.transform);
				CmdPlaceBuilding(selectBuildingIndex,planeIndex);
			}
			Destroy(selectPreBuilding);
			
			selectPreBuilding = null;
		}


		if(Input.GetKeyDown(KeyCode.K))
		{
			CmdKillAllEnemys();
		}

		if(Input.GetKeyDown(KeyCode.H) && selectBuilding!=null)
		{
			UpgradeBuildingServer();
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			RpcRestartServer();
		}

		if(Input.GetKeyDown(KeyCode.G))
		{
			CmdAddCoin(100);
		}

	}

	[Client]
	public void MoveCamera(){
	
	}

	[ClientRpc]
	public void RpcRestartServer(){
		mServerController_III.OnStopServer ();
	}

	[Command]
	public void CmdKillAllEnemys(){
		Enemy[] enemys = FindObjectsOfType<Enemy>();
		foreach(Enemy enemy in enemys)
		{
			Destroy(enemy.gameObject);
		}
	}

	//杀死一个敌方单位
	public void OnKillEnemy(){
		RpcRefreshCorn (playerAttribute.corn,playerAttribute.killNum);
	}

	public void BuildFail(){
		RpcBuildFail ();
	}

	public void ShowUpgrade(){
		this.mBuildOrder = 0;
		SpawnPoint sp = selectBuilding.GetComponent<SpawnPoint>();
		LevelPrefabs nextPrefabs;
		UnitAttribute ua = sp.GetCurrentPrefab().GetComponent<UnitAttribute>();
		mBuildUpgradePanel.SetBuildInfo (ua);
		if(sp.GetNextPrefabs(out nextPrefabs))
		{
			if(nextPrefabs.soilderPrefabs.Count>0)
			{
				mBuildUpgradePanel.SetBuildInfo1 (nextPrefabs.soilderPrefabs[0].GetComponent<UnitAttribute>());;
			}
		}
		mBuildInfoPanel.root.SetActive (false);
		mBuildUpgradePanel.root.SetActive (true);
	}

	public void ShowUpgrade1(){
		this.mBuildOrder = 1;
		SpawnPoint sp = selectBuilding.GetComponent<SpawnPoint>();
		LevelPrefabs nextPrefabs;
		UnitAttribute ua = sp.GetCurrentPrefab().GetComponent<UnitAttribute>();
		mBuildUpgradePanel.SetBuildInfo (ua);
		if(sp.GetNextPrefabs(out nextPrefabs))
		{
			if(nextPrefabs.soilderPrefabs.Count>1)
			{
				mBuildUpgradePanel.SetBuildInfo1 (nextPrefabs.soilderPrefabs[1].GetComponent<UnitAttribute>());;
			}
		}
		mBuildInfoPanel.root.SetActive (false);
		mBuildUpgradePanel.root.SetActive (true);
	}


//	public void UpgradeBuildingClient(int buildingIndex,int level){
//		RpcUpgradeBuilding (buildingIndex,level);
//	}

	public void UpgradeBuildingServer(){
		CmdUpgradeBuilding (selectBuilding.GetComponent<SpawnPoint>().index,mBuildOrder);
		mBuildUpgradePanel.root.SetActive (false);
	}

	[Command]
	public void CmdUpgradeBuilding(int buildingIndex,int order){
		mServerController_III.UpgradeBuilding (buildingIndex,order,playerIndex);
	}

//	[ClientRpc]
//	public void RpcUpgradeBuilding(int buildingIndex,int level){
//		SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
//		SpawnPoint building = null;
//		foreach(SpawnPoint sp in spawnPoints)
//		{
//			if(sp.index == buildingIndex && sp.group == playerIndex)
//			{
//				building = sp;
//				break;
//			}
//		}
//		if(building!=null)
//		{
//			Renderer rd = building.GetComponentInChildren<Renderer>();
//			Material mat = rd.material;
//			Material mat0 = new Material(Shader.Find("Standard"));
//			mat0.SetTexture("_MainTex",mat.GetTexture("_MainTex"));
//			mat0.SetTexture("_BumpMap",mat.GetTexture("_BumpMap"));
//			if(level == 1)
//			{
//				mat0.SetColor("_Color",Color.red);
//			}
//			else
//			{
//				mat0.SetColor("_Color",Color.blue);
//			}
//			rd.material = mat0;
//		}
//	}

	[ClientRpc]
	public void RpcBuildFail(){
		if(isLocalPlayer)
		{
			mPlayerPanel.ShowMsg("你的金币不足");
		}
	}

	public void RefreshCorn(){
		RpcRefreshCorn (playerAttribute.corn, playerAttribute.killNum);
	}

	[ClientRpc]
	public void RpcRefreshCorn(int cornValue,int killNumValue){
		if(isLocalPlayer)
		{
			mPlayerPanel.corn.text = "Corn " + cornValue;
			mPlayerPanel.killNum.text = "Kill " + killNumValue;
		}
	}

	[Command]
	public void CmdBeginSelect(){
	
	}

	[Command]
	public void CmdPlaceBuilding(int buildIndex,int planeIndex)
	{
		if(playerIndex == 0 || playerIndex == 1)
		{
			mServerController_III.SpawnBuilding(playerIndex,buildIndex,planeIndex,race);
		}
	}

	public void SendPlayerIndex(int index){
		this.playerIndex = index;

		RpcSendPlayerIndex (index);
	}

	[ClientRpc]
	public void RpcSendPlayerIndex(int index){
		if(!isLocalPlayer)
		{
			return;
		}
		this.playerIndex = index;
		RtsCamera rtsCamera = Camera.main.GetComponent<RtsCamera>();
		if (rtsCamera != null) {
			if (index == 0) {
				this.buildingLayer = 18;
				rtsCamera.LookAt = new Vector3(11,1.5f,-80);
			}
			else if(index == 1)
			{
				this.buildingLayer = 19;
				rtsCamera.LookAt = new Vector3(11,1.5f,82.5f);
			}
		}
		CmdRequestPlayerInfo ();
	}

	[Command]
	public void CmdRequestPlayerInfo(){
		SendPlayerInfoMsg ();
	}

	//发送战前准备信息
	public void SendPlayerInfoMsg(){
		mServerController_III = ServerController_III.instance;
		bool isAIMode = mServerController_III.isAIMode;
		int isPlayer0Ready = -1;
		int isPlayer1Ready = -1;
		int playerRace0 = 0;
		if (mServerController_III.players [0] != null) {
			isPlayer0Ready = mServerController_III.players [0].isReady ? 1:0;
			playerRace0 = mServerController_III.players [0].race;
		}
		int playerRace1 = 0;
		if (mServerController_III.players [1] != null) {
			isPlayer1Ready = mServerController_III.players [1].isReady ? 1:0;
			playerRace1 = mServerController_III.players [1].race;
		}
		if(mServerController_III.isAIMode)
		{
			playerRace1 = mServerController_III.aiController.race;
		}


		RpcSendPlayerInfoMsg (isAIMode,isPlayer0Ready,isPlayer1Ready,playerRace0,playerRace1);
	}

	UIButton mCurrentReadyButton;
	[ClientRpc]
	public void RpcSendPlayerInfoMsg(bool isAIMode,int isPlayer0Ready,int isPlayer1Ready,int playerRace0,int playerRace1)
	{
		if(isLocalPlayer)
		{
			mServerMsgPanel.root.SetActive(true);
			if(playerIndex == 0 )
			{

				mCurrentReadyButton = mServerMsgPanel.readyButton;
				if(isPlayer0Ready!=1)
				{
					mServerMsgPanel.readyButton.isEnabled = true;
					mServerMsgPanel.readyButton.onClick.Clear();
					mServerMsgPanel.readyButton.onClick.Add(new global::EventDelegate(ClientReady));
					mServerMsgPanel.race0.GetComponent<UIButton>().isEnabled = true;
				}
				else if(isPlayer0Ready==1)
				{
					mServerMsgPanel.race0.GetComponent<UIButton>().isEnabled = false;
				}


				if(isAIMode || isPlayer1Ready!=-1)
				{
					mServerMsgPanel.readyButton1.isEnabled = false;
					mServerMsgPanel.race1.GetComponent<UIButton>().isEnabled = false;
					mServerMsgPanel.race1.value = mServerMsgPanel.race1.items[playerRace1];
				}
				else if(isPlayer1Ready==-1)
				{
					mServerMsgPanel.readyButton1.isEnabled = true;
					mServerMsgPanel.readyButton1.GetComponentInChildren<UILabel>().text = "Add AI";
					mServerMsgPanel.race1.GetComponent<UIButton>().isEnabled = true;
					mServerMsgPanel.readyButton1.onClick.Clear();
					mServerMsgPanel.readyButton1.onClick.Add(new global::EventDelegate(AddAI));
				}
			}else if(playerIndex == 1)
			{
				mCurrentReadyButton = mServerMsgPanel.readyButton1;
				mServerMsgPanel.readyButton.isEnabled = false;
				mServerMsgPanel.race0.GetComponent<UIButton>().isEnabled = false;
				mServerMsgPanel.race0.value = mServerMsgPanel.race0.items[playerRace0];

				if(isPlayer1Ready!=1)
				{
					mServerMsgPanel.readyButton1.isEnabled = true;
					mServerMsgPanel.race1.GetComponent<UIButton>().isEnabled = true;
				}

				mServerMsgPanel.readyButton1.onClick.Clear();
				mServerMsgPanel.readyButton1.onClick.Add(new global::EventDelegate(ClientReady));
			}

			if(isPlayer0Ready == -1){
				mServerMsgPanel.playerMsg0.text = "Empty";
			}else if(isPlayer0Ready == 0){
				mServerMsgPanel.playerMsg0.text = "Not Ready";
			}else if(isPlayer0Ready == 1){
				mServerMsgPanel.playerMsg0.text = "Ready";
			}

			if(isPlayer1Ready == -1){
				if(isAIMode)
					mServerMsgPanel.playerMsg1.text = "AI";
				else
					mServerMsgPanel.playerMsg1.text = "Empty";
			}else if(isPlayer1Ready == 0){
				mServerMsgPanel.playerMsg1.text = "Not Ready";
			}else if(isPlayer1Ready == 1){
				mServerMsgPanel.playerMsg1.text = "Ready";
			}

		}
	}

	//添加AI
	public void AddAI(){
		int race = mServerMsgPanel.race0.items.IndexOf (mServerMsgPanel.race1.value);
		CmdAddAI (race);
	}

	[Command]
	public void CmdAddAI(int race){
		mServerController_III.AddAI (race);
	}

	//准备
	public void ClientReady(){
		int race = mServerMsgPanel.race0.items.IndexOf (mServerMsgPanel.race0.value);
		this.race = race;
		CmdClientReady (race);
		mCurrentReadyButton.isEnabled = false;
	}
	
	[Command]
	public void CmdClientReady(int race)
	{
		this.isReady = true;
		mServerController_III.ClientReady (this,race);
	}

	public void SendBattleBegin()
	{
		RpcSendBattleBegin ();
	}

	[ClientRpc]
	public void RpcSendBattleBegin()
	{
		if (isLocalPlayer) {
			StartCoroutine(_HideServerMsgPanel());
		}
	}

	IEnumerator _HideServerMsgPanel(){
		mServerMsgPanel.msgTime.gameObject.SetActive (true);
		mServerMsgPanel.root.SetActive(false);

		int t = 3;
		while(t>0)
		{
			mServerMsgPanel.msgTime.text = t.ToString();
			t-=1;
			yield return new WaitForSeconds(1);
		}
		mServerMsgPanel.msgTime.text = "Begin";
		yield return new WaitForSeconds(1);
		isBattleBegin = true;
		mBuildingPanel.root.SetActive(true);
		mPlayerPanel.root.SetActive(true);
		mBuildingPanel.closeButton.onClick.Add(new global::EventDelegate(CloseBuildingPanel));

		List<GameObject> buildings;
		if (race == 0) {
			buildings = ServerController_III.instance.buildPrefabs;
		} else {
			buildings = ServerController_III.instance.buildPrefabs1;
		}
		
		for(int i=0;i<mBuildingPanel.buildings.Count;i++)
		{
			if(buildings.Count>i)
			{
				mBuildingPanel.buildings[i].onClick.Clear();
				mBuildingPanel.buildings[i].onClick.Add(new global::EventDelegate(SelectBuilding));
				GameObject go = Instantiate(buildings[i]) as GameObject;
				go.transform.parent = mBuildingPanel.buildings[i].GetComponent<BuildingItemUI>().buildingPrefabPoint;
				go.transform.localScale = new Vector3(15,15,15);
				go.transform.localPosition = Vector3.zero;

				Destroy(go.GetComponent<SpawnPoint>());
				Destroy(go.GetComponent<Collider>());
				mBuildingPanel.buildings[i].GetComponent<BuildingItemUI>().detailTrigger.onClick.Clear();
				mBuildingPanel.buildings[i].GetComponent<BuildingItemUI>().detailTrigger.onClick.Add(new global::EventDelegate(ShowBuildingInfo));
			}
			else
			{
				mBuildingPanel.buildings[i].gameObject.SetActive(false);
			}
//			mBuildingPanel.buildings[i]

		}
		for(int i =0;i<mServerController_III.buildPrefabs.Count;i++)
		{
			mBuildingPanel.buildings[i].GetComponentInChildren<UILabel>().text = mServerController_III.buildPrefabs[i].GetComponent<SpawnPoint>().buildingName;
		}
		mServerMsgPanel.msgTime.gameObject.SetActive (false);
	}


	public void SendPlayerWin(int index){
		RpcSendPlayerWin (index);
	}

	[ClientRpc]
	public void RpcSendPlayerWin(int index)
	{
		if(index == this.playerIndex)
		{
			mResultPanel.winNode.SetActive (true);
		}
		else if(index != this.playerIndex)
		{
			mResultPanel.failNode.SetActive (true);
		}
	}

}
