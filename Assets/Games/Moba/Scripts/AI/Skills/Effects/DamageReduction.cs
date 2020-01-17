using UnityEngine;
using System.Collections;

public class DamageReduction {

	public int triggerOdds = 30;
	public int damageReduce = 30;

	public int GetDamage(int damage)
	{
		bool reduce = Random.Range (0, 100) < triggerOdds ? true : false;
		if(reduce)
		{
			damage -= damageReduce;
		}
		return damage;
	}

}
