using UnityEngine;
using System.Collections;

[System.Serializable]
public class SystemConfig{

    static SystemConfig mInstance;
    public static SystemConfig Instance{
        get
        {
            if (mInstance == null)
            {
                string configText = Resources.Load<TextAsset>("Configs/NewSysConfig/SystemConfig").text;
                mInstance = JsonUtility.FromJson<SystemConfig>(configText);
            }
            return mInstance;
        }
    }

	public int defaultMoveSpeed = 5;
	public string removeIp = "127.0.0.1";
	public int removePort = 8808;
	public string[] rips;

    public int battleSpawnInterval;
}
