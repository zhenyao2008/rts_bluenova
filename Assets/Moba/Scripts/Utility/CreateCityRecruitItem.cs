using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class CreateCityRecruitItem : MonoBehaviour {

	public CityRecruitPanel cityRecruitPanel;
	public GameObject itemPrefab;

	public Vector3 startPos0;
	public Vector3 startPos1;
	public Vector3 padding;

	public bool load;

	void Update()
	{
		if(load)
		{
			for(int i =0;i< transform.childCount;i++)
			{
				DestroyImmediate(transform.GetChild(i).gameObject);
			}
			if(cityRecruitPanel.recruitTriggers!=null)
				cityRecruitPanel.recruitTriggers.Clear();
			for(int i=0;i<4;i++)
			{
				GameObject go = Instantiate(itemPrefab) as GameObject;
				go.transform.parent = transform;
				go.transform.localPosition = startPos0 + padding * i;
				go.transform.localScale = Vector3.one;
				cityRecruitPanel.recruitTriggers.Add(go.GetComponent<CityRecruitItem>());

			}
			for(int i=0;i<4;i++)
			{
				GameObject go = Instantiate(itemPrefab) as GameObject;
				go.transform.parent = transform;
				go.transform.localPosition = startPos1 + padding * i;
				go.transform.localScale = Vector3.one;
				cityRecruitPanel.recruitTriggers.Add(go.GetComponent<CityRecruitItem>());
				
			}
			load = false;
		}
	}
}
