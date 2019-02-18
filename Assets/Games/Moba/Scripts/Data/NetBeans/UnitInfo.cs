using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitInfo  {

	public string unitName;//单位名称
	public string prefabPath;//存放路径
	public int buildDuration;//建造时间
	public int attackType;//攻击类型
	public float minAttackRange;//最小攻击距离
	public bool isMelee;//是否近战
	public float baseAttackInterval;//攻击间隔
	public float baseAttackRange;//攻击距离
	public float baseHealth = -1;//基础生命
	public float baseMana = 60;//基础法力
	public float baseDamage = -1;//基础伤害
	public float baseArmor = -1;//基础护甲
	public int minDamage = -1;//最小伤害
	public int maxDamage = -1;//最大伤害
	public int armorType;//护甲类型
	public string skills;//技能
	public string skillInfo;//技能说明
	public int killPrice ;//击杀奖励
	public int healthRecover = 0;//生命恢复
	public float manaRecover = 0;//法力恢复


}
