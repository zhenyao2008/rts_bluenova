using UnityEngine;
using System.Collections;

public class ResultPanel : MonoBehaviour {

	public GameObject root;

	public GameObject winNode;
	public UIButton winButton;

	public GameObject failNode;
	public UIButton failButton;

	void Awake(){
		winButton.onClick.Add (new EventDelegate (ServerController_III.instance.StopHost));
		failButton.onClick.Add (new EventDelegate (ServerController_III.instance.StopHost));
	}

	public void Win()
	{
		winNode.SetActive (true);
		failNode.SetActive (false);
	}

	public void Fail()
	{
		failNode.SetActive (true);
		winNode.SetActive (false);
	}


}
