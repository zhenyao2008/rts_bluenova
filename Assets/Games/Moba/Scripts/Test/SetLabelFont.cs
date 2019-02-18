using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetLabelFont : MonoBehaviour {

	public bool isSetFont;
	public Font font;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isSetFont){
			isSetFont = false;
			UILabel[] labels = gameObject.GetComponentsInChildren<UILabel> (true);
			for(int i=0;i<labels.Length;i++){
				labels [i].trueTypeFont = font;
			}
		}
	}
}
