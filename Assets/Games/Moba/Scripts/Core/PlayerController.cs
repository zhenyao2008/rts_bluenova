using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class PlayerController :  NetworkBehaviour {

	public PlayerII player;
	int preIndex = -1;
	public const int groundLayer = 15;
//	RtsCamera mRtsCamera;
	public bool isLockCamera = false;
//	ETCJoystick mJoystick ;
	SkillPanel mSkillPanel;
	public GameObject autoSelectObj;
//	SelectTarget mSelectTarget;
//	PlayerPanel mPlayerPanel;
	ResultPanel mResultPanel;

	void Start(){
		mChat = FindObjectOfType<ChatPanel>();
		mSkillPanel = FindObjectOfType<SkillPanel>();
		if (NetworkClient.active && isLocalPlayer) {
			CmdCheckActiveButton();
			if(mChat!=null)mChat.chatInput.onSubmit.Add (new global::EventDelegate (ChatSubmit));
//			mRtsCamera = FindObjectOfType<RtsCamera>();
//			mJoystick = FindObjectOfType<ETCJoystick>();
			if(mSkillPanel!=null){
				mSkillPanel.backCity.onClick.Add(new global::EventDelegate(BackCity));
				mSkillPanel.skill0.onClick.Add(new global::EventDelegate(Skill01));
//				mSkillPanel.autoSelect.onClick.Add(new global::EventDelegate(Skill01));
				mSkillPanel.root.SetActive(false);
			}
//			mSelectTarget = FindObjectOfType<SelectTarget>();
//			mPlayerPanel = FindObjectOfType<PlayerPanel>();
//			if(mResultPanel==null)
//			{
				mResultPanel = FindObjectOfType<ResultPanel>();
				if(mResultPanel!=null){
					mResultPanel.failButton.onClick.Add(new global::EventDelegate(Reload));

					mResultPanel.winButton.onClick.Add(new global::EventDelegate(Reload));

				}
//			}
////			else
////			{
////				mResultPanel.failNode.SetActive(false);
////				mResultPanel.failNode.SetActive(false);
////			}

//			ServerController_II.GetInstance ().playerController = this;
		}
	}

	void Reload(){
		NetworkClient.ShutdownAll ();
//		Application.LoadLevel ("BattlePVE");
		ServerController_II.GetInstance ().StartClient ();
	}


	public static PlayerController GetLocalPlayer(){
		PlayerController[] pcs = FindObjectsOfType<PlayerController>();
		foreach(PlayerController pc in pcs)
		{
			if(pc.isLocalPlayer)
			{
				return pc;
			}
		}
		return null;
	}

	void Update()
	{
		if (!isLocalPlayer)
			return;

		if(Input.GetMouseButtonDown(0) && !UICamera.isOverUI)
		{
			RaycastHit hit;
			if(Input.mousePosition.x > Screen.width/3 || Input.mousePosition.y > Screen.height/3)
			{
				if(player!=null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,player.targetLayer))
				{
//					CmdManualAttack(hit.transform.GetComponent<UnitBase>());
				}
				else if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<groundLayer))
				{
					CmdMove(hit.point);
				}
			}
		}
	}

	[ClientRpc]
	public void RpcCameraFollow(){
//		Debug.Log ("RpcCameraFollow");
//		if (mRtsCamera != null) {
////#if !UNITY_STANDALONE
//			mRtsCamera.Follow(trans,false);
//
//			mRtsCamera.GetComponent<RtsCameraKeys> ().enabled = false;
//			mRtsCamera.GetComponent<RtsCameraMouse> ().enabled = false;
////#endif
//		}
//		if (mJoystick != null) {
//			mJoystick.onMove.AddListener(JoystickMove);
//		}
//		mSkillPanel.root.SetActive (true);
//		player = trans.GetComponent<PlayerII>();
//		mPlayerPanel.root.SetActive (true);
//		mPlayerPanel.SetUnitAttribute (player.unitAttribute);
	}

	public void JoystickMove(Vector2 v){
		CmdMove (player.pos + new Vector3(v.x,0,v.y).normalized);
	}

	[Command]
	public void CmdCheckActiveButton(){
		ServerController_II.GetInstance ().playerSelect.CheckActiveButton ();
	}

	[Command]
	public void CmdMove(Vector3 targetPos)
	{
		if (player != null)
			player.ManualMove (Camera.main.transform.position,targetPos);
	}

	[Command]
	public void CmdSelectHero(int index)
	{
		player = ServerController_II.GetInstance().SelectPlayer(index,this.preIndex);
		player.controller = this;
		this.preIndex = index;
//		if(isLockCamera)
//			RpcCameraFollow (player.transform);
	}


//	[Command]
//	public void CmdManualAttack(UnitBase ub){
//		player.ManualAttack (ub);
//	}

	[Command]
	public void CmdAutoSelectTarget(){
//		int targetLayer = player.targetLayer;
//		Collider[] colls = Physics.OverlapSphere (player.transform.position,30,targetLayer);
//		if(colls!=null)
//		{
//			Collider col = colls[Random.Range(0,colls.Length)];
//		}
	}

	public void SetTarget(Transform t){
//		RpcSetTarget (t);
	}

//	[ClientRpc]
//	public void RpcSetTarget(Transform t){
//		if(mSelectTarget!=null && t!=null)mSelectTarget.followTarget = t;
//	}

	[Command]
	public void CmdHandleTarget(){
	
	}

	public void Skill01()
	{
		Vector3 direct = player.transform.forward;
		Debug.Log (direct);
		CmdSkill01 (direct);
	}

	public void SkillCoolDown01(float cdTime){
		
	}

	public void Skill02()
	{
//		CmdSkill (1);
	}

	public void Skill03()
	{
//		CmdSkill (2);
	}

	public void BackCity(){
		CmdBackCity ();
		if (player != null && player.playerUI != null)	
			player.playerUI.SpecialFrant (player.specialTime);
	}

	[Command]
	public void CmdSkill01(Vector3 direct){
		Debug.Log (direct);
		if (player.Skill01 (direct)) {
			RpcSkill01 ();
		}
	}

	[ClientRpc]
	public void RpcSkill01(){
		mSkillPanel.CoolDown01 (player.skillInterval01);
	}

	[Command]
	public void CmdBackCity()
	{
		if (player != null) {
			player.state = UnitState.Special;
		}	
	}

	public void ShowBattleEnd(int win){
		RpcShowBattleEnd (win);
	}

	[ClientRpc]
	public void RpcShowBattleEnd(int win)
	{
		if (win == 0) {
			mResultPanel.Win ();
		} else {
			mResultPanel.Fail ();
		}
		GameObject go = new GameObject ();
		ReLoadClient rc = go.AddComponent<ReLoadClient>();
		rc.ReStart ();
	}

	//---------------------- Chat --------------------------
	ChatPanel mChat;
//	int maxCharPerLine = 15;
	public void ChatSubmit(){
//		string text = mChat.chatInput.value;

//		StringBuilder stringBuilder = new StringBuilder (text);
//		int count = text.Length;



		CmdChatSubmit (mChat.chatInput.value + "\n");
		mChat.chatInput.value = "";
	}
	[Command]
	public void CmdChatSubmit(string text){
		mChat.ServerAddChatMsg(text);
	}
	//-------------------- End Chat ------------------------
}
