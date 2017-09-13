using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;


public class NetworkMasterServer : MonoBehaviour
{
	public int MasterServerPort = 43333;


	public Dictionary<int,NetworkConnection> clients;
	public const int maxBattleServerCount = 5;
	public List<NetworkConnection> battleServers = new List<NetworkConnection>();
	public List<NetworkConnection> availableBattleServers= new List<NetworkConnection>();

	int mCurrentPort = 8100;
	void AddBattleServer(NetworkMessage msg)
	{
		battleServers.Add (msg.conn);
		availableBattleServers.Add(msg.conn);
	}

	void OnServerLaunchBattle(NetworkMessage netMsg){
		Debug.Log ("Start Battle");
	}

	public string battleFileName = @"D:/Projects/Moba2/BattleServer.exe";
	void StartBattleServer()
	{
		System.Diagnostics.Process process = new System.Diagnostics.Process ();
		process.StartInfo.FileName = battleFileName; 
		process.StartInfo.Arguments = mCurrentPort.ToString ();
		process.Start();
		mCurrentPort ++;
	}

	void OnBattleServerReady(NetworkMessage netMsg){
//		MasterMsgTypes.BattleServerReadyMessage msg = netMsg.ReadMessage<MasterMsgTypes.BattleServerReadyMessage>();
		Debug.Log ("LaunchClient");
		AddBattleServer (netMsg);
//		LaunchClient (0,msg.port);
	}

	//房间等待有可用的战斗服务器接入
	void Update()
	{

	}
//	public Dictionary<int,NetworkConnection> battleServers;

	// map of gameTypeNames to rooms of that type
	Dictionary<string, Rooms> gameTypeRooms = new Dictionary<string, Rooms>();
	Dictionary<int,int> playerRooms = new Dictionary<int, int>();
	public void InitializeServer()
	{
		if (NetworkServer.active)
		{
			Debug.LogError("Already Initialized");
			return;
		}
		NetworkServer.Listen(MasterServerPort);

		// system msgs
		NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
		NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
		NetworkServer.RegisterHandler(MsgType.Error, OnServerError);
		// application msgs
		NetworkServer.RegisterHandler(MasterMsgTypes.RegisterHostId, OnServerRegisterHost);
		NetworkServer.RegisterHandler(MasterMsgTypes.UnregisterHostId, OnServerUnregisterHost);
		NetworkServer.RegisterHandler(MasterMsgTypes.RequestListOfHostsId, OnServerListHosts);

		NetworkServer.RegisterHandler (MasterMsgTypes.JoinRoom, OnServerJoinRoom);
		NetworkServer.RegisterHandler (MasterMsgTypes.LaunchBattle,OnServerLaunchBattle);
		NetworkServer.RegisterHandler (MasterMsgTypes.RequestBattleServerPortMessageId, OnResponseBattleServerPort);


		NetworkServer.RegisterHandler (MasterMsgTypes.UserLoginId, OnUserLogin);
		NetworkServer.RegisterHandler (MasterMsgTypes.BattleServerReadyMessageId, OnBattleServerReady);
		NetworkServer.RegisterHandler (MasterMsgTypes.UserRequestOnlinePlayId, OnUserRequestOnlinePlay);

		InitRooms ();
		DontDestroyOnLoad(gameObject);
	}

	void InitRooms(){
		Rooms rms = new Rooms ();
		rms.name = "Moba";
		for(int i=0;i<5;i++)
		{
			MasterMsgTypes.Room room = new MasterMsgTypes.Room();
			room.name = "Host" + i;
			room.maxRoomMember = 10;
			rms.rooms.Add(room.name ,room);
		}
		gameTypeRooms.Add ("Moba", rms);
	}

	public void ResetServer()
	{
		NetworkServer.Shutdown();
		NetworkServer.Reset ();
	}


