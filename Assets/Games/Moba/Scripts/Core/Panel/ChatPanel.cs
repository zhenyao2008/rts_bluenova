using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ChatPanel : NetworkBehaviour{

	public GameObject root;
	public UILabel chatLabel;
	public UIInput chatInput;

	public UISprite chatOutputSprite;
	public UIButton controllBtn;

	public UIButton closeBtn;
	public UIButton sendBtn;

	public List<string> availChats;
	public int maxLine = 15;
	public int chatPerLine = 20;


	void Awake(){
//		transform.SetParent(FindObjectOfType<UICamera> ().transform);
//		transform.localPosition = Vector3.zero;
//		transform.localScale = Vector3.one;
		availChats = new List<string> ();


	}

	public void ServerAddChatMsg(string msg){
		RpcAddChatMsg (msg);
	}

	public void Show(){
		chatOutputSprite.enabled = true;
		controllBtn.gameObject.SetActive (false);
		sendBtn.gameObject.SetActive (true);
		closeBtn.gameObject.SetActive (true);
		chatInput.gameObject.SetActive (true);
	}

	public void Hide(){
		chatOutputSprite.enabled = false;
		controllBtn.gameObject.SetActive (true);
		sendBtn.gameObject.SetActive (false);
		closeBtn.gameObject.SetActive (false);
		chatInput.gameObject.SetActive (false);
	}

	public void AddMsg(string msg){
		int line = msg.Length % chatPerLine > 0 ? msg.Length / chatPerLine + 1 : msg.Length / chatPerLine;
		for(int i = 0;i<line;i++)
		{
			if(i == line - 1)
			{
				availChats.Add(msg.Substring(chatPerLine*i,msg.Length - chatPerLine*i));
			}
			else
			{
				availChats.Add(msg.Substring(chatPerLine*i,chatPerLine));
			}
		}
		int removeLine = availChats.Count - maxLine;
		for(int i = 0;i<removeLine;i++)
		{
			availChats.RemoveAt(0);
		}
		StringBuilder stringBuilder = new StringBuilder();
		for(int i=0;i<availChats.Count;i++)
		{
			stringBuilder.Append(availChats[i]);
			stringBuilder.Append("\r\n");
		}
		chatLabel.text = stringBuilder.ToString ();


	}



	[ClientRpc]
	public void RpcAddChatMsg(string msg){
		AddChatMsg (msg);
	}

	StringBuilder stringBuilder = new StringBuilder();

	void AddChatMsg(string msg){
		stringBuilder.Append (msg);
		if (stringBuilder.Length > 10000) {
			stringBuilder.Remove(0,1000);
		}
		chatLabel.text = stringBuilder.ToString();
	}


//	public void CmdSubmitChat()
//	{
//		chatText += chatInput.value + "\n";
//	}
}
