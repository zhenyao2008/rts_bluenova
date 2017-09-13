using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using Newtonsoft.Json;
using System;
using Framework;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CityController : MonoBehaviour {

	public Camera cameraUI;
	static CityController instance;
	public static CityController SingleTon()
	{
		return instance;
	}

    public MainInterfaceCtrl mainInterfaceCtrl;
    

    void Awake()
    {
        instance = this;
        //mainInterfaceCtrl = gameObject.AddComponent<MainInterfaceCtrl>();
        UIManager.Instance().mainInterfaceCtrl.ShowPanel();
    }



	void Start()
	{
		//LoadLua ();
		//InitBuildings ();
	}

#if !UNITY_EDITOR
	void LoadLua()
	{
		string pathURL =
			#if UNITY_ANDROID
			"jar:file://" + Application.dataPath + "!/assets/";
		#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
		#else
		"file://" + Application.dataPath + "/StreamingAssets001/";
		#endif 
		
		#region get from ab
		AssetBundleManager abm = AssetBundleManager.SingleTon ();
		AssetBundle ab = abm.LoadFromFile ("uluaentrance.lua");
		TextAsset ta = ab.LoadAsset<TextAsset>("uluaentrance");
		#endregion
		
		LuaScriptMgr mgr = new LuaScriptMgr ();
		mgr.Start ();
		mgr.DoString (ta.text);
		LuaTable luaTable = mgr.GetLuaTable ("luaFileList");
		string[] fileNames = luaTable.ToArray<string>();
		for(int i=0;i<fileNames.Length;i++)
		{
			ab = abm.LoadFromFile (fileNames[i]);
			ta = ab.LoadAsset<TextAsset>(fileNames[i].Substring(0,fileNames[i].IndexOf(".")));
			mgr.DoString(ta.text);
		}
	}
#else
	void LoadLua()
	{
		string pathURL =
			#if UNITY_ANDROID
			"jar:file://" + Application.dataPath + "!/assets/";
		#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
		#else
		"file://" + Application.dataPath + "/StreamingAssets001/";
		#endif 

//		AssetBundle.CreateFromFile (Application.dataPath + localLuaAssetBundlePath + fileName);
		string path = "Assets/_Moba/Lua/";
		TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset> (path + "uluaentrance" + ".txt");
		Debug.Log (ta);
//		TextAsset ta = ab.LoadAsset<TextAsset>("uluaentrance");
//		
		LuaScriptMgr mgr = new LuaScriptMgr ();
		mgr.Start ();
		mgr.DoString (ta.text);
		LuaTable luaTable = mgr.GetLuaTable ("luaFileList");
		string[] fileNames = luaTable.ToArray<string>();
		for(int i=0;i<fileNames.Length;i++)
		{

			ta = AssetDatabase.LoadAssetAtPath<TextAsset> (path + fileNames[i].Replace(".lua","") + ".txt");
//			ab = abm.LoadFromFile (fileNames[i]);
//			ta = ab.LoadAsset<TextAsset>(fileNames[i].Substring(0,fileNames[i].IndexOf(".")));
			mgr.DoString(ta.text);
			Debug.Log (ta);
		}
	}
#endif

	void InitBuildings()
	{

	}
}
