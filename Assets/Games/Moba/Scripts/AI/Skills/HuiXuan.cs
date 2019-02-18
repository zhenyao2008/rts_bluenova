using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HuiXuan : SkillBase {

	Collider[] mColls;
	public GameObject hitPrefab;

	public override bool IsSkillAble(){
		if(mNextTime < Time.time)
		{
			mColls = Physics.OverlapSphere (unitBase.transform.position,checkRadius,unitBase.targetLayer);
			if(mColls.Length >= minHitNum)
			{
				return true;
			}
		}
		return false;
	}

	public override void OnEnter(){
		mNextTime = Time.time + interval;
		mSkillHitTime = Time.time + skillHitDelay;
		skillDuration = Mathf.Max (skillHitDelay,skillDuration);
		mSkillOutTime = Time.time + skillDuration;
		unitBase.ChangeClientState (skillAnimName, UnitState.Skill);
	}

	public override void OnUpdate()
	{
		if(mSkillHitTime < Time.time)
		{
			mColls = Physics.OverlapSphere (unitBase.transform.position,checkRadius,unitBase.targetLayer);
			for(int i=0;i<mColls.Length;i++)
			{
				Enemy enemy = mColls[i].GetComponent<Enemy>();
				if(enemy!=null)
				{
					enemy.Damage(unitBase,unitBase.unitAttribute.GetHitDamage(enemy));
					if(hitPrefab!=null)
					{
						Transform t = enemy.GetHitPoint();
						GameObject go = Instantiate(hitPrefab,t.position,t.rotation) as GameObject;
						NetworkServer.Spawn(go);
						StartCoroutine(DestoryDelay(go,1));
					}
				}
			}
			mSkillHitTime = Mathf.Infinity;
		}
		if(mSkillOutTime<Time.time)
		{
			OnExit();
		}
	}

	IEnumerator DestoryDelay(GameObject obj,float delay){
		yield return new WaitForSeconds (delay);
		NetworkServer.Destroy (obj);
	}

	public override void OnExit()
	{
		unitBase.state = UnitState.Idle;
	}
}
