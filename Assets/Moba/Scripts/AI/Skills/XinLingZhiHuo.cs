using UnityEngine;
using System.Collections;

//心灵之火
//这个相当于是AI的Action。
public class XinLingZhiHuo : SkillBase {

	public float attackIncreasePercent;
	public int healthRecover;
	public int armorIncrease;
	public int priority = 0;

	public override void OnAwake(){

	}

	Collider[] mColls;
	UnitBase skillable;
	public override bool IsSkillAble(){
		if(mNextTime < Time.time)
		{
			skillable = null;
			mColls = Physics.OverlapSphere (unitBase.transform.position,checkRadius,1<<unitBase.gameObject.layer);
			for(int i=0;i<mColls.Length;i++)
			{
				if(mColls[i].GetComponent<UnitBase>().unitAttribute.currentHealth >0 && mColls[i].GetComponent<UnitBase>().unitAttribute.currentHealth < mColls[i].GetComponent<UnitBase>().unitAttribute.maxHealth )
				{
					BuffBase buffBase;
					if(mColls[i].GetComponent<UnitBase>().buffDics.TryGetValue(typeof(XinLingZhiHuo),out buffBase))
					{
						if(buffBase.priority<priority)
						{
							skillable = mColls[i].GetComponent<UnitBase>();
							return true;
						}
					}
					else
					{
						skillable = mColls[i].GetComponent<UnitBase>();
						return true;
					}
				}
			}
		}
		return false;
	}

	public override void OnEnter()
	{
		mNextTime = Time.time + interval;
		mSkillHitTime = Time.time + skillHitDelay;
		skillDuration = Mathf.Max (skillHitDelay,skillDuration);
		mSkillOutTime = Time.time + skillDuration;
		unitBase.ChangeClientState (skillAnimName, UnitState.Skill);
	}

	public override void OnUpdate(){

		BuffBase buffBase;
		if (mSkillHitTime < Time.time) {
			if(skillable==null || skillable.unitAttribute.currentHealth <=0)
			{
				skillable = GetTarget();
			}
			else if (skillable.buffDics.ContainsKey(typeof(XinLingZhiHuoBuff)) && skillable.buffDics.TryGetValue (typeof(XinLingZhiHuoBuff), out buffBase)) {
				if(buffBase.priority >= priority)
				{
					skillable = GetTarget();
				}
			}
			if(skillable == null)
			{
				return;
			}

			if (skillable.buffDics.TryGetValue (typeof(XinLingZhiHuoBuff), out buffBase)) {
				skillable.buffDics.Remove (typeof(XinLingZhiHuoBuff));
			}
			_Hit ();

			mSkillHitTime = Mathf.Infinity;
		} 
//		else {
//			if(skillable==null || skillable.unitAttribute.currentHealth <=0)
//			{
//				Debug.Log("1");
//				skillable = null;
//				mNextTime = 0;
//				unitBase.state = UnitState.Idle;
//				return;
//			}
//			if (skillable.buffDics.ContainsKey(typeof(XinLingZhiHuoBuff)) && skillable.buffDics.TryGetValue (typeof(XinLingZhiHuoBuff), out buffBase)) {
//				if(buffBase.priority >= priority)
//				{
//					Debug.Log("2");
//					skillable = null;
//					mNextTime = 0;
//					unitBase.state = UnitState.Idle;
//					return;
//				}
//			}
//		
//		}
		if(mSkillOutTime<Time.time)
		{
			OnExit();
		}
	}

	UnitBase GetTarget()
	{
		mColls = Physics.OverlapSphere (unitBase.transform.position,checkRadius,1<<unitBase.gameObject.layer);
		for(int i=0;i<mColls.Length;i++)
		{
			if(mColls[i].GetComponent<UnitBase>().unitAttribute.currentHealth >0 && mColls[i].GetComponent<UnitBase>().unitAttribute.currentHealth < mColls[i].GetComponent<UnitBase>().unitAttribute.maxHealth )
			{
				BuffBase buffBase;
				if(mColls[i].GetComponent<UnitBase>().buffDics.TryGetValue(typeof(XinLingZhiHuo),out buffBase))
				{
					if(buffBase.priority<priority)
					{
						return mColls[i].GetComponent<UnitBase>();
					}
				}
				else
				{
					return mColls[i].GetComponent<UnitBase>();
				}
			}
		}
		return null;
	}


	void _Hit(){
		XinLingZhiHuoBuff buff = new XinLingZhiHuoBuff ();
		buff.duration = 10;
		buff.tipsInterval = 2;
		buff.unitBase = skillable;
		ArmorIncrease armorInc = new ArmorIncrease ();
		armorInc.armor = this.armorIncrease;
		buff.armorIncrease = armorInc;
		AttackIncreasePercent aip = new AttackIncreasePercent ();
		aip.percent = this.attackIncreasePercent;
		buff.attackIncreasePercent = aip;
		buff.healthRecover = healthRecover;
		skillable.buffDics.Add (typeof(XinLingZhiHuoBuff), buff);
		buff.OnEnter ();
	}



}
