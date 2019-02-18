using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CityShopPanel : CityBasePanel_I {

	public Button closeButton;

	public Button baozangButton;
	public Button ziyuanButton;
	public Button zhuangshiButton;
	public Button junduiButton;
	public Button fangyuButton;
	public Button hudunButton;

	static CityShopPanel instance;
	public static CityShopPanel SingleTon()
	{
		return instance;
	}

	void Awake()
	{
		instance = this;
		baozangButton.onClick.AddListener (new UnityEngine.Events.UnityAction (OnBaoZhangButtonClick));
		ziyuanButton.onClick.AddListener (new UnityEngine.Events.UnityAction (OnZiYuanButtonClick));
		junduiButton.onClick.AddListener (new UnityEngine.Events.UnityAction (OnJunDuiButtonClick));
		closeButton.onClick.AddListener (new UnityEngine.Events.UnityAction (OnCloseButtonClick));
	}

	void OnBaoZhangButtonClick()
	{
		#if UNITY_STANDALONE
		Debug.Log ("OnBaoZhangButtonClick");
		#endif
	}

	void OnZiYuanButtonClick()
	{
		#if UNITY_STANDALONE
		Debug.Log ("OnZiYuanButtonClick");
		#endif
		CityResourceBuildingPanel.SingleTon ().root.SetActive (true);
		CityResourceBuildingPanel.SingleTon ().prePanel = this;
		root.SetActive (false);
	}

	void OnJunDuiButtonClick()
	{
		#if UNITY_STANDALONE
		Debug.Log ("OnJunDuiButtonClick");
		#endif
		CityWarBuildingPanel.SingleTon ().root.SetActive (true);
		CityWarBuildingPanel.SingleTon ().prePanel = this;
		root.SetActive (false);
	}

	
	void OnCloseButtonClick()
	{
		CityPanel_I.SingleTon ().root.SetActive (true);
		root.SetActive (false);
	}




}
