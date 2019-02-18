using UnityEngine;
using System.Collections;

public class GizmosCube : MonoBehaviour {

	public Vector3 cubeSize= new Vector3(1,1,1);
	public Color color;

	void OnDrawGizmos()
	{
		Gizmos.DrawCube (transform.position, cubeSize);
	}

}
