using UnityEngine;
using System.Collections;

public class CityRecruitItem : MonoBehaviour {

	public UIEventTrigger trigger;
	public int num;
	public UILabel numText;
	public Transform prefabPoint;
	public UnitProperties unitPropertis;

	public void Add()
	{
		num ++;
		numText.text = num + "x";
	}
}
