using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigUtility {

	public static BattleConfig systemConfig;

	const string SYSTEM_CONFIG_PATH = "configs/SystemConfig";

    public static UnitAttributeGroup unitAttributeGroup;

	public static void Init(){
		//systemConfig = JsonUtility.FromJson<BattleConfig> (SYSTEM_CONFIG_PATH);
        string data = Resources.Load<TextAsset>("Configs/GameConfig/UnitConfig").text;
        Debug.Log(data);
        unitAttributeGroup = JsonUtility.FromJson<UnitAttributeGroup>(data);
        Debug.Log(unitAttributeGroup.unitAttributes.Length);
	}
}

[System.Serializable]
public class BattleConfig{
	//今度コーンを増える時間帯
	public float moneyPlusInterval;

	public int moneyPlusPer; 

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