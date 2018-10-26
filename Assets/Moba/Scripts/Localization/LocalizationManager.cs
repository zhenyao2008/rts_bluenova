using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.Localzation
{
    public delegate string LoadLocalization(LocalizationType localizationType);

    public enum LocalizationType
    {
        jp, cn, en
    }

    [ExecuteInEditMode]
    public class LocalizationManager : SingleMonoBehaviour<LocalizationManager>
    {

        Dictionary<string, string> mKeyValuePairs;

        LocalizationType localzationType;

        public bool load;

        public event LoadLocalization onLoad;

        protected override void Awake()
        {
            base.Awake();
            onLoad += ResourcesManager.Instance.LoadLocalization;
            ConfigUtility.Init();
        }

        void LoadKeyValue(LocalizationType localzationType)
        {
            if (onLoad != null)
            {
                mKeyValuePairs = new Dictionary<string, string>();
                string localizationData = onLoad(localzationType);
                KeyValuePairs keyValuePairs = JsonUtility.FromJson<KeyValuePairs>(localizationData);
                for (int i = 0; i < keyValuePairs.keyValuePairs.Length; i++)
                {
                    KeyValuePair keyValuePair = keyValuePairs.keyValuePairs[i];
                    if (!mKeyValuePairs.ContainsKey(keyValuePair.key))
                        mKeyValuePairs.Add(keyValuePair.key, keyValuePair.value);
                }
                Debug.Log("mKeyValuePairs:" + mKeyValuePairs.Count);
            }
        }

        public string Localization(string key)
        {
            return mKeyValuePairs[key];
        }

        void Update()
        {
            if (load)
            {
                CoverAttributeToJson();
                load = false;
            }
        }

        public void CoverAttributeToJson()
        {
#if UNITY_EDITOR
            List<UnitAttributeEntity> unitAttributeList = new List<UnitAttributeEntity>();
            for (int i = 0; i < UnityEditor.Selection.gameObjects.Length; i++)
            {
                UnitAttribute unitAttribute = UnityEditor.Selection.gameObjects[i].GetComponent<UnitAttribute>();

                UnitAttributeEntity unitAttributeEntity = UnitAttributeEntity.CoverTo(unitAttribute);
                if (unitAttributeEntity != null)
                    unitAttributeList.Add(unitAttributeEntity);

            }
            UnitAttributeGroup unitAttributeGroup = new UnitAttributeGroup();
            unitAttributeGroup.unitAttributes = unitAttributeList.ToArray();
            Debug.Log(JsonUtility.ToJson(unitAttributeGroup, true));
#endif
        }
    }


    [System.Serializable]
    public class KeyValuePairs
    {
        public KeyValuePair[] keyValuePairs;
    }

    [System.Serializable]
    public class KeyValuePair
    {
        public string key;
        public string value;
    }

}

