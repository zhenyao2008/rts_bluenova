using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

namespace Framework {

    public class ResourceManager : MonoBehaviour {

        static ResourceManager instance;
        public static ResourceManager Instance()
        {
            if (instance == null )
            {
                GameObject go = new GameObject("_ResourceManager");
                instance = go.AddComponent<ResourceManager>();
            }
            return instance;
        }

        void Awake()
        {
            instance = this;
        }   

        public const string localPanelPrefabPath = "Assets/_BoomBeach/Prefabs/UI/CSharp/";

        public GameObject LoadLocalPanelPrefab(string panelName)
        {
#if UNITY_EDITOR
            string path = localPanelPrefabPath + panelName + ".prefab";
            Debug.Log(path);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab;
#else
            return null;
#endif
        }

        //DOTO
        public GameObject LoadLocalAssetbundlePanel(string panelName)
        {
            return null;
        }
	   
    }

}
