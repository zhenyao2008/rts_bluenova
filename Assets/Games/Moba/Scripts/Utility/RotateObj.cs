using UnityEngine;
using System.Collections;

public class RotateObj : MonoBehaviour {

	public float rotateSpeed = 100;
	Transform mTrans;

	void Start(){
		mTrans = transform;
	}

	void Update () {
		mTrans.Rotate (Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
