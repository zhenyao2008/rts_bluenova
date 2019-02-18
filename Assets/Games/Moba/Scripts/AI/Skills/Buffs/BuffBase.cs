using UnityEngine;
using System.Collections;


public enum SkillEffectType{
	DamageReduction
};

//buff，effect，skill
public class BuffBase{

	public UnitBase unitBase;

//	public float tipsInterval = 2;//释放间隔
	public float duration = 10;//持续时间
	public int priority=0;//权重，相同技能不同属性下使用

	protected float mExitTime;

	public virtual void OnEnter()
	{

	}

	public virtual void OnUpdate()
	{

	}

	public virtual void OnExit()
	{

	}

}