	Rooms EnsureRoomsForGameType(string gameTypeName)
	{
		if (gameTypeRooms.ContainsKey(gameTypeName))
		{
			return gameTypeRooms[gameTypeName];
		}

		Rooms newRooms = new Rooms();
		newRooms.name = gameTypeName;
		gameTypeRooms[gameTypeName] = newRooms;
		return newRooms;
	}

	// --------------- System Handlers -----------------

	void OnServerConnect(NetworkMessage netMsg)
	{
		Debug.Log("Master received client");
	}

	void OnServerDisconnect(NetworkMessage netMsg)
	{
		Debug.Log("Master lost client");

		// remove the associated host
		foreach (var rooms in gameTypeRooms.Values)
		{
			foreach (var room in rooms.rooms.Values)
			{
				if (room.connectionId == netMsg.conn.connectionId)
				{
					// tell other players?

					// remove room
					rooms.rooms.Remove(room.name);

					Debug.Log("Room ["+room.name+"] closed because host left");
					break;
				}
			}
		}

	}

	void OnServerError(NetworkMessage netMsg)
	{
		Debug.Log("ServerError from Master");
	}

	// --------------- Application Handlers -----------------


	void OnServerRegisterHost(NetworkMessage netMsg)
	{
		Debug.Log("OnServerRegisterHost");
		if (!clients.ContainsKey (netMsg.conn.connectionId)) {
			clients.Add(netMsg.conn.connectionId,netMsg.conn);
		}
		var msg = netMsg.ReadMessage<MasterMsgTypes.RegisterHostMessage>();
		var rooms = EnsureRoomsForGameType(msg.gameTypeName);

		int result = (int)MasterMsgTypes.NetworkMasterServerEvent.RegistrationSucceeded;
		if (!rooms.AddHost(msg.gameName, msg.comment, netMsg.conn.address, msg.hostPort, netMsg.conn.connectionId))
		{
			result = (int)MasterMsgTypes.NetworkMasterServerEvent.RegistrationFailedGameName;
		}

		var response = new MasterMsgTypes.RegisteredHostMessage();
		response.resultCode = result;
		netMsg.conn.Send(MasterMsgTypes.RegisteredHostId, response);
	}

	void OnServerJoinRoom(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<MasterMsgTypes.JoinRoomMessage> ();
		MasterMsgTypes.Room[] rooms = gameTypeRooms ["Moba"].GetRooms ();
		var response = new MasterMsgTypes.JoinedRoomMessage();
		if(rooms.Length <= msg.roomId)
		{
			response.joined = false;
			response.errorMsg = "Room is not exit.";
		}
		else if(rooms [msg.roomId].isStarted)
		{
			response.joined = false;
			response.errorMsg = "Room is started.";
		}
		else if(rooms [msg.roomId].rms !=null && rooms [msg.roomId].rms.Length >= rooms [msg.roomId].maxRoomMember)
		{
			response.joined = false;
			response.errorMsg = "Room is full.";
		}
		else
		{
			MasterMsgTypes.Room room = rooms [msg.roomId];
			MasterMsgTypes.RoomMember[] rms = room.rms;
			List<int> usedLocationId = new List<int>();
			if(rms==null)
				rms = new MasterMsgTypes.RoomMember[0];
			bool isContain = false;
			for(int i=0;i<rms.Length;i++)
			{
				usedLocationId.Add(rms[i].locationId);
				if(rms[i].connectionId == netMsg.conn.connectionId)
				{
					isContain = true;
					break;
				}
			}
			if(!isContain){
				List<MasterMsgTypes.RoomMember> rmList = new List<MasterMsgTypes.RoomMember>(rms);
				MasterMsgTypes.RoomMember rm = new MasterMsgTypes.RoomMember();
				rm.connectionId =  netMsg.conn.connectionId;
				for(int i=0;i<10;i++)
				{
					if(!usedLocationId.Contains(i))
					{
						rm.locationId=i;
						break;
					}
				}
				rm.memberName=netMsg.conn.address;
				rmList.Add(rm);
				room.rms = rmList.ToArray();
				rooms [msg.roomId] = room;
				if(playerRooms.ContainsKey(netMsg.conn.connectionId))
				{
					playerRooms[netMsg.conn.connectionId] = msg.roomId;
				}
				else
				{
					playerRooms.Add(netMsg.conn.connectionId,msg.roomId);
				}
			}

			response.room = rooms [msg.roomId];
			response.joined = true;
		} 
		netMsg.conn.Send(MasterMsgTypes.JoinedRoom, response);
	}


