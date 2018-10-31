using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ResourcesManager : SingleMonoBehaviour<ResourcesManager>
{
	Dictionary<string,Sprite> mBuildingIcons;

	protected override void Awake ()
	{
		base.Awake ();
	}

	//以下是示例代码
	#region Units

	public GameObject GetUnitObject (string resPath)
	{
		GameObject prefab = GetUnitPrefab (resPath);
		GameObject go = Instantiate (prefab) as GameObject;
		Common.SetShaderForEditor (go);
		return go;
	}

	public GameObject GetUnitPrefab (string soliderName)
	{
		return AssetbundleManager.Instance.GetAssetFromLocal<GameObject> (soliderName, soliderName);
	}

	#endregion

	#region Buildings

	public GameObject GetBuildingObejct (string buildingName)
	{
		GameObject prefab = AssetbundleManager.Instance.GetAssetFromLocal<GameObject> (buildingName, buildingName);
		GameObject go = Instantiate (prefab) as GameObject;
		Common.SetShaderForEditor (go);
		return go;
	}

	#endregion

	#region Audios

	public AudioClip GetAudioClipBGM (string bgm)
	{
		AudioClip clip = AssetbundleManager.Instance.GetAssetFromLocal<AudioClip> (ABConstant.SOUND_BGM, bgm);
		return clip;
	}

	public AudioClip GetAudioClipSE (string se)
	{
		AudioClip clip = AssetbundleManager.Instance.GetAssetFromLocal<AudioClip> (ABConstant.SOUND_SE, se);
		return clip;
	}

	#endregion

	#region UI

	public GameObject GetUIInterface (string prefabName)
	{
		GameObject go = AssetbundleManager.Instance.GetAssetFromLocal<GameObject> (ABConstant.PREFAB_INTERFACE, prefabName);
		return go;
	}

	#endregion

	#region Battles

	public GameObject GetBattleRoot ()
	{
		GameObject prefab = AssetbundleManager.Instance.GetAssetFromLocal<GameObject> (ABConstant.BATTLE, "BattleRoot");
		GameObject go = Instantiate (prefab) as GameObject;
		return go;
	}

	#endregion

	//TODO
	public GameObject LoadUIPrefab(string uiName){
		GameObject prefab = Resources.Load<GameObject> ("UI/" + uiName);
		return prefab;
	}

    const string LOCALZATION_PATH_BASE = "localization/localization_";

    public string LoadLocalization(BlueNoah.Localzation.LocalizationType localizationType){
        return  Resources.Load<TextAsset>(LOCALZATION_PATH_BASE + localizationType.ToString()).text;
    }

	public Sprite GetSprite (string path)
	{
		return null;
	}

	public Sprite GetCharacterIconById (int charaId)
	{
		return null;
	}

	public Sprite GetBuildingFullIconById(int buildingId){
		if (mBuildingIcons == null) {
			mBuildingIcons = new Dictionary<string, Sprite> ();
			AssetBundle ab = AssetBundle.LoadFromFile (Application.streamingAssetsPath + "/" + ABConstant.UI_BUILDING_ICON + ABConstant.ASSETBUNDLE);
			Sprite[] icons = ab.LoadAllAssets<Sprite> ();
            ab.Unload(false);
			for(int i=0;i<icons.Length;i++){
				mBuildingIcons.Add (icons[i].name,icons[i]);
			}
		}
		if(mBuildingIcons.ContainsKey(buildingId.ToString()))
			return mBuildingIcons[buildingId.ToString()];
		return null;
	}

	public GameObject GetCharacterPrefab (int charaId, int sortLayer = 1)
	{
		return null;
	}

	public byte[] GetCSV (string csvName)
	{
		return null;
	}

    //public static T LoadFromAssetBundleOrEditor<T>(string path, string fileName) where T : Object
    //{
    //    T t = null;
    //    #if UNITY_EDITOR
    //          if (fileName.IndexOf(".", System.StringComparison.CurrentCulture) == -1)
    //          {
    //              if (typeof(T) == typeof(Sprite))
    //              {
    //                  fileName += ".png";
    //              }
    //              else if (typeof(T) == typeof(GameObject))
    //              {
    //                  fileName += ".prefab";
    //              }
    //              else if (typeof(T) == typeof(AudioClip))
    //              {
    //                  //TODO
    //                  fileName += ".wav";
    //              }
    //              else if (typeof(T) == typeof(Material))
    //              {
    //                  fileName += ".mat";
    //              }
    //              else
    //              {
    //                  fileName += ".asset";
    //              }
    //          }
    //          path = "Assets" + AssetBundleConstant.ASSETBUNDLE_RESOURCES_PATH + path + "/" + fileName;
    //          t = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
    //          Debug.Log(path + "||" + t);
    //    #else
    //    if (fileName.IndexOf(".", System.StringComparison.CurrentCulture) != -1)
    //    {
    //        fileName = fileName.Substring(0, fileName.IndexOf(".", System.StringComparison.CurrentCulture));
    //    }
    //    t = AssetBundleLoadManager.Instance.LoadAssetFromLocalAssetBundle<T>(path + ".ab", fileName);
    //    //#endif
    //    return t;
    //}



}
