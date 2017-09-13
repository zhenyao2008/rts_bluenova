using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[ExecuteInEditMode]
public class LoadTowers : MonoBehaviour {

	public bool load = false;
	public int thisLayer;
	public int targetLayer;

	public Transform[] towerPoints0;
	public Transform[] towerPoints1;

	public GameObject towerPrefab0;
	public GameObject towerPrefab1;

	void Update () {
		if(load)
		{
			load = false;
			Load();
		}
	}

	public void Load()
	{
		foreach(Transform trans in towerPoints0)
		{
			GameObject go = Instantiate(towerPrefab0,trans.position,trans.rotation) as GameObject;
			go.transform.parent = transform;
			PlayerII p = go.GetComponent<PlayerII>();
			p.targetLayers = new System.Collections.Generic.List<int>();
			p.targetLayers.Add(targetLayer);
			go.layer = thisLayer;
			NetworkServer.Spawn(go);
		}
		foreach(Transform trans in towerPoints1)
		{
			GameObject go = Instantiate(towerPrefab1,trans.position,trans.rotation) as GameObject;
			go.transform.parent = transform;
			PlayerII p = go.GetComponent<PlayerII>();
			p.targetLayers = new System.Collections.Generic.List<int>();
			p.targetLayers.Add(targetLayer);
			go.layer = thisLayer;
			NetworkServer.Spawn(go);
		}
	}
}
