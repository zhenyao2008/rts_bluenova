using UnityEngine;
using System.Collections;

public class NGUIScreen : MonoBehaviour {

	public UIRoot root;
	int defaultWidth;
	int defaultHeight;

	// Use this for initialization
	void Start () {
		defaultWidth = root.manualWidth;
		defaultHeight = root.manualHeight;
		float radio = (defaultWidth / (float)defaultHeight) / (Screen.width / (float)Screen.height); 
		transform.localScale = new Vector3(1,radio,transform.localScale .z);
	}
}
