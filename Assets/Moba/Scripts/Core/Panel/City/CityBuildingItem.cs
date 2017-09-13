using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CityBuildingItem : MonoBehaviour {

	public GameObject frant;
	public GameObject back;
	public UTweenRotation tweenRotation;

	public Image buildingIcon;
	public Text buildingName;
	public Text buildingNum;
	public Button detailButton;
	public Button backButton;
	public Button selectButton;
	public CityBuilding  building;
	public CityBasePanel_I parentPanel;
	void Awake(){
		tweenRotation = GetComponent<UTweenRotation>();
		detailButton.onClick.AddListener (new UnityEngine.Events.UnityAction(OnDetailButtonClick));
		backButton.onClick.AddListener (new UnityEngine.Events.UnityAction(OnBackButtonClick));
		selectButton.onClick.AddListener (new UnityEngine.Events.UnityAction (OnItemClick));
	}

	public void OnDetailButtonClick()
	{
		detailButton.enabled = false;
		tweenRotation.PlayForward ();
		float delay = tweenRotation.duration / 2;
		StartCoroutine (_Forward(delay));
	}

	void OnItemClick()
	{

		if (building) {
			if(BuildingController.SingleTon())
			{
				if(BuildingController.SingleTon().isNewBuilding)
				{
					if(BuildingController.SingleTon().currentBuilding)
						Destroy(BuildingController.SingleTon().currentBuilding);
				}
				else
					BuildingController.SingleTon().DeSelect();
				
				GameObject go = Instantiate(building.gameObject) as GameObject;
				BuildingController.SingleTon().SetPreBuilding(go);
				BuildingController.SingleTon().isNewBuilding = true;
				BuildingController.SingleTon().isNewBuildingFirstClick = true;
				BuildingController.SingleTon().ShowPlane();
//				go.GetComponent<CityBuilding>()
//				CityPanel.SingleTon().Active();
				CityPanel_I.SingleTon().root.SetActive(true);
				CityPanel_I.SingleTon().confirmBtns.gameObject.SetActive(true);
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit,Mathf.Infinity,1<<15))
				{
					go.transform.position = hit.point + new Vector3(0,0.15f,0);
					go.GetComponent<CityBuilding>().Select();
					go.GetComponent<Collider>().enabled = true;
//					CityPanel.SingleTon().buildConfirm.SetActive(true);
				}
				BuildingController.SingleTon().CheckPlaceAble();
				if(!BuildingController.SingleTon().CheckPlaceAble())
				{
					BuildingController.SingleTon().currentBuilding.GetComponent<CityBuilding>().ShowDisable();
//					CityPanel.SingleTon().buildConfirmYesBtn.isEnabled = false;
				}
				parentPanel.root.SetActive(false);
			}
		}
	}



	IEnumerator _Forward(float delay)
	{
		yield return new WaitForSeconds(delay);
		frant.SetActive (false);
		back.SetActive (true);
		backButton.enabled = true;
	}

	public void OnBackButtonClick()
	{
		backButton.enabled = false;
		tweenRotation.PlayRevert ();
		float delay = tweenRotation.duration / 2;
		StartCoroutine (_Revert(delay));
	}

	IEnumerator _Revert(float delay)
	{
		yield return new WaitForSeconds(delay);
		back.SetActive (false);
		frant.SetActive (true);
		detailButton.enabled = true;
	}
}
