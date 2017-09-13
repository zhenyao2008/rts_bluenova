using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

[ExecuteInEditMode]
public class Test1 : MonoBehaviour {

	public bool load;
	public Font font;

	void Update()
	{
		if(load){
			
			Text[] texts = GetComponentsInChildren<Text>(true);
			foreach(Text text in texts)
			{
				text.font = font;
			}
			load = false;
		}

	}

}
