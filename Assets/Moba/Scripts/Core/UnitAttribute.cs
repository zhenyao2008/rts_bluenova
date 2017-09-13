using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public enum AttackType{
	Normal,
	Puncture,
	Magic,
	Siege,
	Chaos
};
public enum ArmorType{
	None,
	Middle,
	Light,
	Heavy,
	Construction
};
public class UnitAttribute : NetworkBehaviour  {

	public int buildCorn;//建造费用
	public string unitName;
	public int buildDuration;//建造时间
	public int minDamage = -1;
	public int maxDamage = -1;
	public AttackType attackType = AttackType.Normal;
	public float attackInterval;//攻击间隔
	public float attackRange;//攻击距离
	public bool isMelee;
	public int baseHealth = -1;
	public int armor = -1;
	public ArmorType armorType = ArmorType.Heavy;
	public string skillInfo;
	public int killPrice = -1;

	public int healthRecover = 0;
	public float mana = 60;
	public float manaRecover = 0;
//	[SyncVar]






	public int baseDamage = -1;//不再使用的变量
	[SyncVar]
	public int currentHealth = -1;
	[SyncVar]
	public int maxHealth = -1;
//	[SyncVar]
	public int currentDamage = -1;//不再使用的变量

	public int killExp = -1;
	[SyncVar]
	public int levelUpExp = -1;
	public int corn = -1;
	[SyncVar]
	public int exp = -1;
	[SyncVar]
	public int level = -1;
	DamageFactor mDamageFactor;


	UnitBase mUnitBase;

	void Awake(){
		mDamageFactor = GetDamageFactor (attackType);
		maxHealth = baseHealth;
		currentHealth = baseHealth;
		mUnitBase = GetComponent<UnitBase> ();
	}

	public static Dictionary<AttackType,DamageFactor> damageFactors;

	public void ReCalculateAttribute()
	{
		currentDamage = (int)(baseDamage * (1 + (float)(level-1) / 10));
		maxHealth = (int)(baseHealth * (1 + (float)(level-1) / 10));
		currentHealth = maxHealth;
	}

	public void OnDamage(UnitAttribute other,int damage){
//		Debug.Log (damage);
		this.currentHealth -= damage > 0 ? damage : 1;
	}
	
	int GetBaseDamage(){
		return Random.Range (minDamage,maxDamage+1);
	}

	//击中目标时真实的伤害计算
	public int GetHitDamage(UnitBase target){
		UnitAttribute targetAttribute = target.unitAttribute;
		int damage = GetBaseDamage ();

		List<DamageIncrease> dis = mUnitBase.damageIncreases;
		if(dis!=null && dis.Count>0)
		{
			int preDamage = damage;
			for(int i=0;i<dis.Count;i++)
			{
				damage = dis[i].GetDamage(damage);
			}
			if(preDamage < damage)
			{
				mUnitBase.ShowMsgTips(1,"! " + Mathf.Max (damage-preDamage,1) , Color.red,1.5f,new Vector3(0,10,0));
			}
		}
		
		List<DamageReduction> drs = target.damageReductions;
		if(drs!=null && drs.Count>0)
		{
			int preDamage = damage;
			for(int i=0;i<drs.Count;i++)
			{
				damage = drs[i].GetDamage(damage);
			}
			if(preDamage > damage)
			{
				target.ShowMsgTips(1,Mathf.Max ((preDamage-damage),1).ToString() , Color.yellow,1,Vector3.zero);
			}
		}

		damage = (int)(damage * mDamageFactor.damageFactor[targetAttribute.armorType]);
		return damage;
	}

	public DamageFactor GetDamageFactor(AttackType atkType)
	{
		if (damageFactors == null) {
			damageFactors = new Dictionary<AttackType, DamageFactor>();
			DamageFactor df = new DamageFactor();
			df.attackType = AttackType.Normal;
			df.damageFactor = new Dictionary<ArmorType, float>();
			df.damageFactor.Add(ArmorType.None,1);
			df.damageFactor.Add(ArmorType.Light,1);
			df.damageFactor.Add(ArmorType.Heavy,1);
			df.damageFactor.Add(ArmorType.Construction,0.5f);
			df.damageFactor.Add(ArmorType.Middle,1.5f);
			damageFactors.Add(AttackType.Normal,df);

			df = new DamageFactor();
			df.attackType = AttackType.Puncture;
			df.damageFactor = new Dictionary<ArmorType, float>();
			df.damageFactor.Add(ArmorType.None,1.5f);
			df.damageFactor.Add(ArmorType.Light,2);
			df.damageFactor.Add(ArmorType.Heavy,1);
			df.damageFactor.Add(ArmorType.Construction,0.35f);
			df.damageFactor.Add(ArmorType.Middle,0.75f);
			damageFactors.Add(AttackType.Puncture,df);

			df = new DamageFactor();
			df.attackType = AttackType.Magic;
			df.damageFactor = new Dictionary<ArmorType, float>();
			df.damageFactor.Add(ArmorType.None,1);
			df.damageFactor.Add(ArmorType.Light,1);
			df.damageFactor.Add(ArmorType.Heavy,2);
			df.damageFactor.Add(ArmorType.Construction,0.35f);
			df.damageFactor.Add(ArmorType.Middle,0.75f);
			damageFactors.Add(AttackType.Magic,df);

			df = new DamageFactor();
			df.attackType = AttackType.Chaos;
			df.damageFactor = new Dictionary<ArmorType, float>();
			df.damageFactor.Add(ArmorType.None,1);
			df.damageFactor.Add(ArmorType.Light,1);
			df.damageFactor.Add(ArmorType.Heavy,1);
			df.damageFactor.Add(ArmorType.Construction,1);
			df.damageFactor.Add(ArmorType.Middle,1);
			damageFactors.Add(AttackType.Chaos,df);

			df = new DamageFactor();
			df.attackType = AttackType.Siege;
			df.damageFactor = new Dictionary<ArmorType, float>();
			df.damageFactor.Add(ArmorType.None,1.5f);
			df.damageFactor.Add(ArmorType.Light,1);
			df.damageFactor.Add(ArmorType.Heavy,1);
			df.damageFactor.Add(ArmorType.Construction,1.5f);
			df.damageFactor.Add(ArmorType.Middle,0.5f);
			damageFactors.Add(AttackType.Siege,df);
		}
		return damageFactors[atkType];
	}
}

public class DamageFactor
{
	public AttackType attackType;

	public Dictionary<ArmorType,float> damageFactor;

}

