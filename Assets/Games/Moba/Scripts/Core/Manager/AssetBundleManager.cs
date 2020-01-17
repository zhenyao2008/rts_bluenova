using UnityEngine;
using System.Collections;

public class AssetBundleManager : MonoBehaviour {

	static AssetBundleManager instance;

	public static AssetBundleManager SingleTon()
	{
		if (instance == null) {
			GameObject go = new GameObject ("_AssetBundleManager");
			instance = go.AddComponent<AssetBundleManager>();
			DontDestroyOnLoad(go);
		}
		return instance;
	}

//	string pathURL =
//		#if UNITY_ANDROID
//		"jar:file://" + Application.dataPath + "!/assets/";
//	#elif UNITY_IPHONE
//	Application.dataPath + "/Raw/";
//	#else
//	"file://" + Application.dataPath + "/StreamingAssets/";
//	#endif 

	public string localLuaAssetBundlePath = "/StreamingAssets/uLua/";

	public bool isLoadFromFile = true;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	public AssetBundle LoadFromFile(string fileName)
	{
		AssetBundle ab;
		ab = AssetBundle.LoadFromFile (Application.dataPath + localLuaAssetBundlePath + fileName);
		Debug.Log (Application.dataPath + localLuaAssetBundlePath + fileName);
		return ab;
	}








}
