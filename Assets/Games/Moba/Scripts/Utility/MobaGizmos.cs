using UnityEngine;
using System.Collections;

public class MobaGizmos : MonoBehaviour {

	public float radius =0.5f;
	public Color color = Color.green;


	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawWireSphere (transform.position,radius);
	}

}
