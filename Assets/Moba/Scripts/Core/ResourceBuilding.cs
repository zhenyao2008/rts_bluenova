using UnityEngine;
using System.Collections;

public class ResourceBuilding : MonoBehaviour {

	public GameObject playerUIPrefab;
	public GameObject playerUI;
	public float scoreInterval = 2;
	public int score = 10;
	float nextScoreTime;

	void Start()
	{
		nextScoreTime = Time.time + scoreInterval;
		GameObject go = Instantiate (playerUIPrefab) as GameObject;
		playerUI = go;
		go.GetComponent<PlayerUI> ().followPoint = transform;
	}

	void Update(){
		if(nextScoreTime < Time.time)
		{
//			ShowScore(score);
		}
	}

	GameObject scoreGo;
	public void ShowScore(int num)
	{

	}


}