	void OnServerUnregisterHost(NetworkMessage netMsg)
	{
		Debug.Log("OnServerUnregisterHost");
		var msg = netMsg.ReadMessage<MasterMsgTypes.UnregisterHostMessage>();

		// find the room
		var rooms = EnsureRoomsForGameType(msg.gameTypeName);
		if (!rooms.rooms.ContainsKey(msg.gameName))
		{
			//error
			Debug.Log("OnServerUnregisterHost game not found: " + msg.gameName);
			return;
		}

		var room = rooms.rooms[msg.gameName];
		if (room.connectionId != netMsg.conn.connectionId)
		{
			//err
			Debug.Log("OnServerUnregisterHost connection mismatch:" + room.connectionId);
			return;
		}
		rooms.rooms.Remove(msg.gameName);

		// tell other players?

		var response = new MasterMsgTypes.RegisteredHostMessage();
		response.resultCode = (int)MasterMsgTypes.NetworkMasterServerEvent.UnregistrationSucceeded;
		netMsg.conn.Send(MasterMsgTypes.UnregisteredHostId, response);
	}

	void OnServerListHosts(NetworkMessage netMsg)
	{
		Debug.Log("OnServerListHosts");
		var msg = netMsg.ReadMessage<MasterMsgTypes.RequestHostListMessage>();
		if (!gameTypeRooms.ContainsKey(msg.gameTypeName))
		{
			var err = new MasterMsgTypes.ListOfHostsMessage();
			err.resultCode = -1;
			netMsg.conn.Send(MasterMsgTypes.ListOfHostsId, err);
			return;
		}

		var rooms = gameTypeRooms[msg.gameTypeName];
		var response = new MasterMsgTypes.ListOfHostsMessage();
		response.resultCode = 0;
		response.hosts = rooms.GetRooms();
		netMsg.conn.Send(MasterMsgTypes.ListOfHostsId, response);
	}

	//客户端用户登录
	void OnUserLogin(NetworkMessage netMsg){
		var msg = netMsg.ReadMessage<MasterMsgTypes.UserLoginMessage>();
		//TODO
		//省去一系列验证
		int connectionId = netMsg.conn.connectionId;

		User user = new User ();
		user.userName = msg.userName;
		user.connectionId = connectionId;
		user.conn = netMsg.conn;
		NetworkMasterServerData.SingleTon ().AddUser (user);
		MasterMsgTypes.UserLoginedMessage msgLogined = new MasterMsgTypes.UserLoginedMessage ();
		msgLogined.isLogin = true;
		netMsg.conn.Send (MasterMsgTypes.UserLoginedId,msgLogined);


	}

	//客户端用户注册
	void OnUserRegister(NetworkMessage netMsg)
	{
		//只保留接口目前还不需要
	}

	//客户端寻找在线玩家
	void OnUserRequestOnlinePlay(NetworkMessage netMsg){
		if (NetworkMasterServerData.SingleTon ().onlinePlaySearchingUsers.Count > 0) {
			MasterMsgTypes.ResponseBattleServerPortMessage response = new MasterMsgTypes.ResponseBattleServerPortMessage ();
			response.port = mCurrentPort;
			netMsg.conn.Send (MasterMsgTypes.ResponseBattleServerPortMessageId, response);
			NetworkMasterServerData.SingleTon ().onlinePlaySearchingUsers [0].conn.Send (MasterMsgTypes.ResponseBattleServerPortMessageId, response);
			StartBattleServer();
		} else {
			User user;
			if(NetworkMasterServerData.SingleTon ().GetUser(netMsg.conn.connectionId,out user))
			{
				NetworkMasterServerData.SingleTon ().onlinePlaySearchingUsers.Add(user);
			}
		}
	}

