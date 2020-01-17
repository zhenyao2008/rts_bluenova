using UnityEngine;
using System.Collections;

public class UnSpawnDelay : MonoBehaviour {

	public float delay = 1;
	float unSpawnTime;

	void OnEnable()
	{
		unSpawnTime = Time.time + delay;
	}

	void Update()
	{
		if(unSpawnTime > Time.time)
		{
			gameObject.SetActive(false);
		}
	}

}
