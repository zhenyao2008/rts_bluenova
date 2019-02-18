using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
	public class Sample1Ctrl : BaseCtrl
	{
		Sample1Panel mSample1Panel;
		public override void ShowPanel (Hashtable parameters)
		{
			base.ShowPanel (parameters);
			bool isCreate;
			mSample1Panel = UIMgr.ShowPanel<Sample1Panel> (UIManager.UILayerType.Common, out isCreate);
			if (isCreate) {
				Debug.Log ("Sample1Panel is created.");
			}
		}
	}
}
