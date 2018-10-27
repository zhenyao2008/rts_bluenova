using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIFrame
{
	public class BuildingListCtrl : BaseCtrl
	{

		BuildingListPanelView mBuildingListPanelView;
		List<GameObject> mBuildingItems;

		public override void ShowPanel (Hashtable parameters)
		{
			base.ShowPanel (parameters);
			bool isCreate;
			mBuildingListPanelView = UIMgr.ShowPanel<BuildingListPanelView> (UIManager.UILayerType.Common, out isCreate);
			if (isCreate) {
				List<GameObject> buildings = PlayerController_III.instance.buildings;
				mBuildingItems = new List<GameObject> ();
				for(int i=0;i < buildings.Count;i++){
					GameObject item = Instantiate(mBuildingListPanelView.building_item) as GameObject;
					item.transform.SetParent (mBuildingListPanelView.grid_building_list.transform);
					item.transform.localPosition = Vector3.zero;
					item.transform.localScale = Vector3.one;
					item.SetActive (true);
					mBuildingItems.Add (item);
					SetItem (item.transform,i,buildings[i].GetComponent<SpawnPoint> ());
				}
			}
			mBuildingListPanelView.root.SetActive (true);
		}

		void SetItem(Transform item,int id,SpawnPoint sp){

            BuildingListItem buildingListItem = item.gameObject.GetOrAddComponent<BuildingListItem>();
            buildingListItem.SetData(item,id,sp);


			//Image img_icon = item.Find ("Button/img_item").GetComponent<Image>();
			//Text txt_cost = item.Find ("Button/txt_cost").GetComponent<Text> ();
			//Text txt_name = item.Find ("Button/txt_name").GetComponent<Text> ();
			//txt_name.text = sp.buildingName;
			//txt_cost.text = sp.GetCurrentPrice ().ToString();
			//Text txt_warning = item.Find ("Button/txt_warning").GetComponent<Text> ();
			//EventTrigger trigger = item.Find ("Button").GetComponent<EventTrigger> ();
			//EventTrigger.Entry entry = new EventTrigger.Entry ();
			//entry.eventID = EventTriggerType.PointerDown;
			//entry.callback.AddListener ((data) => {
			//	PlayerController_III.instance.SelectPreBuilding (id);
			//});
			//trigger.triggers.Add (entry);
			//img_icon.sprite = ResourcesManager.Instance.GetBuildingFullIconById (id + 1);
			//BuildingCSVStructure buildingCSVStructure = CSVManager.Instance.GetBuildingById (id);
			//txt_cost.text = buildingCSVStructure.building_cost.ToString ();
			//txt_name.text = buildingCSVStructure.building_name;
//			txt_warning.text = "お金が足りない。";
		}

		public override void Close ()
		{
			base.Close ();
			mBuildingListPanelView.Close ();
		}

	}
}
