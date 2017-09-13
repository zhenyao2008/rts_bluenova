using UnityEngine;
using System.Collections;

public class ResultPanel : MonoBehaviour {

	public GameObject root;

	public GameObject winNode;
	public UIButton winButton;

	public GameObject failNode;
	public UIButton failButton;

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
