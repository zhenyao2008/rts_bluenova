using UnityEngine;
using System.Collections;

public class TestAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		mAnim = GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	Animation mAnim;

	void OnGUI()
	{
//		int clipCounts = mAnim.GetClipCount ();
		int index = 0;
		foreach(AnimationState state in mAnim)
		{
			if(index<35)
			{
				if(GUI.Button(new Rect(10,10 + 20 * index,300,20),state.name))
				{
					mAnim.wrapMode = WrapMode.Loop;
					mAnim.Play(state.name);
				}
			}
			else
			{
				if(GUI.Button(new Rect(120,10 + 20 * (index-35),300,20),state.name))
				{
					mAnim.wrapMode = WrapMode.Loop;
					mAnim.Play(state.name);
				}
			}

			index ++;
		}
	}
}
