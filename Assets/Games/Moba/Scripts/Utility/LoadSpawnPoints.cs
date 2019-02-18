using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class LoadSpawnPoints : MonoBehaviour {

	public bool Load;
	public bool p0;
	public float left=0;
	public float leftPading = 1.4f;

	public float top=0;
	public float topPading = 2.0f;

	public float forward = 1;
	void Update()
	{
		if(Load)
		{
			points.Clear();
			Load = false;
			LoadPoints(transform);

//			for(int i=0;i<points.Count;i++)
//			{
//				points[i].position = new Vector3(left - i%6 *leftPading,0,top + i / 6 * topPading);
//			}

//			if(p0)
//				FindObjectOfType<ServerController_III>().spawnPoints0 = points;
//			else
//				FindObjectOfType<ServerController_III>().spawnPoints1 = points;
		}
	}

	public List<Transform> points;
	void LoadPoints(Transform trans)
	{
		int childCount = trans.childCount;
		for (int i =0; i<childCount; i++) {
			Transform t = trans.GetChild(i);
			if(t.name == "SpawnPoint")
			{
				points.Add(t);
				t.localPosition = new Vector3(0,0,forward);
			}
			LoadPoints(t);
		}
	}

}
