using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigUtility {

	public static SystemConfig systemConfig;

	const string SYSTEM_CONFIG_PATH = "configs/SystemConfig";

	static ConfigUtility(){
		systemConfig = JsonUtility.FromJson<SystemConfig> (SYSTEM_CONFIG_PATH);
	}
}

[System.Serializable]
public class SystemConfig{
	//今度コーンを増える時間帯
	public float moneyPlusInterval;

	public int moneyPlusPer; 

}
