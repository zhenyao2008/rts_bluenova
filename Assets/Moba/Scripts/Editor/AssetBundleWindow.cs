using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetBundleWindow : EditorWindow {

	[MenuItem("Window/BuildAsset")]
	public static EditorWindow Init()
	{
		EditorWindow window = GetWindow(typeof(AssetBundleWindow));
		window.minSize	= new Vector2(280, 300);
		window.Show();
		return window;
	}
	string path = "Assets/StreamingAssets/uLua";
	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,100,30),"BuildLua"))
		{
//			AssetBundleBuild[] buildMap  = new AssetBundleBuild[1];
//			
//			buildMap[0].assetBundleName = "resources";//打包的资源包名称 随便命名
//			string[] resourcesAssets = new string[2];//此资源包下面有多少文件
//			resourcesAssets[0] = "resources/1.prefab";
//			resourcesAssets[1] = "resources/MainO.cs";
//			buildMap[0].assetNames = resourcesAssets;
			#if UNITY_IOS
			BuildPipeline.BuildAssetBundles(path,BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.iOS);
			#else
			BuildPipeline.BuildAssetBundles(path,BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.Android);
			#endif
//			AssetDatabase.
		}
	}
}
