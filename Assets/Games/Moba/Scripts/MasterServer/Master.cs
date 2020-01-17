using UnityEngine;
using UnityEngine.Networking;

public class MasterMsgTypes
{
	public enum NetworkMasterServerEvent
	{
		RegistrationFailedGameName, // Registration failed because an empty game name was given.
		RegistrationFailedGameType, // Registration failed because an empty game type was given.
		RegistrationFailedNoServer, // Registration failed because no server is running.
		RegistrationSucceeded, // Registration to master server succeeded, received confirmation.
		UnregistrationSucceeded, // Unregistration to master server succeeded, received confirmation.
		HostListReceived, // Received a host list from the master server.
	}

	// -------------- battleserver to masterserver ------------
	public const short BattleServerRegisterMessageId = 173;


	public const short RequestBattleServerPortMessageId = 170;
	public const short BattleServerReadyMessageId = 171;

	// -------------- masterserver to battleserver ------------
	public const short BattleServerRegisteredMessageId = 183;



	public const short ResponseBattleServerPortMessageId = 180;
	public const short BattleServerReadyedMessageId = 181;

	// -------------- client to masterserver Ids --------------

	public const short RegisterHostId = 150;
	public const short UnregisterHostId = 151;
	public const short RequestListOfHostsId = 152;
	public const short JoinRoom = 153;
	public const short ChangeLocation = 154;
	public const short LaunchBattle = 155;

	public const short UserLoginId = 156;
	public const short UserRegisterId = 157;
	public const short UserRequestOnlinePlayId = 158;
	public const short UserRequestUserListId = 159;

	// -------------- masterserver to client Ids --------------

	public const short RegisteredHostId = 160;
	public const short UnregisteredHostId = 161;
	public const short ListOfHostsId = 162;
	public const short JoinedRoom = 163;
	public const short ChangedLocation = 164;
	public const short LaunchedBattle = 165;

	public const short UserLoginedId = 166;
	public const short UserRegisteredId = 167;
	public const short UserRequestedOnlinePlayId = 168;
	public const short UserRequestedUserListId = 169;
	// -------------- client to server messages --------------

	public class UserLoginMessage:MessageBase
	{
		public string userName;
		public string password;
	}

	public class UserLoginedMessage:MessageBase
	{
		public bool isLogin;
		public string message;
	}

	public class UserListMessage:MessageBase
	{
		public string[] userNames;
	}


	public class BattleServerReadyMessage:MessageBase
	{
		public int port;
	}

	public class RequestBattleServerPortMessage:MessageBase
	{

	}

	public class ResponseBattleServerPortMessage:MessageBase
	{
		public int port;
	}

	public class LaunchBattleMessage:MessageBase
	{
	}

	public class LaunchedBattleMessage:MessageBase
	{
		public bool isReady;
		public int port;
	}

	public class JoinRoomMessage : MessageBase
	{
		public int roomId;
	}

	public class JoinedRoomMessage : MessageBase
	{
		public bool joined;
		public string errorMsg;
		public Room room;
	}

	public struct RoomMember{
		public int locationId;
		public int connectionId;
		public string memberName;
	}

	public class RegisterHostMessage : MessageBase
	{
		public string gameTypeName;
		public string gameName;
		public string comment;
		public bool passwordProtected;
		public int playerLimit;
		public int hostPort;
	}

	public class UnregisterHostMessage : MessageBase
	{
		public string gameTypeName;
		public string gameName;
	}

	public class RequestHostListMessage : MessageBase
	{
		public string gameTypeName;
	}

	// -------------- server to client messages --------------

	public struct Room
	{
		public string name;
		public bool isStarted;
		public string comment;
		public bool passwordProtected;
		public int playerLimit;
		public string hostIp;
		public int hostPort;
		public int connectionId;
		public RoomMember[] rms;
		public int maxRoomMember ; 
	}

	public class ListOfHostsMessage : MessageBase
	{
		public int resultCode;
		public Room[] hosts;
	}

	public class RegisteredHostMessage : MessageBase
	{
		public int resultCode;
	}
}
