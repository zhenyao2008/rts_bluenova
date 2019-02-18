using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ServerController : NetworkManager {

	public List<Transform> spawnPoint_Archer0;
	public List<Transform> spawnPoint_Peltast0;

	public List<Transform> spawnPoint_Archer1;
	public List<Transform> spawnPoint_Peltast1;

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		Debug.Log ("Server Start!");
		StartCoroutine (_SpawnSoldier());
	}

//	public GameObject SpawnSoldier(int index)
//	{
//		GameObject go = Instantiate<GameObject> (this.spawnPrefabs[index]);
//		return go;
//	}

	public void SpawnSolders(List<Transform> spawnPoints,GameObject prefab){
		for(int i=0;i<spawnPoints.Count;i++)
		{
			GameObject go = Instantiate(prefab,spawnPoints[i].position,spawnPoints[i].rotation) as GameObject;
			NetworkServer.Spawn(go);
		}
	}

	public int spawnSoldierInterval = 5;
	IEnumerator _SpawnSoldier()
	{
		while(!NetworkServer.active)
		{


			yield return new WaitForSeconds(0.5f);
		}

//		yield return new WaitForSeconds(spawnSoldierInterval);
		while(true)
		{
			SpawnSolders(spawnPoint_Archer0,spawnPrefabs[0]);
			SpawnSolders(spawnPoint_Archer1,spawnPrefabs[0]);
			SpawnSolders(spawnPoint_Peltast0,spawnPrefabs[1]);
			SpawnSolders(spawnPoint_Peltast1,spawnPrefabs[1]);
			yield return new WaitForSeconds(spawnSoldierInterval);
		}
	}




}
