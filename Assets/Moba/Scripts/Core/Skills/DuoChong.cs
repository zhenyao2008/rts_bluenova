using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//多重箭
//描述：对范围的随机N个目标进行总计M次攻击。
public class DuoChong : SkillBase {

	Collider[] mColls;
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
			StartCoroutine(_Attack(mColls));
			mSkillHitTime = Mathf.Infinity;
		}
		if(mSkillOutTime<Time.time)
		{
			OnExit();
		}
	}

	IEnumerator _Attack(Collider[] colls){
		int index = 0;
		for(int i=0;i<maxHitNum;i++)
		{
			Enemy enemy = mColls[Random.Range(0,colls.Length)].GetComponent<Enemy>();
			if(enemy!=null)
			{
				unitBase.RemoteAttack(enemy);
			}
			index++;
			yield return new WaitForSeconds(0.05f);
		}
		yield return null;
	}



	IEnumerator DestoryDelay(GameObject obj,float delay){
		yield return new WaitForSeconds (delay);
		NetworkServer.Destroy (obj);
	}


}
