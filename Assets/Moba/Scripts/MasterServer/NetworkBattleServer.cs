using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkBattleServer : MonoBehaviour {

	public NetworkClient client = null;
	public string masterServerIpAddress = "127.0.0.1";
	public int masterServerPort = 43333;

	string[] mBattleArgs;
	public int localServerPort;

	public static NetworkBattleServer instance;
	public static NetworkBattleServer SingleTon(){
		if(instance == null)
		{
			GameObject go = new GameObject("_BattleServer");
			instance = go.AddComponent<NetworkBattleServer>();
			GameObject.DontDestroyOnLoad(go);
		}
		return instance;
	}

	void Awake(){
		InitializeClient ();
	}

	public void InitializeClient()
	{
		if (client != null)
		{
			Debug.LogError("Already connected");
			return;
		}
#if UNITY_EDITOR
		localServerPort = 8002;
#else
		mBattleArgs = System.Environment.GetCommandLineArgs();
		localServerPort = int.Parse (mBattleArgs [1]);
#endif
		client = new NetworkClient();
		client.Connect(masterServerIpAddress, masterServerPort);
		// system msgs
		client.RegisterHandler(MsgType.Connect, OnClientConnect);
		client.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
		client.RegisterHandler(MsgType.Error, OnClientError);
		
		// application msgs
//		client.RegisterHandler(MasterMsgTypes.RegisteredHostId, OnRegisteredHost);
//		client.RegisterHandler(MasterMsgTypes.UnregisteredHostId, OnUnregisteredHost);
//		client.RegisterHandler(MasterMsgTypes.ListOfHostsId, OnListOfHosts);
//		client.RegisterHandler (MasterMsgTypes.JoinedRoom,OnJoinedRoom);
//		client.RegisterHandler (MasterMsgTypes.LaunchedBattle, OnBattleLaunched);
//		

	}

	// --------------- System Handlers -----------------

	void OnClientConnect(NetworkMessage netMsg)
	{
		Debug.Log("Client Connected to Master");
		Application.LoadLevel ("BattlePVE");
	}

	void OnClientDisconnect(NetworkMessage netMsg)
	{
		Debug.Log("Client Disconnected from Master");
//		ResetClient();
//		OnFailedToConnectToMasterServer();

	}
	
	void OnClientError(NetworkMessage netMsg)
	{
		Debug.Log("ClientError from Master");
//		OnFailedToConnectToMasterServer();
	}

	void OnRegisteredHost(NetworkMessage netMsg)
	{
		Application.LoadLevel ("BattlePVE");
	}

	public void BattleServerReady(){
		client = new NetworkClient();
		client.Connect(masterServerIpAddress, masterServerPort);
		MasterMsgTypes.BattleServerReadyMessage msg = new MasterMsgTypes.BattleServerReadyMessage ();
		msg.port = this.localServerPort;
		client.Send (MasterMsgTypes.BattleServerReadyMessageId,msg);
	}

}
