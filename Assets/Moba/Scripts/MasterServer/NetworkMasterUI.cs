using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMasterUI : MonoBehaviour {

	public GameObject roomListRoot;
	public List<UIButton> roomListButtons;

	public GameObject playerListRoot;
	public List<UIButton> playerListButtons;

	public UIButton launchButton;

	NetworkMasterClient mNetworkMasterClient;

	void Start(){
		mNetworkMasterClient = FindObjectOfType<NetworkMasterClient>();
		for(int i=0;i<roomListButtons.Count;i++)
		{
			roomListButtons[i].onClick.Add(new EventDelegate(OnRoomButtonClick));
		}
		mNetworkMasterClient.onJoinedRoomEvent += OnJoinedRoom;
		launchButton.onClick.Add (new EventDelegate (mNetworkMasterClient.RequestLaunchBattle));
//		launchButton.onClick.Add (new EventDelegate (Test));
	}

	public const string battleFileName = @"D:/Projects/Moba2/Moba";
	void Test(){
		#if UNITY_STANDALONE
		System.Diagnostics.Process.Start(battleFileName);
#endif
	}

	void OnRoomButtonClick()
	{
		UIButton uiBtn = UIButton.current;
		if(roomListButtons.Contains(uiBtn))
		{
			mNetworkMasterClient.RequestJoinRoom(roomListButtons.IndexOf(uiBtn));
			roomListRoot.SetActive(false);
			playerListRoot.SetActive(true);
		}
	}

//	MasterMsgTypes.Room mRoom;
	void OnJoinedRoom(MasterMsgTypes.Room room)
	{
//		mRoom = room;
		MasterMsgTypes.RoomMember[] roomMembers = room.rms;

		foreach(UIButton uiButton in playerListButtons)
		{
			uiButton.isEnabled = true;
			uiButton.GetComponentInChildren<UILabel>().text = "Empty";
		}

		for(int i=0;i<roomMembers.Length;i++)
		{
			if(playerListButtons.Count > roomMembers[i].locationId)
			{
				playerListButtons[roomMembers[i].locationId].isEnabled = false;
				playerListButtons[roomMembers[i].locationId].GetComponentInChildren<UILabel>().text = "Player";
			}
		}
	}

}
