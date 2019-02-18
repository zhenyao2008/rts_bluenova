using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TestPro : MonoBehaviour {

	public float speed = 1;
	public List<Vector3> positions;


	float t = 0;
	int index = 0;
	void Update()
	{
		t += Time.deltaTime * speed;
		transform.position = Vector3.Lerp (positions[index%positions.Count],positions[(index +1) % positions.Count] ,t);
		if(t > 1)
		{
			index ++ ;
			t = t - 1;
		}

	}

}
