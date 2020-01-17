using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingPanel : CityBasePanel_I {

	public Button saveButton;

	void Awake()
	{
		if(saveButton)
		{

		}
	}

	void Save()
	{
		DataCenter.Instance ().SaveUserInfo (CityPanel_I.SingleTon().userInfo);

	}

}
