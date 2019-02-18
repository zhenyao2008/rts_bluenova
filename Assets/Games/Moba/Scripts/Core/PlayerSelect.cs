using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
public class PlayerSelect : NetworkBehaviour {

	public List<UIButton> uiButtons;

	public List<int> actives;

	void Start()
	{
		if (NetworkClient.active) {
			foreach(UIButton usButton in uiButtons)
			{
				usButton.onClick.Add(new global::EventDelegate(SelectHero));
			}
		}
	}

	public void InActiveButton(int index){
		actives.Add (index);
		CheckActiveButton ();
	}
	public void ActiveButton(int index){
		actives.Remove (index);
		CheckActiveButton ();
	}

	[ClientRpc]
	public void RpcInActiveButton(int index){
		uiButtons [index].isEnabled = false;
	}

	[ClientRpc]
	public void RpcActiveButton(int index){
		uiButtons [index].isEnabled = true;
	}

	public void CheckActiveButton(){
		for(int i =0;i<uiButtons.Count;i++)
		{
			if(actives.Contains(i))
			{
				RpcInActiveButton(i);
			}
			else
			{
				RpcActiveButton(i);
			}
		}
	}

	public void SelectHero()
	{
		int index = uiButtons.IndexOf (UIButton.current);
		PlayerController pc = PlayerController.GetLocalPlayer ();
		pc.CmdSelectHero (index);
		gameObject.SetActive (false);
	}

}
