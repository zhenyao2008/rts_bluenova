using UnityEngine;
using System.Collections;

public class PanelBase : MonoBehaviour
{

	public static PanelBase current;
	public GameObject root;
	public UIButton closeButton;
	public UIButton returnButton;
	public PanelBase preBuildingPanel;

	public virtual void Awake ()
	{
		
	}


	protected virtual void Start ()
	{
		if (returnButton)
			returnButton.onClick.Add (new EventDelegate (Return));
		if (closeButton) {
			closeButton.onClick.Add (new EventDelegate (Close));
		}
	}

	public virtual void Active ()
	{
		root.SetActive (true);
		current = this;
	}

	public void Close ()
	{
		root.SetActive (false);
		if (CameraController.SingleTon () != null)
			CameraController.SingleTon ().enabled = true;
		if (EasyTouch.instance != null)
			EasyTouch.instance.enable = true;
		if (BuildingController.SingleTon () != null)
			BuildingController.SingleTon ().enabled = true;
		if (CityPanel.SingleTon () != null && CityPanel.SingleTon ().root != null)
			CityPanel.SingleTon ().root.SetActive (true);
	}

	void Return ()
	{
		if (preBuildingPanel) {
			preBuildingPanel.Active ();
			root.SetActive (false);
		}
	}
  
}
