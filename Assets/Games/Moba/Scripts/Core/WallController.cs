using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

	public WallGroup currentWall;

	public int wallLayer = 20;

	public static WallController instance;
	public static WallController SingleTon(){
		return instance;
	}


	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity,1<<wallLayer))
			{
				currentWall = hit.transform.gameObject.GetComponent<WallGroup>();
			}
		}
	}



}
