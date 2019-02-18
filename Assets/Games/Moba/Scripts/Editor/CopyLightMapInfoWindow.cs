using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngineInternal;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class CopyLightMapInfoWindow : EditorWindow {

	[MenuItem("LevelEditor/CopyLightMapInfo")]
	static void Init()
	{
		CopyLightMapInfoWindow ci = EditorWindow.GetWindow<CopyLightMapInfoWindow>();
		ci.minSize = new Vector2 (300,120);
	}

	Object fromGo;
	Object toGo;
	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("From:",GUILayout.Width(80));
		fromGo = EditorGUILayout.ObjectField (fromGo,typeof(GameObject),true,GUILayout.MinWidth(120));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("To:",GUILayout.Width(80));
		toGo =  EditorGUILayout.ObjectField (toGo,typeof(GameObject),true,GUILayout.MinWidth(120));
		EditorGUILayout.EndHorizontal ();
		if(GUILayout.Button("复制"))
		{
			CopyLightMapInfo();
		}
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("从烘焙对象复制光照信息。只能是场景中的游戏对象。");
		EditorGUILayout.EndHorizontal ();
	}

	void CopyLightMapInfo()
	{
		GameObject fromGo01 = (GameObject)fromGo;
		GameObject toGo01 = (GameObject)toGo;
		Renderer[] renderes = fromGo01.GetComponentsInChildren<Renderer> (true);
		foreach(Renderer rd in renderes)
		{
			string path = GetTransPath(fromGo01.transform,rd.transform);
			Transform trans = toGo01.transform.Find(path);
			CloneLightMapInfo(rd,trans.GetComponent<Renderer>());
		}
	}
	
	void CloneLightMapInfo(Renderer from,Renderer to)
	{
		to.lightmapIndex = from.lightmapIndex;
		to.lightmapScaleOffset = from.lightmapScaleOffset;
	}


	public static string importLightMapDataPath = "Assets/LevelSettings/LightMap.txt";
	public static string importLightMapImagePath = "Assets/Level/Levels/1_DDMY_11/Lightmap-0_comp_light.exr";

	public static void ImportLightMapUVs(){
		MeshRenderer[] rds = GameObject.FindObjectsOfType<MeshRenderer>();
		TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset> (importLightMapDataPath);
		Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D> (importLightMapImagePath);
		Debug.Log (tex);
		LightmapData[] ld = new LightmapData[1];
		ld [0] = new LightmapData ();
		ld [0].lightmapColor = tex;
		LightmapSettings.lightmaps = ld;

		List<LightParamBean> lps = JsonConvert.DeserializeObject<List<LightParamBean>> (ta.text);
		Dictionary<int,LightParamBean> lpds = new Dictionary<int, LightParamBean> ();
		foreach(LightParamBean lp in lps)
		{
			lpds.Add(lp.instanceId,lp);
		}

		foreach(MeshRenderer rd in rds)
		{
			if(lpds.ContainsKey(rd.GetInstanceID()))
			{
				rd.lightmapIndex = lpds[rd.GetInstanceID()].lightmapIndex;
				float[] lsf =  lpds[rd.GetInstanceID()].lightmapScaleOffset;
				Vector4 lightmapScaleOffset = new Vector4(lsf[0],lsf[1],lsf[2],lsf[3]);
				rd.lightmapScaleOffset = lightmapScaleOffset;
			}
		}
	}

	public static void ExportLightMapUVs(){
		MeshRenderer[] rds = GameObject.FindObjectsOfType<MeshRenderer>();
		List<LightParamBean> lps = new List<LightParamBean> ();
		foreach(MeshRenderer rd in rds)
		{
			LightParamBean lpb = new LightParamBean();
			float[] lightmapScaleOffset = new float[4];
			lightmapScaleOffset[0] = rd.lightmapScaleOffset.x;
			lightmapScaleOffset[1] = rd.lightmapScaleOffset.y;
			lightmapScaleOffset[2] = rd.lightmapScaleOffset.z;
			lightmapScaleOffset[3] = rd.lightmapScaleOffset.w;
			lpb.lightmapIndex = rd.lightmapIndex;
			lpb.instanceId = rd.GetInstanceID();
			lpb.lightmapScaleOffset = lightmapScaleOffset;
			lps.Add(lpb);
		}
		string json = JsonConvert.SerializeObject (lps);
		File.WriteAllText (Application.dataPath + "/LevelSettings/" + "LightMap.txt", json);
		Debug.Log (json);
	}

	string GetTransPath(Transform root,Transform trans)
	{
		string path = trans.name;
		while(trans.parent!=null && trans.parent!= root)
		{
			trans = trans.parent;
			path = trans.name + "/" + path;
		}
		Debug.Log (path);
		return path;
	}

}
