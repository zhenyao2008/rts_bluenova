using UnityEngine;
using System.Collections;

public class UTweenPosition : MonoBehaviour {

	public delegate void OnForwardFinish();
	public OnForwardFinish onForwardFinish;

	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
	public float delay;
	public float duration = 1;

	public Vector3 startPos;
	public Vector3 endPos;

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
			mTrans.localPosition = Vector3.Lerp (startPos, endPos,curve.Evaluate(t));
			if(t >= 1)
			{
				this.enabled = false;
				if (onForwardFinish!=null)
					onForwardFinish ();
			}
		} else {
			t += Time.deltaTime / duration;
			mTrans.localPosition = Vector3.Lerp (startPos, endPos,curve.Evaluate(1-t));
			if(t >= 1)
			{
				this.enabled = false;
			}
		}
	}

	public void PlayForward()
	{
		this.enabled = true;
		transform.localPosition = startPos;
		t = 0;
		mIsForward = true;
		mPlayTime = Time.time + delay;
	}

	public void PlayRevert()
	{
		this.enabled = true;
		transform.localPosition = endPos;
		t = 0;
		mIsForward = false;
		mPlayTime = Time.time + delay;
	}

}
