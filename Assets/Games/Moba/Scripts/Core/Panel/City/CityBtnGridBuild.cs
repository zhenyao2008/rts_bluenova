using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CityBtnGridBuild : MonoBehaviour {

	public bool load;
	public bool setSize;
	public GameObject prefab;
	void Update()
	{
		if(load)
		{
			UIGrid grid = GetComponent<UIGrid>();
			List<Transform> tl = grid.GetChildList();
			foreach(Transform t in tl)
			{
				DestroyImmediate(t.gameObject);
			}

			for(int i = 0;i< 5;i++)
			{
				GameObject go = Instantiate(prefab) as GameObject;
				go.transform.parent = transform;
				go.GetComponent<UISprite>().width = 100;
				go.GetComponent<UISprite>().height = 100;
			}

			grid.Reposition();
			load = false;
		}
		if(setSize)
		{
			UIGrid grid = GetComponent<UIGrid>();
			for(int i=0;i<grid.GetChildList().Count;i++)
			{
				grid.GetChildList()[i].GetComponent<UISprite>().width = 100;
				grid.GetChildList()[i].GetComponent<UISprite>().height = 100;
				grid.GetChildList()[i].GetComponent<CityPanelItem>().tc.delay = i * 0.033f;
				grid.GetChildList()[i].GetComponent<CityPanelItem>().tp0.delay = i * 0.033f;
			}
			setSize = false;
		}
	}

}
