using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.Event
{
    public enum TouchType { TouchBegin, TouchEnd, Touch, Click, LongPress, TwoFingerBegin, TwoFinger, TwoFingerEnd};

    public class EventData
    {
        public Vector3 touchStartPos0;
        public Vector3 touchPos0;
        public Vector3 deltaTouchPos0;
        public float touchStartTime0;

        public Vector3 touchStartPos1;
        public Vector3 touchPos1;
        public Vector3 deltaTouchPos1;
        public float touchStartTime1;

        public float deltaAngle;
        public float pinchDistance;
        public float deltaPinchDistance;
    }

    public class EasyInput : SimpleSingleMonoBehaviour<EasyInput>
    {
        //Click interval max(same as ugui system)
        //Double click interval 0.3f~0.5f
        Dictionary<TouchType, List<UnityAction<EventData>>> touchActionDic;
        Dictionary<TouchType, List<UnityAction<EventData>>> lateTouchActionDic;
        EventData mEventData;

        protected override void Awake()
        {
            base.Awake();
            mEventData = new EventData();
            touchActionDic = new Dictionary<TouchType, List<UnityAction<EventData>>>();
            //TODO use AddListener as UnityEvent;
            touchActionDic.Add(TouchType.TouchBegin, new List<UnityAction<EventData>>());
            touchActionDic.Add(TouchType.TouchEnd, new List<UnityAction<EventData>>());
            touchActionDic.Add(TouchType.Touch, new List<UnityAction<EventData>>());
            touchActionDic.Add(TouchType.Click, new List<UnityAction<EventData>>());
            touchActionDic.Add(TouchType.LongPress, new List<UnityAction<EventData>>());
            touchActionDic.Add(TouchType.TwoFingerBegin, new List<UnityAction<EventData>>());
            touchActionDic.Add(TouchType.TwoFinger, new List<UnityAction<EventData>>());
            touchActionDic.Add(TouchType.TwoFingerEnd, new List<UnityAction<EventData>>());

            lateTouchActionDic = new Dictionary<TouchType, List<UnityAction<EventData>>>();
            lateTouchActionDic.Add(TouchType.TouchBegin, new List<UnityAction<EventData>>());
            lateTouchActionDic.Add(TouchType.TouchEnd, new List<UnityAction<EventData>>());
            lateTouchActionDic.Add(TouchType.Touch, new List<UnityAction<EventData>>());
            lateTouchActionDic.Add(TouchType.Click, new List<UnityAction<EventData>>());
            lateTouchActionDic.Add(TouchType.LongPress, new List<UnityAction<EventData>>());
            lateTouchActionDic.Add(TouchType.TwoFingerBegin, new List<UnityAction<EventData>>());
            lateTouchActionDic.Add(TouchType.TwoFinger, new List<UnityAction<EventData>>());
            lateTouchActionDic.Add(TouchType.TwoFingerEnd, new List<UnityAction<EventData>>());
        }

        public void RemoveAllListener(TouchType touchType)
        {
            if (touchActionDic.ContainsKey(touchType))
            {
                touchActionDic.Remove(touchType);
            }
        }

        public void AddListener(TouchType touchType, UnityAction<EventData> unityAction)
        {
            touchActionDic[touchType].Add(unityAction);
        }

        public void AddLateUpdateListener(TouchType touchType, UnityAction<EventData> unityAction)
        {
            lateTouchActionDic[touchType].Add(unityAction);
        }

        bool GetTouchDown()
        {
            bool isDown = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetMouseButtonDown(0))
            {
                isDown = true;
            }
#else
            if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
            {
                isDown = true;
            }
#endif
            return isDown;
        }

        bool GetTouch()
        {
            bool isPress = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetMouseButton(0))
            {
                isPress = true;
            }
#else
            if (Input.touchCount == 1 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary))
            {
                isPress = true;
            }
#endif
            return isPress;
        }

        bool GetLongPress()
        {
            return false;
        }

        bool GetTouchUp()
        {
            bool isUp = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetMouseButtonUp(0))
            {
                //mouseUpTime = Time.realtimeSinceStartup;
                //endPos = Input.mousePosition;
                isUp = true;
            }
#else
            if (Input.touchCount == 1 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled))
            {
                //mouseUpTime = Time.realtimeSinceStartup;
                //endPos = Input.touches[0].position;
                isUp = true;
            }
#endif
            return isUp;
        }

        public Vector3 GetMousePosition()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            return Input.mousePosition;
#else
        if(Input.touches.Length == 1){
            return (Vector3)Input.touches[0].position;
        }else{
            return Vector3.zero;
        }
#endif
        }

        bool GetMouseClick()
        {
            //範囲の判断はもっと確認したい。
            bool isClick = false;
            float distance = Vector3.Distance(mEventData.touchStartPos0, GetTouchPosition(0));
            if (distance / Screen.height < 0.02f)
            {
                isClick = true;
            }
            return isClick;
        }

        bool GetTwoFingerBegin()
        {
            bool isTwoFingerDown = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if ((Input.GetKeyDown(KeyCode.Q) && Input.GetMouseButton(0)) || (Input.GetKey(KeyCode.Q) && Input.GetMouseButtonDown(0))
               || (Input.GetKeyDown(KeyCode.Q) && Input.GetMouseButtonDown(0)))
            {
                isTwoFingerDown = true;

            }
#else
            if ((Input.touches.Length == 2 && Input.touches[1].phase == TouchPhase.Began && Input.touches[0].phase != TouchPhase.Ended && Input.touches[0].phase != TouchPhase.Canceled) 
                || (Input.touches.Length == 2 && Input.touches[0].phase == TouchPhase.Began && Input.touches[1].phase != TouchPhase.Ended && Input.touches[1].phase != TouchPhase.Canceled))
            {
                isTwoFingerDown =  true;
            }
#endif
            return isTwoFingerDown;
        }

        bool GetTwoFinger()
        {
            bool isTwoFinger = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetKey(KeyCode.Q) && Input.GetMouseButton(0))
            {
                isTwoFinger = true;
            }
