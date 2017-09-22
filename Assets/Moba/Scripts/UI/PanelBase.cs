using UnityEngine;
using System.Collections;

namespace UIFrame
{
	public class PanelBase : MonoBehaviour
	{

		public static PanelBase current;
		public GameObject root;
		public PanelBase preBuildingPanel;

		public virtual void Awake ()
		{

		}

		protected virtual void Start ()
		{
		
		}

		public virtual void Active ()
		{
			root.SetActive (true);
			current = this;
		}

		public void Close ()
		{
			root.SetActive (false);
		}

		void Return ()
		{
			if (preBuildingPanel) {
				preBuildingPanel.Active ();
				root.SetActive (false);
			}
		}
	}
}
