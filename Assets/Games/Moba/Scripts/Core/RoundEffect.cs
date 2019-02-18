using UnityEngine;
using System.Collections;

public class RoundEffect : MonoBehaviour {

	public float speed = 1;
	public Vector3[] positions;
	
	
	float t = 0;
	int index = 0;
	void Update()
	{
		if (positions != null && positions.Length > 1) {
			t += Time.deltaTime * speed;
			transform.position = Vector3.Lerp (positions[index%positions.Length],positions[(index +1) % positions.Length] ,t);
			if(t > 1)
			{
				index ++ ;
				t = t - 1;
			}
		}
	}
}
