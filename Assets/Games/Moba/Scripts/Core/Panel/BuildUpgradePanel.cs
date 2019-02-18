using UnityEngine;
using System.Collections;

public class BuildUpgradePanel : MonoBehaviour {

	public GameObject root;
	public UILabel soilderName;
	public UILabel buildCorn;
	public UILabel buildTime;
	public UILabel health;
	public UILabel damage;
	
	public UILabel attackType;
	public UILabel attackSpeed;
	public UILabel attackRange;
	public UILabel armor;
	public UILabel armorType;
	public UILabel corn;//突破奖励
	public UILabel skillInfo;

	public UILabel soilderName1;
	public UILabel buildCorn1;
	public UILabel buildTime1;
	public UILabel health1;
	public UILabel damage1;
	
	public UILabel attackType1;
	public UILabel attackSpeed1;
	public UILabel attackRange1;
	public UILabel armor1;
	public UILabel armorType1;
	public UILabel corn1;//突破奖励
	public UILabel skillInfo1;


	public UIButton closeBtn;
	public UIButton upgradeBtn;

	void Start(){
		closeBtn.onClick.Add (new EventDelegate(Close));
	}
	
	void Close(){
		root.SetActive (false);
	}

	public void SetBuildInfo(UnitAttribute ua)
	{
		this.soilderName.text = ua.unitName;
		this.buildCorn.text = ua.buildCorn.ToString();
		this.buildTime.text = ua.buildDuration.ToString () + "s";
		this.health.text = ua.baseHealth.ToString();
		this.damage.text = ua.minDamage + "-" + ua.maxDamage;
		switch(ua.attackType)
		{
		case AttackType.Normal:
			this.attackType.text = "普通";break;
		case AttackType.Puncture:
			this.attackType.text = "穿刺";break;
		case AttackType.Magic:
			this.attackType.text = "魔法";break;
		case AttackType.Siege:
			this.attackType.text = "攻城";break;
		case AttackType.Chaos:
			this.attackType.text = "混乱";break;
		}
		this.attackSpeed.text = ua.attackInterval + "s/次";
		this.attackRange.text = ua.attackRange + "/" + (ua.isMelee ? "近战" : "远程");
		this.armor.text = ua.armor.ToString();
		switch(ua.armorType)
		{
		case ArmorType.None:
			this.armorType.text = "无甲";break;
		case ArmorType.Light:
			this.armorType.text = "轻甲";break;
		case ArmorType.Middle:
			this.armorType.text = "中甲";break;
		case ArmorType.Heavy:
			this.armorType.text = "重甲";break;
		case ArmorType.Construction:
			this.armorType.text = "建筑";break;
		}
		this.corn.text = ua.killPrice.ToString();
		this.skillInfo.text = ua.skillInfo;
	}

	public void SetBuildInfo1(UnitAttribute ua)
	{
		this.soilderName1.text = ua.unitName;
		this.buildCorn1.text = ua.buildCorn.ToString();
		this.buildTime1.text = ua.buildDuration.ToString () + "s";
		this.health1.text = ua.baseHealth.ToString();
		this.damage1.text = ua.minDamage + "-" + ua.maxDamage;
		switch(ua.attackType)
		{
		case AttackType.Normal:
			this.attackType1.text = "普通";break;
		case AttackType.Puncture:
			this.attackType1.text = "穿刺";break;
		case AttackType.Magic:
			this.attackType1.text = "魔法";break;
		case AttackType.Siege:
			this.attackType1.text = "攻城";break;
		case AttackType.Chaos:
			this.attackType1.text = "混乱";break;
		}
		this.attackSpeed1.text = ua.attackInterval + "s/次";
		this.attackRange1.text = ua.attackRange + "/" + (ua.isMelee ? "近战" : "远程");
		this.armor1.text = ua.armor.ToString();
		switch(ua.armorType)
		{
		case ArmorType.None:
			this.armorType1.text = "无甲";break;
		case ArmorType.Light:
			this.armorType1.text = "轻甲";break;
		case ArmorType.Middle:
			this.armorType1.text = "中甲";break;
		case ArmorType.Heavy:
			this.armorType1.text = "重甲";break;
		case ArmorType.Construction:
			this.armorType1.text = "建筑";break;
		}
		this.corn1.text = ua.killPrice.ToString();
		this.skillInfo1.text = ua.skillInfo;
	}





}
