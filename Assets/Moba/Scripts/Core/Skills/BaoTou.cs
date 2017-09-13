using UnityEngine;
using System.Collections;

public class BaoTou : SkillBase {

	public int triggerOdds = 40;
	public int damageIncrease = 30;
	
	public override void OnAwake()
	{
		DamageIncrease dr = new DamageIncrease ();
		dr.triggerOdds = this.triggerOdds;
		dr.damageIncrease = this.damageIncrease;
		unitBase.damageIncreases.Add (dr);
	}


}
