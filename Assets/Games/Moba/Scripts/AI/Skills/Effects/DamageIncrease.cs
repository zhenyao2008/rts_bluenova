using UnityEngine;
using System.Collections;

public class DamageIncrease{
	public int triggerOdds = 40;
	public int damageIncrease = 30;
	
	public int GetDamage(int damage)
	{
		bool reduce = Random.Range (0, 100) < triggerOdds ? true : false;
		if(reduce)
		{
			damage += damageIncrease;
		}
		return damage;
	}
}
