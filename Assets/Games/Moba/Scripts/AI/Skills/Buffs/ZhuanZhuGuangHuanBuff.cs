using UnityEngine;
using System.Collections;

public class ZhuanZhuGuangHuanBuff : BuffBase {

	public float checkInterval;
	float mNextCheckTime;
	public ArmorIncrease armorIncrease;
	public float ringRadius;//光环影响范围

	public override void OnEnter()
	{
		this.mNextCheckTime = Time.time + checkInterval;
		unitBase.armorIncreasesByLightRing = armorIncrease;
	}

	Collider[] mColls;
	public override void OnUpdate()
	{
		if(mNextCheckTime < Time.time)
		{
			mColls = Physics.OverlapSphere(unitBase.transform.position,ringRadius,1<<unitBase.gameObject.layer);
			for(int i=0;i<mColls.Length;i++)
			{
				UnitBase ub = mColls[i].GetComponent<UnitBase>();
				if(ub!=null)
				{
					if(ub.armorIncreasesByLightRing==null)
					{
						ub.armorIncreasesByLightRing = armorIncrease;
					}
					else if(ub.armorIncreasesByLightRing.armor < armorIncrease.armor)
					{
						ub.armorIncreasesByLightRing = armorIncrease;
					}
				}
			}
			mNextCheckTime = Time.time + checkInterval;
		}
	}
	
	public override void OnExit()
	{
		
	}


}
