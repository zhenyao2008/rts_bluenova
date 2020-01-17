using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CityRecruitPanel : MonoBehaviour {

	public List<UIEventTrigger> unitPrefabTriggers;
	public List<UnitProperties> unitPrefabs;
	
	public List<CityRecruitItem> recruitTriggers;
	public Dictionary<UnitProperties,CityRecruitItem> existUnitRecruit;

	public static CityRecruitPanel instance;
	public static CityRecruitPanel SingleTon(){
		return instance;
	}

	void Awake()
	{
		instance = this;
		for(int i=0;i<unitPrefabTriggers.Count;i++)
		{
			unitPrefabTriggers[i].onClick.Add(new EventDelegate(OnUnitPrefabTriggerClick));
		}
	}

	void OnUnitPrefabTriggerClick()
	{
//		int index = unitPrefabTriggers.IndexOf (UIEventTrigger.current);
//		UnitProperties up = unitPrefabs [index];
		recruitTriggers [0].Add ();

//		if(!existUnitRecruit.ContainsKey(up))
//		{
//			existUnitRecruit.Add(up);
//		}
//
//		for(int i =0;i<recruitTriggers.Count;i++)
//		{
//			CityRecruitItem cri = recruitTriggers[i].GetComponent<CityRecruitItem>();
//		}

	}




	void AddUnitRecruit()
	{

	}

}
