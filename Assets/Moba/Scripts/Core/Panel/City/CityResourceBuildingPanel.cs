using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class CityResourceBuildingPanel : CityBasePanel_I {

	public Button closeButton;
	public Button returnButton;

	public List<CityBuildingItem> items;

	static CityResourceBuildingPanel instance;
	public static CityResourceBuildingPanel SingleTon(){
		return instance;
	}

	void Awake()
	{
		instance = this;
		closeButton.onClick.AddListener (new UnityEngine.Events.UnityAction (OnCloseButtonClick));
		returnButton.onClick.AddListener (new UnityEngine.Events.UnityAction (OnReturnButtonClick));
	}



	void OnCloseButtonClick()
	{
		CityPanel_I.SingleTon ().root.SetActive (true);
		root.SetActive (false);
	}

	void OnReturnButtonClick()
	{
		prePanel.root.SetActive (true);
		root.SetActive (false);
	}

}
