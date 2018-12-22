using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIFrame
{
	public class PanelBase : MonoBehaviour
	{

		public static PanelBase current;
		public GameObject root;
		public Button btn_back;
		public Button btn_close;
		public PanelBase preBuildingPanel;

		public virtual void Awake ()
		{
            root = transform.Find("Root").gameObject;
            btn_close = transform.Find("Root/btn_close").GetComponent<Button>();
			if (btn_close != null)
				btn_close.onClick.AddListener (Close);
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
			Debug.Log ("Close");
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
