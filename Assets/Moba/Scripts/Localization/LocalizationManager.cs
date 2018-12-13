using System.Collections;
using System.Collections.Generic;
using System.Text;
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
                CoverAttributeToCSV();
                load = false;
            }
        }

        public void CoverAttributeToCSV()
        {
#if UNITY_EDITOR
            List<UnitAttributeEntity> unitAttributeList = new List<UnitAttributeEntity>();
            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            string[] prefabs = UnityEditor.AssetDatabase.FindAssets("t:GameObject", new string[1]{"Assets/Moba/Prefabs/Soldiers"});
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("id,resourceName,buildCorn,unitName,buildDuration,minDamage,maxDamage,attackType,attackInterval,attackRange,isMelee,baseHealth,armor,armorType,skillInfo,killPrice,healthRecover,mana," +
                                     "manaRecover,maxHealth,killExp,levelUpExp,corn");
            for (int i = 0; i < prefabs.Length; i++)
            {
                string prefabPath = UnityEditor.AssetDatabase.GUIDToAssetPath(prefabs[i]);
                Debug.Log(prefabPath);
                UnitAttribute unitAttribute = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath).GetComponent<UnitAttribute>();
                unitAttribute.unitId = 10000 + i;
                string unitAttributeString = UnitAttributeToString(unitAttribute,prefabPath.Replace("Assets/Moba/Prefabs/", "").Replace(".prefab",""));
                stringBuilder.AppendLine(unitAttributeString);
                UnitAttributeEntity unitAttributeEntity = UnitAttributeEntity.CoverTo(unitAttribute);
                if (unitAttributeEntity != null)
                    unitAttributeList.Add(unitAttributeEntity);
            }
            UnitAttributeGroup unitAttributeGroup = new UnitAttributeGroup();
            unitAttributeGroup.unitAttributes = unitAttributeList.ToArray();
            Debug.Log(stringBuilder.ToString());
#endif
        }

        string UnitAttributeToString(UnitAttribute unitAttribute,string path){
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(unitAttribute.unitId + ",");
            stringBuilder.Append(path + ",");
            stringBuilder.Append(unitAttribute.buildCorn + ",");
            stringBuilder.Append(unitAttribute.unitName + ",");
            stringBuilder.Append(unitAttribute.buildDuration + ",");
            stringBuilder.Append(unitAttribute.minDamage + ",");
            stringBuilder.Append(unitAttribute.maxDamage + ",");
            stringBuilder.Append(unitAttribute.attackType + ",");
            stringBuilder.Append(unitAttribute.attackInterval + ",");
            stringBuilder.Append(unitAttribute.attackRange + ",");
            stringBuilder.Append(unitAttribute.isMelee + ",");
            stringBuilder.Append(unitAttribute.baseHealth + ",");
            stringBuilder.Append(unitAttribute.armor + ",");
            stringBuilder.Append(unitAttribute.armorType + ",");
            stringBuilder.Append(unitAttribute.skillInfo + ",");
            stringBuilder.Append(unitAttribute.killPrice + ",");
            stringBuilder.Append(unitAttribute.healthRecover + ",");
            stringBuilder.Append(unitAttribute.mana + ",");
            stringBuilder.Append(unitAttribute.manaRecover + ",");
            stringBuilder.Append(unitAttribute.maxHealth + ",");
            stringBuilder.Append(unitAttribute.killExp + ",");
            stringBuilder.Append(unitAttribute.levelUpExp + ",");
            stringBuilder.Append(unitAttribute.corn);
            return stringBuilder.ToString();
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

