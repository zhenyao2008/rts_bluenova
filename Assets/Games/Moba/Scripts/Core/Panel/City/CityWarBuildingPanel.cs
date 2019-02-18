using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CityWarBuildingPanel : CityBasePanel_I {

	public Button closeButton;
	public Button returnButton;
	
	static CityWarBuildingPanel instance;
	public static CityWarBuildingPanel SingleTon(){
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
