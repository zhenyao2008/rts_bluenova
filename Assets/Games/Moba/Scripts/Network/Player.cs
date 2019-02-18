using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player:NetworkBehaviour {

	[SyncVar]
	private Vector3 pos;

	[SyncVar]
	private Quaternion qua;

	private CharacterController mCha;
	SimpleRpgCamera rpgCamera;

	public Animation playerAnim;

	public PlayerUI playerUIPrefab;

	public Transform headPoint;

	public Transform shootPoint;

	[SyncVar]
	public int currentHealth;
	[SyncVar]
	public int maxHealth;

	void Awake(){
		mCha = GetComponent<CharacterController> ();
		playerAnim = GetComponentInChildren<Animation> ();
		rpgCamera = FindObjectOfType<SimpleRpgCamera> ();
		currentHealth = 1000;
		maxHealth = 1000;
	}
	ChatPanel mChat;
	void Start()
	{
		pos = transform.position;
		playerAnim.Play ("idle");
		if(isLocalPlayer)
		{
			rpgCamera.target = transform;
		}
		if(NetworkClient.active)
		{
			GameObject go = Instantiate<GameObject>(playerUIPrefab.gameObject);
			go.GetComponent<PlayerUI>().player = this;
		}
		if(isLocalPlayer)
		{
			mChat = FindObjectOfType<ChatPanel>();
			mChat.chatInput.onSubmit.Add(new global::EventDelegate(SubmitText));;
		}


	}

	void Update()
	{
		if(NetworkServer.active)
		{
			UpdateServer();
		}

		if (NetworkClient.active ) {
			UpdateClient();
		}

//		if (isLocalPlayer && isClient ) {
//			transform.position += new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0) * Time.deltaTime * 20;
//			pos = transform.position;
//		} 
//
//		if(isClient ){
//			transform.position = Vector3.Lerp(transform.position,pos,0.1f);
//		}

	}
	[SyncVar]
	public bool rotateLeft = false;
	[SyncVar]
	public bool rotateRight = false;
	[SyncVar]
	public bool moveW = false;
	[SyncVar]
	public bool moveS = false;
	[SyncVar]
	public bool jump = false;

//	float speed = 200;
	float rotateSpeed = 100;
	void UpdateServer()
	{
		if(moveW)
		{
//			transform.position += transform.forward * Time.deltaTime * speed;
			mCha.SimpleMove(transform.forward * 2);
//			playerAnim.Play("run");
			pos = transform.position;
		}
		if(moveS)
		{
//			transform.position -= transform.forward * Time.deltaTime * speed;
			mCha.SimpleMove(-transform.forward );
//			playerAnim.Play("run");
			pos = transform.position;
		}

		if (rotateLeft) {
			transform.Rotate(new Vector3(0,-1,0),Time.deltaTime * rotateSpeed);
			qua = transform.rotation;
		}
		if (rotateRight) {
			transform.Rotate(new Vector3(0,1,0),Time.deltaTime * rotateSpeed);
			qua = transform.rotation;
		}
//
//		if(!moveW && !moveS)
//		{
//			playerAnim.Play("idle");
//		}

	}

//	bool localJump = false;
	void UpdateClient()
	{
		transform.position = Vector3.Lerp(transform.position,pos,0.1f);
		transform.rotation = Quaternion.Lerp (transform.rotation,qua,0.1f);




		if(!isLocalPlayer){
			return;
		}
		if(Input.GetKeyDown(KeyCode.W))
		{
			CmdMoveW();
		}
		if(Input.GetKeyUp(KeyCode.W))
		{
			CmdUnMoveW();
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			CmdMoveS();
		}
		if(Input.GetKeyUp(KeyCode.S))
		{
			CmdUnMoveS();
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			CmdRotateLeft(true);
		}
		if(Input.GetKeyUp(KeyCode.A))
		{
			CmdRotateLeft(false);
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			CmdRotateRight(true);
		}
		if(Input.GetKeyUp(KeyCode.D))
		{
			CmdRotateRight(false);
		}
		if(Input.GetKeyDown(KeyCode.J))
		{
			CmdFire();
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			CmdJump();
		}
	}

	public GameObject bulletPrefab;

	[Command]
	public void CmdFire(){
		GameObject go = Instantiate<GameObject> (bulletPrefab);
		go.transform.position = shootPoint.position + new Vector3 (0,1.5f,0) + transform.forward * 2;
		go.transform.forward = transform.forward;
//		Bullet bullet = go.GetComponent<Bullet> ();
//		RaycastHit hit;

		Vector3 direct = transform.forward;
		int angle = Random.Range (-5,5);
		float anglePI = angle * Mathf.PI / 180 / 2;
		direct =new Quaternion (0,Mathf.Sin(anglePI),0,Mathf.Cos(anglePI)) * direct;

//		Vector3 targetPos = transform.position + direct * 200;
//		if(Physics.Raycast(transform.position,direct,out hit))
//		{
//			targetPos = hit.point;
//		}
		NetworkServer.Spawn (go);

//
//
//		Vector3 direct = transform.forward;
//		StartCoroutine (_Fire(bullet, direct));
	}

	[Command]
	public void CmdMoveW()
	{
		this.moveW = true;
		RpcMove ();
	}

	[ClientRpc]
	public void RpcMove(){
		playerAnim.Play ("run");
	}

	[ClientRpc]
	public void RpcUnMove(){
		playerAnim.Play ("idle");
	}

	[Command]
	public void CmdUnMoveW()
	{
		this.moveW = false;
		if(!this.moveS)
			RpcUnMove ();
	}

	[Command]
	public void CmdMoveS()
	{
		this.moveS = true;
		RpcMove ();
	}
	
	[Command]
	public void CmdUnMoveS()
	{
		this.moveS = false;
		if(!this.moveW)
			RpcUnMove ();
	}

	[Command]
	public void CmdRotateLeft(bool isTrue)
	{
		rotateLeft = isTrue;
	}
	[Command]
	public void CmdRotateRight(bool isTrue)
	{
		rotateRight = isTrue;
	}

	[Command]
	public void CmdJump(){
		RpcJump ();
		StartCoroutine (_Jump());
	}

	[ClientRpc]
	public void RpcJump() {
		StartCoroutine (_Jump());
	}

	float jumpHeight = 2;
	IEnumerator _Jump(){
		float t = 0;
		Transform trans = playerAnim.transform;
		Vector3 startPos = Vector3.zero;
		Vector3 targetPos = startPos + new Vector3 (0,jumpHeight,0);
		while(t < 1)
		{
			t += Time.deltaTime;
			if(t<0.5)
				trans.localPosition = Vector3.Lerp(startPos,targetPos,t * 2);
			else
				trans.localPosition = Vector3.Lerp(startPos,targetPos,2-t * 2);
			yield return null;
		}
	}

	[SyncVar]
	public string chatText;

	public void SubmitText(){

		CmdText (mChat.chatInput.value + "\n");
		mChat.chatInput.value = "";
	}

	[Command]
	public void CmdText(string text)
	{
//		ChatPanel serverChat = FindObjectOfType<ChatPanel> ();
//		serverChat.chatText += text;
//		this.chatText = serverChat.chatText;
	}

}
