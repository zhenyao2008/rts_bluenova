using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityBuildingAttribute : MonoBehaviour {

	public string information = "TestTestTest";

	public bool speedUpAble;
	public int speedUpDurationMax;
	public int speedUpCurrent;

	public bool soldierAble;
	public List<UnitAttribute> units;


	public bool sellAble;


//	CityBuilding mCityBuilding;

	void Awake()
	{
//		mCityBuilding = GetComponent<CityBuilding> ();
	}


}
