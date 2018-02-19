using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrame
{
	public class BuildingDetailPanelView : PanelBase {

		public GridLayoutGroup grid_unit_detail;
		public Button btn_upgrade;
		public Button btn_upgrade1;
		public Transform unit_tree;
		public Transform arrow1;
		public Transform arrow2;
		public Transform arrow3;
		public Transform arrow4;
		public Transform unit_point1;
		public Transform unit_point2;
		public Transform unit_point3;
		public Transform unit_point4;
		public Transform unit_point5;

		public override void Awake()
		{
			grid_unit_detail = transform.Find ("grid_unit_detail").GetComponent<GridLayoutGroup>();
			btn_upgrade = transform.Find ("btn_upgrade").GetComponent<Button>();
			btn_upgrade1 = transform.Find ("btn_upgrade1").GetComponent<Button> ();
			unit_tree = transform.Find ("unit_tree");
			arrow1 = transform.Find ("arrow1");
			arrow2 = transform.Find ("arrow2");
			arrow3 = transform.Find ("arrow3");
			arrow4 = transform.Find ("arrow4");
			unit_point1 = transform.Find ("unit_point1");
			unit_point2 = transform.Find ("unit_point2");
			unit_point3 = transform.Find ("unit_point3");
			unit_point4 = transform.Find ("unit_point4");
			unit_point5 = transform.Find ("unit_point5");
		}

		void OnDestroy()
		{
			
		}

	}
}
