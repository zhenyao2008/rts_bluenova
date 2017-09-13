using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingUI : MonoBehaviour {

	public Transform followPoint;
	public CityBuilding cityBuilding;
	public Image resourceImage;
	public Image resourceImage1;
	public Text resourceText;
	public UTweenPosition tweenPosition;
	public Vector3 tweenOffset = new Vector3 (0, 5, 0);

	Vector3 mPos;
	public Vector3 posOffset;

	void Awake(){
		tweenPosition.onForwardFinish += HideText;
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.H))
		{
			resourceText.enabled = true;
			resourceText.text = "+120000";
			resourceText.transform.localPosition = Vector3.zero;
			tweenPosition.startPos = Vector3.zero;
			tweenPosition.endPos = Vector3.zero + tweenOffset;
			tweenPosition.PlayForward();
		}
	}

	void LateUpdate()
	{
//		if (frant != null && UICamera.currentCamera) {
//			Vector3 screenPos = Camera.main.WorldToScreenPoint (followPoint.position);
//			pos = UICamera.currentCamera.ScreenToWorldPoint (screenPos) + offset;
//			pos.z = 0;
//			transform.position = pos;
//		}


		Vector3 screenPos = Camera.main.WorldToScreenPoint(followPoint.position);
		mPos = CityController.SingleTon().cameraUI.ScreenToWorldPoint (screenPos) + new Vector3(posOffset.x * (screenPos.x-Screen.width/2) / Screen.width/2, posOffset.y * Mathf.Cos(CameraController.SingleTon().rtsCamera.Tilt / 90) , 0);
		mPos.z = 0;
		transform.position = mPos;
	}


	void HideText()
	{
		resourceText.enabled = false;
	}
}
