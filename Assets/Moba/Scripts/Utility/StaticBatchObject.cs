using UnityEngine;
using System.Collections;

public class StaticBatchObject : MonoBehaviour {

	float time;
	public bool staticBatch;
	void Awake()
	{
		time = Time.realtimeSinceStartup;
		if(staticBatch)StaticBatchingUtility.Combine (gameObject);
		Debug.Log (Time.realtimeSinceStartup  - time);
	}

	void Start()
	{
		if(staticBatch)StaticBatchingUtility.Combine (gameObject);
	}

}
