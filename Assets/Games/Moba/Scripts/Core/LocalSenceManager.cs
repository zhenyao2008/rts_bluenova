using UnityEngine;
using System.Collections;

public class LocalSenceManager : MonoBehaviour {

	public GameObject treesPrefab;
	public GameObject wall0;
	public GameObject wall1;


	void Awake()
	{
		treesPrefab = Instantiate (treesPrefab);
		wall0 = Instantiate (wall0);
		wall1 = Instantiate (wall1);
	}

	private void Start()
	{
        BlueNoah.CameraControl.CameraController.Instance.SetCameraMoveArea(GameObject.Find("CameraMoveArea").GetComponent<BoxCollider>());
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.L))
		{
			StaticBatchingUtility.Combine(treesPrefab);
			StaticBatchingUtility.Combine(wall0);
			StaticBatchingUtility.Combine(wall1);
		}
	}

}
