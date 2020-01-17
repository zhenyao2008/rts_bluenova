using UnityEngine;
using System.Collections;

public class SkillPanel : MonoBehaviour {

	public GameObject root;

	public UIEventTrigger skill0;
	public UILabel skillLabel0;

	public UIEventTrigger skill1;
	public UIEventTrigger skill2;

	public UIEventTrigger autoSelect;

	public UIEventTrigger backCity;

	public void SetPlayerConroller(PlayerController pc)
	{
//		skill0.onClick
	}


	public void CoolDown01(float cdTime)
	{
		Debug.Log ("cdTime:" + cdTime);
		skill0.enabled = false;
		StartCoroutine(_CoolDown (cdTime,skillLabel0));
	}

	float mCdTips = 0.1f;
	IEnumerator _CoolDown(float cdTime,UILabel label)
	{
		string deflautText = label.text;
		while(cdTime>0)
		{
			label.text =string.Format("{0:f1}", cdTime); 
			cdTime -= mCdTips;
			yield return new WaitForSeconds(mCdTips);
		}
		label.text = deflautText;
		skill0.enabled = true;
	}

}
