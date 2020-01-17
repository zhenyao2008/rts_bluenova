using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class SceneEditorWindow : EditorWindow {

	static EditorWindow ew;
	static GUIStyle boldStyle;
	[MenuItem("LevelEditor/美术工具")]
	static void Init()
	{
		ew = EditorWindow.GetWindow<SceneEditorWindow> ();
		ew.minSize = new Vector2 (520,700);
		boldStyle = new GUIStyle ();
		boldStyle.fontStyle = FontStyle.Bold;
	}

	public List<GameObject> animObjects;
	public List<TextAsset> animTexts;

	bool foldOut = true;
	bool foldOut1 = true;

	bool conn_foldout = false;
	bool conn_foldout1 = false;
	bool conn_foldout2 = false;
	bool conn_foldout3 = false;
	bool conn_foldout4 = false;
	void OnGUI()
	{
//		HTGUILayout.DrawTitleChapter ("aaa", 30, true, 1, ew.);
		HTGUILayout.FoldOut (ref foldOut, "创建 or 删除场景对象", false);
		if(foldOut)
		{
//			EditorGUILayout.BeginHorizontal ();
//			GUILayout.Label ("Part 01: 创建删除场景对象");
//			EditorGUILayout.EndHorizontal ();
//			
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("创建SceneRoot")) {
				CreateBaseSceneObjects();
			}
			if (GUILayout.Button ("删除隐藏对象")) {
				RemoveHideGameObject();
			}
			EditorGUILayout.EndHorizontal ();
		}

		HTGUILayout.FoldOut (ref foldOut1, "FBX相关", false);
		if(foldOut1)
		{
//			EditorGUILayout.BeginHorizontal ();
//			GUILayout.Label ("Part 02: FBX导入相关");
//			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("1.   关闭选中FBX文件的法线");
			if (GUILayout.Button ("关闭",GUILayout.Width(100))) {
				CloseNormal();
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("2.   创建选中FBX文件的预制体");
			if (GUILayout.Button ("创建",GUILayout.Width(100))) {
				CreatePrefabs();
			}
			EditorGUILayout.EndHorizontal ();


			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("3.   动画分帧");
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			conn_foldout1 = EditorGUILayout.Foldout( conn_foldout1, "   说明" );
			if (GUILayout.Button ("分帧",GUILayout.Width(100))) {
				AnimationClipImporter.Import();
			}
			EditorGUILayout.EndHorizontal ();
			if ( conn_foldout1 )
			{
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("          对选中FBX文件进行动画分帧。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("          导入条件是同一目录下面有相同名字的.fbx文件和.txt文件。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("     例如： Assets/Units/C1001文件夹里面同时存在C1001.fbx和C1001.txt文件。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("          C1001.txt文件格式： 开始帧-结束帧 动作名称。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("     例如： 0-30 Idle");
				EditorGUILayout.EndHorizontal ();
			}


			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("4.   导入/导出动画事件");
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			conn_foldout2 = EditorGUILayout.Foldout( conn_foldout2, "   说明" );
			if (GUILayout.Button ("导出",GUILayout.Width(100))) {
				AnimationEventConvert.ExportAnimationEvents();
			}
			if (GUILayout.Button ("导入",GUILayout.Width(100))) {
				AnimationEventConvert.ImportAnimationEvents();
			}
			EditorGUILayout.EndHorizontal ();
			if ( conn_foldout2 )
			{
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("          导入选中FBX的动画事件。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("          导入条件是同一目录下面有相同名字的.fbx文件和.asset文件。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("     例如： Assets/Units/C1001文件夹里面同时存在C1001.fbx和C1001.asset文件。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("          导出FBX动画事件。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("     C1001.asset 是由导出时自动生成的。");
				EditorGUILayout.EndHorizontal ();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			GUILayout.Label ("5.   其他设置");
			conn_foldout = EditorGUILayout.Foldout( conn_foldout, "   说明" );
			if ( conn_foldout )
			{
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("",GUILayout.Width(20));
				GUILayout.Label ("导入设定。凡是添加到或者Reimport的在Assets/Level/文件夹里的.FBX会应用此设置。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("",GUILayout.Width(20));
				GUILayout.Label ("如果不想应用设置则放在Assets/Level/*/fbx_1/文件夹里，.FBX就不会应用此设置。");
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("",GUILayout.Width(20));
				GUILayout.Label ("自动设置内容：1.关闭动画组件；2.开启LightMap UV(UV2)。");
				EditorGUILayout.EndHorizontal ();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.Label ("6.   LightMapUV");

			EditorGUILayout.BeginHorizontal ();
			conn_foldout3 = EditorGUILayout.Foldout( conn_foldout3, "   说明" );
			if (GUILayout.Button ("导入",GUILayout.Width(100))) {
				CopyLightMapInfoWindow.ImportLightMapUVs();
			}
			if (GUILayout.Button ("导出",GUILayout.Width(100))) {
				CopyLightMapInfoWindow.ExportLightMapUVs();
			}
			EditorGUILayout.EndHorizontal ();
			if ( conn_foldout3 )
			{
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("",GUILayout.Width(20));
				GUILayout.Label ("此处省略19600个字。");
				EditorGUILayout.EndHorizontal ();
			}


			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.Label ("7.   系统配置");

			EditorGUILayout.BeginHorizontal ();
			conn_foldout4 = EditorGUILayout.Foldout( conn_foldout4, "   说明" );
//			if (GUILayout.Button ("导入",GUILayout.Width(100))) {
////				CopyLightMapInfoWindow.ImportLightMapUVs();
//
//			}

			if (GUILayout.Button ("导出",GUILayout.Width(100))) {
//				CopyLightMapInfoWindow.ExportLightMapUVs();
//				string path = EditorUtility.OpenFilePanel("导出","Assets/_Moba/Resources/Configs/","txt");
				systemConfigPath = EditorUtility.OpenFilePanel("导出","Assets/_Moba/Resources/Configs/","txt");
				ExportSystemConfig();

			}
			EditorGUILayout.EndHorizontal ();
			if (conn_foldout4)
			{
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("",GUILayout.Width(20));
				GUILayout.Label ("此处省略19600个字。");
				EditorGUILayout.EndHorizontal ();
			}

		}
	}

//	void ImportSystemConfig(){
//		TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset> (systemConfigPath);
//		SystemConfig sc = JsonConvert.DeserializeObject<SystemConfig> (ta.text);
//	}

	string systemConfigPath = "Assets/_Moba/Resources/Configs/NewSysConfig/SystemConfig.txt";
	void ExportSystemConfig(){
		Debug.Log ("ExportSystemConfig");
		TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset> (systemConfigPath);
		SystemConfig sc = null;
		if (ta == null) {
			sc = new SystemConfig ();
		} else {
			sc = JsonConvert.DeserializeObject<SystemConfig> (ta.text);
		}
		string json = JsonConvert.SerializeObject (sc,Formatting.Indented);
		File.WriteAllText (Application.dataPath + "/_Moba/Resources/Configs/NewSysConfig/SystemConfig.txt", json);
		AssetDatabase.Refresh ();
	}

	//创建基础场景对象
	void CreateBaseSceneObjects()
	{
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Prefabs/SceneRoot.prefab");
		Object obj = Instantiate (prefab,Vector3.zero,Quaternion.identity);
		obj.name = prefab.name;
	}

	public static List<Object> selectObjs;
	public static string selectPath;
	void CloseNormal()
	{
		selectObjs = new List<Object> ();
		Object[] objs = Selection.objects;
		for(int i =0;i<objs.Length;i++)
		{
			selectObjs.Add(objs[i]);
			selectPath = AssetDatabase.GetAssetPath (objs[i]);
			AssetDatabase.ImportAsset (selectPath);
		}
	}

	void CreatePrefabs(){
		Object[] objs = Selection.objects;
		for(int i=0;i<objs.Length;i++)
		{
			string path = AssetDatabase.GetAssetPath(objs[i]);
			GameObject go = Instantiate(objs[i]) as GameObject;
			go.transform.position = Vector3.zero;
			go.transform.rotation = Quaternion.identity;
			string prefabName = objs[i].name;
			string targetPath = path.Remove(path.LastIndexOf("/"));
			targetPath = targetPath.Remove(targetPath.LastIndexOf("/"));
			string folderpath = targetPath;
			targetPath += "/Prefab/" + prefabName + ".prefab";
			if(!AssetDatabase.IsValidFolder(folderpath + "/Prefab"))
			{
				AssetDatabase.CreateFolder(folderpath,"Prefab");
			}
			if(AssetDatabase.LoadAssetAtPath<GameObject>(targetPath)!=null)
			{
				Debug.LogWarning(prefabName + ".prefab exist." );
			}
			else
			{
				PrefabUtility.CreatePrefab(targetPath,go);
				Debug.Log(prefabName + ".prefab Create." );
			}
		}
	}

	void RemoveHideGameObject()
	{
		List<GameObject> sceneObjs = GetAllObjectsInScene (false);
		foreach(GameObject go in sceneObjs)
		{
			if(go!=null && !go.activeInHierarchy)
			{
				Transform trans = go.transform;
				string path = "/" + go.gameObject.name;
				while(trans.parent!=null)
				{
					path = "/"  + trans.parent.name + path;
					trans = trans.parent;
				}
				Debug.Log("删除隐藏对象:"  + path);
			}
		}
		foreach(GameObject go in sceneObjs)
		{
			if(go!=null && !go.activeInHierarchy)
			{
				GameObject.DestroyImmediate(go);
			}
		}
	}




	//获取所有场景游戏对象（包括InActive）
	public static List<GameObject> GetAllObjectsInScene(bool bOnlyRoot)
	{
		GameObject[] pAllObjects = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
		List<GameObject> pReturn = new List<GameObject>();
		foreach (GameObject pObject in pAllObjects)
		{
			if (bOnlyRoot)
			{
				if (pObject.transform.parent != null)
				{
					continue;
				}
			}
			if (pObject.hideFlags == HideFlags.NotEditable || pObject.hideFlags == HideFlags.HideAndDontSave)
			{
				continue;
			}
			if (Application.isEditor)
			{
				string sAssetPath = AssetDatabase.GetAssetPath(pObject.transform.root.gameObject);
				if (!string.IsNullOrEmpty(sAssetPath))
				{
					continue;
				}
			}
			pReturn.Add(pObject);
		}
		return pReturn;
	}



}






