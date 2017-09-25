using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UnitProperties : MonoBehaviour
{

	public bool load;
	public UnitAttribute loadTarget;
	//单位名称
	public string unitName;
	//存放路径
	public string prefabPath;
	//建造时间
	public int buildDuration;
	//最小伤害
	public int minDamage = -1;
	//最大伤害
	public int maxDamage = -1;
	//攻击类型
	public AttackType attackType = AttackType.Normal;
	//攻击间隔
	public float attackInterval;
	//攻击距离
	public float attackRange;
	//最小攻击距离
	public float minAttackRange;
	//是否近战
	public bool isMelee;
	//基础生命
	public int baseHealth = -1;

	public int armor = -1;
	public ArmorType armorType = ArmorType.Heavy;
	public string skillInfo;
	public int killPrice = -1;
	public int healthRecover = 0;
	public float mana = 60;
	public float manaRecover = 0;
	public int baseDamage = -1;
	public int currentHealth = -1;
	public int maxHealth = -1;
	public int exp = -1;
	public int levelUpExp = -1;
	public int level = -1;

	void Update ()
	{
		if (load) {
			UnitAttribute ua = GetComponent<UnitAttribute> ();
			if (loadTarget)
				ua = loadTarget;
			unitName = ua.unitName;
			buildDuration = ua.buildDuration;//建造时间
			minDamage = ua.minDamage;
			maxDamage = ua.maxDamage;
			attackType = ua.attackType;
			attackInterval = ua.attackInterval;//攻击间隔
			attackRange = ua.attackRange;//攻击距离
			isMelee = ua.isMelee;
			baseHealth = ua.baseHealth;
			armor = ua.armor;
			armorType = ua.armorType;
			skillInfo = ua.skillInfo;
			killPrice = ua.killPrice;
			healthRecover = ua.healthRecover;
			mana = ua.mana;
			manaRecover = ua.manaRecover;
			baseDamage = ua.baseDamage;
			currentHealth = ua.currentHealth;
			maxHealth = ua.maxHealth;
			exp = ua.exp;
			levelUpExp = ua.levelUpExp;
			level = ua.level;
			load = false;
			loadTarget = null;
		}
	}
}
