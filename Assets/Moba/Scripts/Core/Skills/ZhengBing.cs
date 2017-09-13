using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZhengBing : SkillBase {

	public GameObject soilderPrefab;
	public int soilderCount;
	public GameObject startPrefab;
	public AudioClip castClip;

	public override bool IsSkillAble(){
		if(mNextTime < Time.time)
		{
			if(Physics.OverlapSphere (unitBase.transform.position,checkRadius,unitBase.targetLayer).Length > 0)
			{
				return true;
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
		RpcPlayAudio ();
		unitBase.unitAttribute.mana -= mana;

	}

	[ClientRpc]
	public void RpcPlayAudio(){
		if (castClip) {
			GetComponent<AudioSource> ().clip = castClip;
			GetComponent<AudioSource> ().Play();
		}
	}

	[ClientRpc]
	public void RpcPlayEffect(Vector3 pos){
		BattleFramework.PoolManager.SingleTon ().Spawn (startPrefab,pos,Quaternion.identity,0,3);
	}

	public override void OnUpdate()
	{
		if(mSkillHitTime < Time.time)
		{
			mSkillHitTime = Mathf.Infinity;
			Vector3 pos = unitBase.transform.position - unitBase.transform.forward * Random.Range(3,5);
			for(int i=0;i<soilderCount;i++)
			{
				Vector3 realPos = pos + new Vector3(Random.Range(-2.0f,2.0f),0,Random.Range(-2.0f,2.0f));
				GameObject go = Instantiate(soilderPrefab,realPos,unitBase.transform.rotation) as GameObject;
				Enemy soilder = go.GetComponent<Enemy>();
				go.layer = unitBase.gameObject.layer;
				soilder.targetLayers = unitBase.targetLayers;
				NetworkServer.Spawn(go);
				RpcPlayEffect(realPos);
			}

		}
		if(mSkillOutTime<Time.time)
		{
			OnExit();
		}
	}

//	IEnumerator _Cast()
//	{
//
//	}






}