#else
            if(Input.touches.Length == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary) && (Input.touches[1].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Stationary)){
                isTwoFinger = true;
            }
#endif
            return isTwoFinger;
        }

        bool GetTwoFingerEnd()
        {
            bool isTwoFingerUp = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if ((Input.GetKeyUp(KeyCode.Q) && Input.GetMouseButton(0)) || (Input.GetKey(KeyCode.Q) && Input.GetMouseButtonUp(0))
                || (Input.GetKeyUp(KeyCode.Q) && Input.GetMouseButtonUp(0)))
            {
                isTwoFingerUp = true;
            }
#else
            if ((Input.touches.Length == 2 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)) 
                || (Input.touches.Length == 2 && (Input.touches[1].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Canceled)))
            {
                isTwoFingerUp =  true;
            }
#endif
            return isTwoFingerUp;
        }

        void OnActions(List<UnityAction<EventData>> unityActions)
        {
            for (int i = 0; i < unityActions.Count; i++)
            {
                if (unityActions[i] != null)
                    unityActions[i](mEventData);
            }
        }

        void Update()
        {
            if (GetTouchDown())
            {
                OnTouchDown();
                OnActions(touchActionDic[TouchType.TouchBegin]);
            }
            if (GetTouch())
            {
                OnTouch();
                OnActions(touchActionDic[TouchType.Touch]);
            }
            if (GetTouchUp())
            {
                OnTouchUp();
                if (GetMouseClick())
                {
                    OnActions(touchActionDic[TouchType.Click]);
                }
                OnActions(touchActionDic[TouchType.TouchEnd]);
            }
            if (GetTwoFingerBegin())
            {
                OnTwoFingerBegin();
                OnActions(touchActionDic[TouchType.TwoFingerBegin]);
            }
            if (GetTwoFinger())
            {
                OnTwoFinger();
                OnActions(touchActionDic[TouchType.TwoFinger]);
            }
            if (GetTwoFingerEnd())
            {
                OnTwoFingerEnd();
                OnActions(touchActionDic[TouchType.TwoFingerEnd]);
            }
        }

        void OnTouchDown()
        {
            mEventData.touchStartPos0 = GetTouchPosition(0);
            mEventData.touchPos0 = mEventData.touchStartPos0;
            mEventData.touchStartTime0 = Time.realtimeSinceStartup;
        }

        void OnTouch()
        {
            mEventData.deltaTouchPos0 = GetTouchPosition(0) - mEventData.touchPos0;
            mEventData.touchPos0 = GetTouchPosition(0);
        }

        void OnTouchUp()
        {
            mEventData.deltaTouchPos0 = GetTouchPosition(0) - mEventData.touchPos0;
            mEventData.touchPos0 = GetTouchPosition(0);
        }

        void OnTwoFingerBegin()
        {
            mEventData.touchPos0 = GetTouchPosition(0);
            mEventData.touchPos1 = GetTouchPosition(1);
            mEventData.touchStartPos1 = GetTouchPosition(1);
            mEventData.pinchDistance = Vector3.Distance(mEventData.touchPos0, mEventData.touchPos1);
        }

        void OnTwoFinger()
        {
            mEventData.deltaTouchPos0 = GetTouchPosition(0) - mEventData.touchPos0;
            mEventData.deltaTouchPos1 = GetTouchPosition(1) - mEventData.touchPos1;
            mEventData.deltaAngle = SignedAngle(GetTouchPosition(1) - GetTouchPosition(0), mEventData.touchPos1 - mEventData.touchPos0,new Vector3(0,0,1));
            mEventData.touchPos0 = GetTouchPosition(0);
            mEventData.touchPos1 = GetTouchPosition(1);
            float currentDistance = Vector3.Distance(mEventData.touchPos0, mEventData.touchPos1);
            mEventData.deltaPinchDistance = currentDistance - mEventData.pinchDistance;
            mEventData.pinchDistance = currentDistance;
        }

        float SignedAngle(Vector3 from,Vector3 to,Vector3 axis){
            Vector3 normalized = from.normalized;
            Vector3 normalized2 = to.normalized;
            float num = Mathf.Acos(Mathf.Clamp(Vector3.Dot(normalized, normalized2), -1f, 1f)) * 57.29578f;
            float num2 = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(normalized, normalized2)));
            return num * num2;
        }


        void OnTwoFingerEnd()
        {

        }

        void OnLongPress()
        {

        }

        Vector3 GetTouchPosition(int index)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            return Input.mousePosition;
#else
            return Input.touches[index].position;
#endif
        }

        void LateUpdate()
        {
            if (GetTouchDown())
            {
                OnActions(lateTouchActionDic[TouchType.TouchBegin]);
            }
            if (GetTouch())
            {
                OnActions(lateTouchActionDic[TouchType.Touch]);
            }
            if (GetTouchUp())
            {
                if (GetMouseClick())
                {
                    OnActions(lateTouchActionDic[TouchType.Click]);
                }
                OnActions(lateTouchActionDic[TouchType.TouchEnd]);
            }
            if (GetTwoFingerBegin())
            {
                OnActions(lateTouchActionDic[TouchType.TwoFingerBegin]);
            }
            if (GetTwoFinger())
            {
                OnActions(lateTouchActionDic[TouchType.TwoFinger]);
            }
            if (GetTwoFingerEnd())
            {
                OnActions(lateTouchActionDic[TouchType.TwoFingerEnd]);
            }
        }

    }
}
