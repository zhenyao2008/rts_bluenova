using UnityEngine;
using System.Collections;

public class UTweenRotation : MonoBehaviour {

	public delegate void OnForwardFinish();
	public OnForwardFinish onForwardFinish;

	public delegate void OnRevertFinish();
	public OnRevertFinish onRevertFinish;

	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
	public float delay;
	public float duration = 1;

	public Vector3 startRot;
	public Vector3 endRot;

	Quaternion mStartQua;
	Quaternion mEndQua;

	bool mIsForward;
	Transform mTrans;
	float t;
	float mPlayTime;
	void Awake()
	{
		mTrans = transform;
	}

	void Update(){
		if (mPlayTime > Time.time) {
			return;
		}
		if (mIsForward) {
			t += Time.deltaTime / duration;
			mTrans.localRotation = Quaternion.Lerp(mStartQua,mEndQua,curve.Evaluate(t));
			if(t >= 1)
			{
				this.enabled = false;
				if (onForwardFinish!=null)
					onForwardFinish ();
			}
		} else {
			t += Time.deltaTime / duration;
			mTrans.localRotation = Quaternion.Lerp(mStartQua,mEndQua,curve.Evaluate(1-t));
			if(t >= 1)
			{
				this.enabled = false;
				if(onRevertFinish!=null)
					onRevertFinish();
			}
		}
	}
	
	public void PlayForward()
	{
		this.enabled = true;
		transform.localEulerAngles = startRot;
		mStartQua = Quaternion.Euler (startRot);
		mEndQua = Quaternion.Euler (endRot);
		t = 0;
		mIsForward = true;
		mPlayTime = Time.time + delay;
	}
	
	public void PlayRevert()
	{
		this.enabled = true;
		transform.localEulerAngles = endRot;
		mStartQua = Quaternion.Euler (startRot);
		mEndQua = Quaternion.Euler (endRot);
		t = 0;
		mIsForward = false;
		mPlayTime = Time.time + delay;
	}
}
