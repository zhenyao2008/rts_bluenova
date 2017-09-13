using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerUI : MonoBehaviour {


	public Player player;

	public GameObject infoTips;

	public Transform followPoint;
	public UnitAttribute unitAttribute;
//	public Camera uiCamera;
	public UISprite frant;
	public UISprite frant1;
	public UISprite specialFrant;//类似技能读条
	public UILabel uiName;
	public Vector3 offset = new Vector3(0,3,0);


//	float defaultWidth;
	void Start()
	{
//		defaultWidth = frant.width;
	}

	Vector3 pos;
	void Update()
	{
		if(followPoint!=null && frant!=null && unitAttribute!=null)
		{
//			frant.width = (int)(defaultWidth * (float)(unitAttribute.currentHealth) / unitAttribute.maxHealth);
			frant.fillAmount = ((float)(unitAttribute.currentHealth)) / unitAttribute.maxHealth;
			if(unitAttribute.currentHealth > 0 && unitAttribute.currentHealth < unitAttribute.maxHealth)
			{
				if(!frant.gameObject.activeInHierarchy){
					frant.gameObject.SetActive (true);
					if(frant1!=null)
						frant1.gameObject.SetActive(true);
				}
			}
		}
		if (followPoint == null)
			Destroy (gameObject);
	}

	void LateUpdate(){
		if (unitAttribute!=null && unitAttribute.currentHealth <= 0) {
			frant.gameObject.SetActive (false);
//			return;
		} 

		if (frant != null && UICamera.currentCamera) {
			Vector3 screenPos = Camera.main.WorldToScreenPoint (followPoint.position);
			pos = UICamera.currentCamera.ScreenToWorldPoint (screenPos) + offset;
			pos.z = 0;
			transform.position = pos;
		} else {

		}
	}




	GameObject[] infoTipsGos = new GameObject[4];
	public void ShowMsgTips(int index,string msg,Color color,float duration,Vector3 offset){
		//TODO,临时封闭普通攻击数字提示
		if(index == 0)
		{
			return;
		}

		if(infoTips!=null)
		{
			if(infoTipsGos[index]==null)
			{
				GameObject go = Instantiate(infoTips) as GameObject;
				infoTipsGos[index] = go;
			}


			infoTipsGos[index].transform.parent = transform;
			infoTipsGos[index].transform.position = transform.position;
			infoTipsGos[index].transform.localScale = Vector3.one;
			UILabel uiLabel = infoTipsGos[index].GetComponent<UILabel>();
			uiLabel.text=msg;
			uiLabel.color=color;
			TweenPosition tweenPosition = infoTipsGos[index].GetComponent<TweenPosition>();
			tweenPosition.from = new Vector3(0,Random.Range(5.0f,10.0f),0);
			tweenPosition.to =tweenPosition.from + offset + new Vector3(Random.Range(-5.0f,5.0f),Random.Range(5.0f,10.0f),0);
			tweenPosition.ResetToBeginning();
			tweenPosition.enabled = true;
			tweenPosition.duration = duration;
			TweenAlpha tweenAlpha = infoTipsGos[index].GetComponent<TweenAlpha>();
			tweenAlpha.ResetToBeginning();
			tweenAlpha.enabled = true;
			tweenAlpha.duration = duration;
		}
	}

	public void SpecialFrant(float dur)
	{
		StopCoroutine ("_SpecialFrant");
		if (specialFrant != null)
			StartCoroutine ("_SpecialFrant", dur * 0.9f);
	}

	IEnumerator _SpecialFrant(float dur){
		specialFrant.gameObject.SetActive (true);
		specialFrant.fillAmount = 0;
		int defaultWidth = specialFrant.width;
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime/dur;
			specialFrant.width = (int)(t * defaultWidth);
			yield return null;
		}
		specialFrant.gameObject.SetActive (false);
	}

}
