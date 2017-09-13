using UnityEngine;
using System.Collections;

public class MuzzleFlashAnimate : MonoBehaviour {

	Transform mTrans;
	public Vector3 defaultScale = Vector3.one;

	void Start(){
		mTrans = transform;
		defaultScale = mTrans.localScale;
	}

	// Update is called once per frame
	void Update () {
		mTrans.localScale = defaultScale * Random.Range(0.5f,1.5f);
		mTrans.localEulerAngles = new Vector3(mTrans.localEulerAngles.x,mTrans.localEulerAngles.y,Random.Range(0,90)) ;
	}
}
