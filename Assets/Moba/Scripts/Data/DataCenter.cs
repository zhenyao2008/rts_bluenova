using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleFramework.Data;
using Newtonsoft.Json;

public class DataCenter : MonoBehaviour {

	static DataCenter instance;

	//各种普通攻击的ID常量，用于从上面的Dictionary里面取数据
	public SysConfig sysConfig;

	public List<Unit_Info> unitInfos;
	public UserInfo userInfo;
	public List<BuildingInfo> buildingInfoList;
	public List<UserBuilding> userBuildingList;

	public static DataCenter Instance()
	{
		if (instance == null) {
			instance = GameObject.FindObjectOfType<DataCenter>();
			if(instance == null)
			{
				GameObject go = new GameObject("_DataCenter");
				instance = go.AddComponent<DataCenter>();
			}
			GameObject.DontDestroyOnLoad(instance.gameObject);
			instance.Init();
//			RealTime.deltaTime;
		}
		return instance;
	}

	public void Init()
	{
//		battleShips = BattleShip.LoadDatas();

		sysConfig = new SysConfig ();
	}




	public List<string> buildings;
	public List<string> buildingRes;
	public List<Vector3> buildingPos;

	public void SaveCityBuilding(List<GameObject> buildingGos)
	{
		buildings = new List<string> ();
		buildingRes = new List<string> ();
		buildingPos = new List<Vector3> ();
		for(int i=0;i< buildingGos.Count;i ++)
		{
			string buildingKey = buildingGos[i].GetComponent<CityBuilding>().buildingName + i;
			buildings.Add(buildingKey);
			buildingRes.Add(buildingGos[i].GetComponent<CityBuilding>().buildingName);
			buildingPos.Add(buildingGos[i].transform.position);
		}
//		ES2.Save(buildings,"buildings");
//		ES2.Save(buildingRes,"buildingRes");
//		ES2.Save(buildingPos,"buildingPos");
	}



	public void SaveUserInfo(UserInfo userInfo){
//		ES2.Save (userInfo.userName, "userName");
//		ES2.Save (userInfo.userLevel, "userLevel");
//		ES2.Save (userInfo.userExp, "userExp");
//		ES2.Save (userInfo.workNum, "workNum");
//		ES2.Save (userInfo.currentCorn, "currentCorn");
//		ES2.Save (userInfo.currentBaoShi, "currentBaoShi");
//		ES2.Save (userInfo.currentStone, "currentStone");
//		ES2.Save (userInfo.currentWood, "currentWood");
//		ES2.Save (userInfo.maxCorn, "maxCorn");
//		ES2.Save (userInfo.maxStone, "maxStone");
//		ES2.Save (userInfo.maxWood, "maxWood");
	}
//
	public void LoadUserInfo(){
//		userInfo = new UserInfo ();
//		if(ES2.Exists("userName"))
//			userInfo.userName = ES2.Load<string>("userName");
//		if(ES2.Exists("userLevel"))
//			userInfo.userLevel = ES2.Load<int>("userLevel");
//		if(ES2.Exists("userExp"))
//			userInfo.userExp = ES2.Load<int>("userExp");
//		if(ES2.Exists("workNum"))
//			userInfo.workNum = ES2.Load<int>("workNum");
//		if(ES2.Exists("currentWood"))
//			userInfo.currentWood = ES2.Load<int>("currentWood");
//		if(ES2.Exists("currentStone"))
//			userInfo.currentStone = ES2.Load<int>("currentStone");
//		if(ES2.Exists("currentBaoShi"))
//			userInfo.currentBaoShi = ES2.Load<int>("currentBaoShi");
//		if(ES2.Exists("currentCorn"))
//			userInfo.currentCorn = ES2.Load<int>("currentCorn");
//		if(ES2.Exists("maxWood"))
//			userInfo.maxWood = ES2.Load<int>("maxWood");
//		if(ES2.Exists("maxStone"))
//			userInfo.maxStone = ES2.Load<int>("maxStone");
//		if(ES2.Exists("maxWood"))
//			userInfo.maxWood = ES2.Load<int>("maxWood");
	}



	public void LoadCityBuilding(){
//		if(ES2.Exists("buildings"))
//			buildings = ES2.LoadList<string> ("buildings");
//		if(ES2.Exists("buildingRes"))
//			buildingRes = ES2.LoadList<string>("buildingRes");
//		if(ES2.Exists("buildingPos"))
//			buildingPos = ES2.LoadList<Vector3> ("buildingPos");
	}

}

