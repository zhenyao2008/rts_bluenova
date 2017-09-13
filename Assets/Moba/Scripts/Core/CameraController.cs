using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController instance;
	public static CameraController SingleTon(){
		return instance;
	}

	public RtsCamera rtsCamera;
	public float cameraSpeed = 50;
	public float minMovingDis = 0.1f;

	public bool isMoving;
	public bool isTilting;

	Vector3 preMousePos;
//	Vector3 preMousePos1;

	void Awake(){
		instance = this;
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_Drag += On_Drag;
		EasyTouch.On_Twist += On_Twist;
		EasyTouch.On_Pinch += On_Pinch;
	}
	
	void LateUpdate(){
		if(UICamera.isOverUI)
		{
			return;
		}

		if(EasyTouch.instance)
		{

			return;
		}

		if(Input.GetMouseButtonDown(0))
		{
			preMousePos = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(0))
		{
			isMoving = false;
		}
		if(Input.GetMouseButton(0))
		{
			Vector3 forward = this.rtsCamera.transform.forward;
			Vector3 right = this.rtsCamera.transform.right;
			forward.y = 0;
			right.y = 0;
			this.rtsCamera.LookAt -=cameraSpeed * forward.normalized * Input.GetAxis("Mouse Y") * Time.deltaTime;
			this.rtsCamera.LookAt -=cameraSpeed * right.normalized * Input.GetAxis("Mouse X") * Time.deltaTime;
			if(Vector3.Distance(preMousePos,Input.mousePosition) > minMovingDis)
			{
				isMoving = true;
			}
		}

		if(Input.GetMouseButtonDown(1))
		{
//			preMousePos1 = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(1))
		{
			isTilting = false;
		}
		if (Input.GetMouseButton (1)) 
		{
			this.rtsCamera.Tilt -= cameraSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
			this.rtsCamera.Rotation += cameraSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
		}

	}

	private Vector3 delta;
	
	void On_Twist (Gesture gesture){
		this.rtsCamera.Rotation += gesture.twistAngle;
//		transform.Rotate( Vector3.up * gesture.twistAngle);
	}
	
	void OnDestroy(){
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_Drag -= On_Drag;
		EasyTouch.On_Twist -= On_Twist;
	}
	
	
	void On_Drag (Gesture gesture){
		On_Swipe( gesture);
	}
	
	void On_Swipe (Gesture gesture){
		Vector3 forward = this.rtsCamera.transform.forward;
		Vector3 right = this.rtsCamera.transform.right;
		this.rtsCamera.LookAt -= cameraSpeed * right * gesture.deltaPosition.x / Screen.width;
		this.rtsCamera.LookAt -= cameraSpeed *  forward * gesture.deltaPosition.y / Screen.height;

//		transform.Translate( Vector3.left * gesture.deltaPosition.x / Screen.width);
//		transform.Translate( Vector3.back * gesture.deltaPosition.y / Screen.height);
	}
	
	void On_Pinch (Gesture gesture){	
		Camera.main.fieldOfView += gesture.deltaPinch * Time.deltaTime;
	}


//
//	void OnGUI()
//	{
//		GUI.Label (new Rect(10,10,100,30),UICamera.isOverUI.ToString());
//		GUI.Label (new Rect(10,40,100,30),Input.GetAxis("Mouse Y").ToString());
//		GUI.Label (new Rect(10,70,100,30),Input.GetAxis("Mouse X").ToString());
//	}

}