	//告诉客户端战斗服务器端口，客户端收到消息过后可以连接战斗服务器。
	void OnResponseBattleServerPort(NetworkMessage netMsg)
	{
//		var msg = netMsg.ReadMessage<MasterMsgTypes.RequestBattleServerPortMessage>();
		MasterMsgTypes.ResponseBattleServerPortMessage response = new MasterMsgTypes.ResponseBattleServerPortMessage ();
		response.port = currentServerPort;
		currentServerPort ++;
		netMsg.conn.Send (MasterMsgTypes.ResponseBattleServerPortMessageId,response);
		Debug.Log ("OnResponseBattleServerPort:" + response.port );
	}

	void LaunchClient(int roomId,int port){
		MasterMsgTypes.Room[] rooms = gameTypeRooms ["Moba"].GetRooms ();
		if(rooms.Length > roomId)
		{
			MasterMsgTypes.Room room = rooms[roomId];
			MasterMsgTypes.RoomMember[] rms = room.rms;
			for(int i = 0;i < rms.Length;i++)
			{
				NetworkConnection conn = GetClient(rms[i].connectionId);
				MasterMsgTypes.LaunchedBattleMessage msg = new MasterMsgTypes.LaunchedBattleMessage();
				msg.isReady = true;
				msg.port = port;
				conn.Send(MasterMsgTypes.LaunchedBattle,msg);
			}
		}
	}

	NetworkConnection GetClient(int connectId){
		return clients [connectId];
	}

	int currentServerPort = 43334;

	void OnGUI()
	{
		if (NetworkServer.active)
		{
			GUI.Label(new Rect(400, 0, 200, 20), "Online port:" + MasterServerPort);
			if (GUI.Button(new Rect(400, 20, 200, 20), "Reset  Master Server"))
			{
				ResetServer();
			}
			if(GUI.Button(new Rect(20, 20, 200, 20), "StartBattleServer"))
			{
				StartBattleServer();
			}
		}
		else
		{
			if (GUI.Button(new Rect(400, 20, 200, 20), "Init Master Server"))
			{
				InitializeServer();
			}
		}

		int y = 100;
		foreach (var rooms in gameTypeRooms.Values)
		{
			GUI.Label(new Rect(400, y, 200, 20), "GameType:" + rooms.name);
			y += 22;
			foreach (var room in rooms.rooms.Values)
			{
				GUI.Label(new Rect(420, y, 200, 20), "Game:" + room.name + " addr:" + room.hostIp + ":" + room.hostPort);
				y += 22;
			}
		}

		y = 100;
		for(int i=0;i<battleServers.Count;i++)
		{
			GUI.Label(new Rect(10, y, 200, 20), "battleServer:" + battleServers[i].connectionId.ToString());
		}


	}
}



public class Rooms
{
	public string name;
	public Dictionary<string, MasterMsgTypes.Room> rooms = new Dictionary<string, MasterMsgTypes.Room>();
	
	public bool AddHost(string gameName, string comment, string hostIp, int hostPort, int connectionId)
	{
		if (rooms.ContainsKey(gameName))
		{
			return false;
		}
		
		MasterMsgTypes.Room room = new MasterMsgTypes.Room();
		room.name = gameName;
		room.comment = comment;
		room.hostIp = hostIp;
		room.hostPort = hostPort;
		room.connectionId = connectionId;
		rooms[gameName] = room;
		
		return true;
	}
	
	public MasterMsgTypes.Room[] GetRooms()
	{
		return rooms.Values.ToArray();
	}
}


