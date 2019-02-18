using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//专注光环
public class ZhuanZhuGuangHuan : SkillBase {

	public GameObject caseEffect;
	float mNextCheckTime;

	public int ringPrefabIndex;
	public ArmorIncrease mArmorIncrease;


	public override void OnAwake()
	{
		Debug.Log ("OnAwake");
		unitBase.AddRing (ringPrefabIndex);
		ZhuanZhuGuangHuanBuff buff = new ZhuanZhuGuangHuanBuff ();
		buff.armorIncrease = mArmorIncrease;
		buff.ringRadius = 20;
		buff.unitBase = unitBase;
		unitBase.buffDics.Add (typeof(ZhuanZhuGuangHuanBuff), buff);
	}



}
