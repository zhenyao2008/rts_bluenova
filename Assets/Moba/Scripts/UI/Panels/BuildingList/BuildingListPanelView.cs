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
			base.Awake ();
		}

		void OnDestroy()
		{
			grid_building_list = null;
			building_item = null;
		}

	}
}
