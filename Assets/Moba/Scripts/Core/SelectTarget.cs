using UnityEngine;
using System.Collections;

public class SelectTarget : MonoBehaviour {

	Transform mTrans;
	public Transform followTarget;

	void Start(){
		mTrans = transform;
	}

	void Update(){
		if (followTarget != null) {
			mTrans.position = followTarget.position;
		} else {
			mTrans.position = Vector3.down;
		}
	}

}
