using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
	public class Sample2Ctrl : BaseCtrl
	{
		Sample2Panel mSample2Panel;
		public override void ShowPanel (Hashtable parameters)
		{
			base.ShowPanel (parameters);
			bool isCreate;
			mSample2Panel = UIMgr.ShowPanel<Sample2Panel> (UIManager.UILayerType.Common, out isCreate);
			if (isCreate) {
				Debug.Log ("Sample2Panel is created.");
			}
		}
	}
}
