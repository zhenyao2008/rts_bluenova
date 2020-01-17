using UnityEngine;
using System.Collections;

public class CityPanelItem : MonoBehaviour {

	public TweenColor tc;
	public TweenPosition tp0;
	public TweenPosition tp1;

	public void Show()
	{
		if(tc)tc.PlayForward ();
//		tp0.ResetToBeginning ();
		tp0.PlayForward ();
//		tp0.onFinished.Clear ();
//		tp0.onFinished.Add (new EventDelegate(tp1.PlayForward));
	}

	public void Hide()
	{
		if(tc)tc.PlayReverse ();
		tp0.PlayReverse ();
//		tp1.ResetToBeginning ();
//		tp1.PlayForward ();
	}
}
