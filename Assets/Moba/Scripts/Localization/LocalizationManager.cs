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
    public class UnitAttributeGroup
    {
        public UnitAttributeEntity[] unitAttributes;
    }

    [System.Serializable]
    public class UnitAttributeEntity
    {
        public int instanceId;
        public int buildCorn;
        public string unitName;
        public int buildDuration;
        public int minDamage;
        public int maxDamage;
        public int attackType;
        public float attackInterval;
        public float attackRange;
        public bool isMelee;
        public int baseHealth;
        public int armor;
        public int armorType;
        public string skillInfo;
        public int killPrice;
        public int healthRecover;
        public float mana;
        public float manaRecover;
        public int maxHealth;
        public int killExp;
        public int levelUpExp;
        public int corn;

        public static UnitAttributeEntity CoverTo(UnitAttribute unitAttribute)
        {
            UnitAttributeEntity unitAttributeEntity = new UnitAttributeEntity();
            unitAttributeEntity.instanceId = unitAttribute.GetInstanceID();
            unitAttributeEntity.buildCorn = unitAttribute.buildCorn;
            unitAttributeEntity.unitName = unitAttribute.unitName;
            unitAttributeEntity.buildDuration = unitAttribute.buildDuration;
            unitAttributeEntity.minDamage = unitAttribute.minDamage;
            unitAttributeEntity.maxDamage = unitAttribute.maxDamage;
            unitAttributeEntity.attackType = (int)unitAttribute.attackType;
            unitAttributeEntity.attackInterval = unitAttribute.attackInterval;
            unitAttributeEntity.attackRange = unitAttribute.attackRange;
            unitAttributeEntity.isMelee = unitAttribute.isMelee;
            unitAttributeEntity.baseHealth = unitAttribute.baseHealth;
            unitAttributeEntity.armor = unitAttribute.armor;
            unitAttributeEntity.armorType = (int)unitAttribute.armorType;
            unitAttributeEntity.skillInfo = unitAttribute.skillInfo;
            unitAttributeEntity.killPrice = unitAttribute.killPrice;
            unitAttributeEntity.healthRecover = unitAttribute.healthRecover;
            unitAttributeEntity.mana = unitAttribute.mana;
            unitAttributeEntity.manaRecover = unitAttribute.manaRecover;
            unitAttributeEntity.maxHealth = unitAttribute.maxHealth;
            unitAttributeEntity.killExp = unitAttribute.killExp;
            unitAttributeEntity.levelUpExp = unitAttribute.levelUpExp;
            unitAttributeEntity.corn = unitAttribute.corn;
            return unitAttributeEntity;
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

