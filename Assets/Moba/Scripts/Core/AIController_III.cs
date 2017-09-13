using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
public class AIController_III : MonoBehaviour ,IPlayerController{

	public PlayerAttribute playerAttribute;
	public int playerIndex;
	public int race = 0;
	ServerController_III mServerController_III;
//	public int minCornRemain = 300;


	void Start()
	{
		mServerController_III = ServerController_III.instance;
		mNextBuildTime = Time.time + commonInterval;

	}
	public float commonInterval = 0.5f;
	float mNextBuildTime = 0;

	void Update()
	{
		if( mNextBuildTime<Time.time)
		{
			mNextBuildTime = Time.time + commonInterval;
//			mNextBuildTime = Mathf.Infinity;
			UpdateBuilding();
		}
	}

	void UpdateBuilding()
	{
		Transform trans = null;

		List<GameObject> buildPrefabs = race == 0 ? mServerController_III.buildPrefabs : mServerController_III.buildPrefabs1;


		if (playerIndex == 0) {
			if( mServerController_III.availablePlanes0.Count == 0)
				return;
			trans = mServerController_III.availablePlanes0 [Random.Range (0, mServerController_III.availablePlanes0.Count)];
			int index = mServerController_III.planes0.IndexOf(trans);
			mServerController_III.SpawnBuilding(playerIndex,Random.Range(0,buildPrefabs.Count),index,race);
		
		} else {
			if( mServerController_III.availablePlanes1.Count == 0)
				return;
			trans = mServerController_III.availablePlanes1[Random.Range(0,mServerController_III.availablePlanes1.Count)];
			int index = mServerController_III.planes1.IndexOf(trans);
			mServerController_III.SpawnBuilding(playerIndex,Random.Range(0,buildPrefabs.Count),index,race);

		}
	}

	public int GetPlayerIndex()
	{
		return playerIndex;
	}

}
