using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MobaController1 : NetworkManager {

	public Camera uiCamera;
	NetworkManagerHUD nm;
	public bool isServer;


	void Awake()
	{
		if(isServer)this.StartServer ();
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);
		GetComponent<NetworkManagerHUD> ().enabled = false;

	}


}
