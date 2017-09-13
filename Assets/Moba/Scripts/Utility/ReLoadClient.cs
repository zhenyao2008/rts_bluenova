using UnityEngine;
using System.Collections;

public class ReLoadClient : MonoBehaviour {


	public void ReStart()
	{
		StartCoroutine (_ReStart());
	}

	IEnumerator _ReStart()
	{
		Debug.Log ("_ReStart");
		yield return new WaitForSeconds(10);
		Destroy (ServerController_II.GetInstance().gameObject);
		Application.LoadLevel ("BattlePVE");
	}

}
