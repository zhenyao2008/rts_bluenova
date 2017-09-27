using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
				Debug.Log ("Sample1Panel is created.");
			}
			mBuildingItems = new List<GameObject> ();
			//TODO
			for(int i=0;i<8;i++){
				GameObject item = Instantiate(mBuildingListPanelView.building_item) as GameObject;
				item.transform.SetParent (mBuildingListPanelView.grid_building_list.transform);
				mBuildingItems.Add (item);
				SetItem (item.transform,i);
			}
		}

		//TODO
		void SetItem(Transform item,int id){
			Image img_icon = item.Find ("img_icon").GetComponent<Image>();
			Text txt_cost = item.Find ("txt_cost").GetComponent<Text> ();
			Text txt_name = item.Find ("txt_name").GetComponent<Text> ();
			BuildingCSVStructure buildingCSVStructure = CSVManager.GetInstance.GetBuildingById (id);
			img_icon.sprite = ResourcesManager.GetInstance.GetBuildingFullIconById (id);
			txt_cost.text = buildingCSVStructure.building_cost.ToString ();
			txt_name.text = buildingCSVStructure.building_name;
		}

	}
}
