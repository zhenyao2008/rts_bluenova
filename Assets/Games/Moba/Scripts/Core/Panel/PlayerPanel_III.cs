using UnityEngine;
using System.Collections;

public class PlayerPanel_III : MonoBehaviour {

	public GameObject root;
	public UILabel corn;
	public UILabel killNum;
	public UILabel timeLimit;
	public UILabel message;

	public void ShowMsg(string msg)
	{
		message.text = msg;
		UITweener uiTweener = message.GetComponent<UITweener> ();
		uiTweener.ResetToBeginning ();
		uiTweener.enabled = true;
	}

}
