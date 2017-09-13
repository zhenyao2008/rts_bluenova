using UnityEngine;
using System.Collections;

public class BuildingItemUI : MonoBehaviour {

//	public bool isEnable;
	public Transform buildingPrefabPoint;
	public UISprite backSprite;
	public UISprite frantSprite;
	public UILabel buildingName;
	public UIEventTrigger detailTrigger;
	public CityBuilding currentBuilding;

	public PanelBase basePanel;

	public UIEventTrigger trigger;
	public UISprite sprite;
	void Awake(){
		trigger = GetComponent<UIEventTrigger> ();
		sprite = GetComponent<UISprite> ();
		trigger.onClick.Add (new EventDelegate(OnItemClick));
		detailTrigger.onClick.Add (new EventDelegate (OnDetailClick));
//		if(!isEnable)
//		{
//			sprite.color = Color.gray;
//		}
	}

	void OnItemClick(){
		if (currentBuilding) {
			if(BuildingController.SingleTon())
			{
				if(BuildingController.SingleTon().isNewBuilding)
				{
					if(BuildingController.SingleTon().currentBuilding)
						Destroy(BuildingController.SingleTon().currentBuilding);
				}
				else
					BuildingController.SingleTon().DeSelect();

				GameObject go = Instantiate(currentBuilding.gameObject) as GameObject;
				BuildingController.SingleTon().SetPreBuilding(go);
				BuildingController.SingleTon().isNewBuilding = true;
				BuildingController.SingleTon().isNewBuildingFirstClick = true;

				if(basePanel)basePanel.Close();
				CityPanel.SingleTon().Active();
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit,Mathf.Infinity,1<<15))
				{
					go.transform.position = hit.point + new Vector3(0,0.15f,0);
					go.GetComponent<CityBuilding>().Select();
					go.GetComponent<Collider>().enabled = true;
					CityPanel.SingleTon().buildConfirm.SetActive(true);
				}
				BuildingController.SingleTon().CheckPlaceAble();
				if(!BuildingController.SingleTon().CheckPlaceAble())
				{
					BuildingController.SingleTon().currentBuilding.GetComponent<CityBuilding>().ShowDisable();
					CityPanel.SingleTon().buildConfirmYesBtn.isEnabled = false;
				}


			}
		}
	}

	void OnDetailClick(){
	
	}

	public void SetBuilding(CityBuilding building)
	{
		this.currentBuilding = building;
		buildingName.text = building.buildingName;
	}

	public void DisableItem()
	{
		if(!trigger)trigger = GetComponent<UIEventTrigger> ();
		if(!sprite)sprite = GetComponent<UISprite> ();
		trigger.enabled = false;
		detailTrigger.enabled = false;
		sprite.color = Color.gray;
		detailTrigger.GetComponent<UISprite> ().color = Color.gray;
	}

	public void EnableItem()
	{
		if(!trigger)trigger = GetComponent<UIEventTrigger> ();
		if(!sprite)sprite = GetComponent<UISprite> ();
		trigger.enabled = true;
		detailTrigger.enabled = true;
		sprite.color = new Color(176/255.0f,150/255.0f,104/255.0f);
		detailTrigger.GetComponent<UISprite> ().color = new Color(176/255.0f,150/255.0f,104/255.0f);
	}
}
