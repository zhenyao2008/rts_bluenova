//using System.Collections;
//using System.Collections.Generic;
//using BlueNoah.CameraControl;
//using BlueNoah.Math.FixedPoint;
//using BlueNoah.PathFinding;
//using BlueNoah.PathFinding.FixedPoint;
//using UnityEngine;
//using UnityEngine.Events;

//public class SimpleFindTester : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
		
//	}
	
//	// Update is called once per frame
//	void Update () {
//        if (Input.GetMouseButtonDown(0))
//        {
//            //Vector3 viewPos = (Vector3)Input.mousePosition;
//            //viewPos.z = 10;
//            ////mPathAgent.transform.position = new FixedPointVector3(target.position.x, target.position.y, target.position.z);//  Vector3Int.RoundToInt(target.position * 100);
//            //Vector3 pos = Camera.main.ScreenToWorldPoint(viewPos);
//            RaycastHit raycastHit;
//            GameObject.Find("Cube");
//            if (CameraController.Instance.RaycastByOrthographicCamera(out raycastHit, LayerConstant.LAYER_GROUND))
//            {
//                TestFind(GameObject.Find("Cube").transform.position.ToFixedPointVector3(), raycastHit.point.ToFixedPointVector3(), null);
//            }
//        }
//    }

//    public void TestFind(FixedPointVector3 startPos, FixedPointVector3 endPos, UnityAction<List<FixedPointNode>> onComplete)
//    {
//        StopAllCoroutines();
//        StartCoroutine(PathFindingMananger.Single.PathAgent._StartFind(startPos, endPos, null));
//    }
//}
