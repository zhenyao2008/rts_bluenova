using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RegisterPanel : CityBasePanel_I {

	public InputField userName;
	public Button confirm;

	static RegisterPanel instance;
	public static RegisterPanel SingleTon(){
		return instance;
	}

	void Awake(){
		instance = this;
		confirm.onClick.AddListener (new UnityEngine.Events.UnityAction (Confirm));
	}

	public void Confirm()
	{
		UserInfo userInfo = new UserInfo ();
		userInfo.userName = userName.text;

	}


}
