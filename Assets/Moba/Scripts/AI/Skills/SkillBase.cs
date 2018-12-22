using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SkillBase : NetworkBehaviour {

	public float interval;
	protected float mNextTime;

	public string skillAnimName;
	public float skillHitDelay;//伤害触发前置时间
	protected float mSkillHitTime;
	public float skillDuration;//技能持续时间，在这个期间不能做其他动作，除非强行打断。
	protected float mSkillOutTime;

	public int damage;
	public float checkRadius = 20;

	public GameObject shootPrefab;

	public int minHitNum = 3;//对于群体攻击，至少有n个目标才会激发
	public int maxHitNum = 6;//对于群体目标，最多攻击的目标数

	public int mana;

	public UnitBase unitBase;



	public virtual void OnAwake(){

	}

	public virtual void OnEnter()
	{
		mSkillOutTime = skillDuration + Time.time;
	}

	public virtual void OnUpdate(){
	
	}

	public virtual void OnExit(){
		unitBase.state = UnitState.Idle;
	}

	//技能进入的条件
	public virtual bool IsSkillAble()
	{
		return false;
	}



}



