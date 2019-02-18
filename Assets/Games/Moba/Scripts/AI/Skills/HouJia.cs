using UnityEngine;
using System.Collections;

//厚甲
public class HouJia : SkillBase {

	public int triggerOdds = 15;
	public int damageReduce = 15;

	public override void OnAwake()
	{
		DamageReduction dr = new DamageReduction ();
		dr.triggerOdds = this.triggerOdds;
		dr.damageReduce = this.damageReduce;
		unitBase.damageReductions.Add (dr);
	}

}
