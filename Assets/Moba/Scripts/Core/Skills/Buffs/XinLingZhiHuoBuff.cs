using System;
using UnityEngine;

public class XinLingZhiHuoBuff:BuffBase
{

	public float tipsInterval = 2;//释放间隔
	float mNextTipsTime;

	public ArmorIncrease armorIncrease;
	public AttackIncreasePercent attackIncreasePercent;
	public int healthRecover;

	UnitAttribute mUnitAttribute;

	public override void OnEnter()
	{
		ResetNextTipsTime ();
		mUnitAttribute = unitBase.unitAttribute;
		this.mExitTime = Time.time + duration;
		unitBase.armorIncreases.Add (armorIncrease);
		unitBase.attackIncreasePercents.Add (attackIncreasePercent);
	}
	
	public override void OnUpdate()
	{
		if(mNextTipsTime < Time.time)
		{
			mUnitAttribute.currentHealth = Mathf.Min(mUnitAttribute.currentHealth + healthRecover,mUnitAttribute.maxHealth);
			unitBase.ShowMsgTips(3,"+" + healthRecover,Color.green,2,new Vector3(0,40,0));
			ResetNextTipsTime ();
		}
		if(mExitTime < Time.time)
		{
			OnExit();
		}
	}
	
	public override void OnExit()
	{
		unitBase.armorIncreases.Remove (armorIncrease);
		unitBase.attackIncreasePercents.Remove (attackIncreasePercent);
		unitBase.oldBuffs.Add (this.GetType());

	}

	void ResetNextTipsTime()
	{
		mNextTipsTime = Time.time + tipsInterval;
	}

}


