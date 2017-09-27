using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrame
{
	public class BuildingListPanelView : PanelBase
	{

		public GridLayoutGroup grid_building_list;
		public GameObject building_item;

		public override void Awake()
		{
			grid_building_list = transform.Find ("grid_building_list").GetComponent<GridLayoutGroup>();
			building_item = grid_building_list.transform.Find ("building_item").gameObject;
		}

		void OnDestroy()
		{
			grid_building_list = null;
			building_item = null;
		}

	}
}
