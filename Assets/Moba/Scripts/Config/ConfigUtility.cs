using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigUtility {

	public static BattleConfig systemConfig;

	const string SYSTEM_CONFIG_PATH = "configs/SystemConfig";

	static ConfigUtility(){
		systemConfig = JsonUtility.FromJson<BattleConfig> (SYSTEM_CONFIG_PATH);
	}
}

[System.Serializable]
public class BattleConfig{
	//今度コーンを増える時間帯
	public float moneyPlusInterval;

	public int moneyPlusPer; 

}
