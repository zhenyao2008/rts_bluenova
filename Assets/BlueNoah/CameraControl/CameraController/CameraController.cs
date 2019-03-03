﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using BlueNoah.Event;
using BlueNoah.Utility;

namespace BlueNoah.CameraControl
{
    [DefaultExecutionOrder(-100)]
    public class CameraController : SimpleSingleMonoBehaviour<CameraController>
    {
        bool mIsControllable = true;
        bool mIsMoveable = true;
        Camera mCamera;
        BoxCollider mMoveArea;
        BaseCameraPinchService mBaseCameraPinchService;
        BaseCameraRotateService mBaseCameraRotateService;
        BaseCameraMoveService mBaseCameraMoveService;

        protected override void Awake()
        {
            base.Awake();
            mCamera = GetComponent<Camera>();
            if (mCamera == null)
                mCamera = Camera.main;

            mBaseCameraRotateService = new BaseCameraRotateService(mCamera);

            if (mCamera.orthographic)
            {
                mBaseCameraMoveService = new OrthographicCameraMoveService(mCamera);
                mBaseCameraPinchService = new OrthographicsCameraPinchService(mCamera);
            }
            else
            {
                mBaseCameraMoveService = new PerspectiveCameraMoveService(mCamera);
                mBaseCameraPinchService = new PerspectiveCameraPinchService(mCamera);
            }

            mBaseCameraMoveService.Init();
            mBaseCameraPinchService.Init();

            gameObject.GetOrAddComponent<EasyInput>();
        }

        void Start()
        {

            EasyInput.Instance.AddLateUpdateListener(Event.TouchType.TouchBegin, OnTouchBegin);

            EasyInput.Instance.AddLateUpdateListener(Event.TouchType.Touch, OnTouch);

            EasyInput.Instance.AddLateUpdateListener(Event.TouchType.TouchEnd, OnTouchEnd);

            EasyInput.Instance.AddLateUpdateListener(Event.TouchType.TwoFingerBegin, OnPinchBegin);

            EasyInput.Instance.AddLateUpdateListener(Event.TouchType.TwoFinger, OnPinch);

            EasyInput.Instance.AddLateUpdateListener(Event.TouchType.TwoFingerEnd, OnPinchEnd);

#if UNITY_EDITOR
            IsVerticalRotateable = true;
            IsHorizontalRotateable = true;
#endif
        }

        public float MinPinchSize
        {
            set
            {
                mBaseCameraPinchService.minSize = value;
            }
            get
            {
                return mBaseCameraPinchService.maxSize;
            }
        }

        public float MaxPinchSize
        {
            set
            {
                mBaseCameraPinchService.maxSize = value;
            }
            get
            {
                return mBaseCameraPinchService.maxSize;
            }
        }

        public bool IsControllable
        {
            get
            {
                return mIsControllable;
            }
            set
            {
                mIsControllable = value;
            }
        }

        public bool IsMoveable
        {
            get
            {
                return mIsMoveable;
            }
            set
            {
                mIsMoveable = value;
                if (!mIsMoveable)
                {
                    mBaseCameraMoveService.ClearRemainDistance();
                }
            }
        }

        public bool IsPinchable
        {
            get
            {
                return mBaseCameraPinchService.IsPinchable;
            }
            set
            {
                mBaseCameraPinchService.IsPinchable = value;
            }
        }

        public bool IsVerticalRotateable
        {
            get
            {
                return mBaseCameraRotateService.IsVerticalRotateable;
            }
            set
            {
                mBaseCameraRotateService.IsVerticalRotateable = value;
            }
        }

        public bool IsHorizontalRotateable
        {
            get
            {
                return mBaseCameraRotateService.IsHorizontalRotateable;
            }
            set
            {
                mBaseCameraRotateService.IsHorizontalRotateable = value;
            }
        }

        public bool IsKeyboardMoveable
        {
            get
            {
                return mBaseCameraMoveService.IsKeyboardMoveable;
            }
            set
            {
                mBaseCameraMoveService.IsKeyboardMoveable = value;
            }
        }

        public float MoveSpeed
        {
            get
            {
                return mBaseCameraMoveService.MoveSpeed;
            }
            set
            {
                mBaseCameraMoveService.MoveSpeed = value;
            }
        }

        public float RotateSpeed
        {
            get
            {
                return mBaseCameraRotateService.RotateSpeed;
            }
            set
            {
                mBaseCameraRotateService.RotateSpeed = value;
            }
        }

        public void SetCameraMoveArea(BoxCollider boxCollider)
        {
            mBaseCameraMoveService.SetMoveArea(boxCollider);
            mBaseCameraRotateService.SetMoveArea(boxCollider);
        }

        void OnTouchBegin(EventData eventData)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                if (!eventData.isPointerOnGameObject)
                {
                    mBaseCameraMoveService.CancelMove();
                    mBaseCameraPinchService.CancelPinch();
                    if (!mBaseCameraRotateService.IsCameraAutoRotate && mIsControllable && IsMoveable && !mBaseCameraPinchService.IsPinching)
                    {
                        mBaseCameraMoveService.MoveBegin(eventData);
                    }
                }
            }
        }

        void OnTouch(EventData eventData)
        {
            if (!mBaseCameraRotateService.IsCameraAutoRotate && mIsControllable && IsMoveable && !mBaseCameraPinchService.IsPinching)
            {
                mBaseCameraMoveService.Move(eventData);
            }
        }

        void OnTouchEnd(EventData eventData)
        {
            if (!mBaseCameraRotateService.IsCameraAutoRotate && mIsControllable && IsMoveable && !mBaseCameraPinchService.IsPinching)
            {
                mBaseCameraMoveService.MoveEnd(eventData);
            }
        }

        void OnRotateBegin(EventData eventData)
        {

        }
        // void OnRotate(EventData eventData)
        // {
        //     float angle = Vector3.Angle(eventData.touchPos1 - eventData.touchPos0, new Vector3(1, 0, 0));
        //     if (mBaseCameraRotateService.IsHorizontalRotateable)
        //     {
        //         mBaseCameraRotateService.HorizontalRotate(eventData.deltaAngle * 0.5f);
        //     }
        //     if (mBaseCameraRotateService.IsVerticalRotateable)
        //     {
        //         if (Mathf.Abs(eventData.deltaTouchPos0.x / eventData.deltaTouchPos0.y) < 0.3f && Mathf.Abs(eventData.deltaTouchPos1.x / eventData.deltaTouchPos1.y) < 0.3f)
        //         {
        //             float y = 0;
        //             if (eventData.deltaTouchPos0.y > 0 && eventData.deltaTouchPos1.y > 0)
        //             {
        //                 y = Mathf.Min(eventData.deltaTouchPos0.y, eventData.deltaTouchPos1.y);
        //             }
        //             if (eventData.deltaTouchPos0.y < 0 && eventData.deltaTouchPos1.y < 0)
        //             {
        //                 y = Mathf.Max(eventData.deltaTouchPos0.y, eventData.deltaTouchPos1.y);
        //             }
        //             mBaseCameraRotateService.VerticalRotate(y * 0.5f);
        //         }
        //     }
        // }
        void OnPinchBegin(EventData eventData)
        {
            mBaseCameraMoveService.CancelMove();
            mBaseCameraPinchService.CancelPinch();
            if (IsPinchable && IsControllable)
            {
                mBaseCameraPinchService.OnPinchBegin();
            }
            OnRotateBegin(eventData);
        }

        void OnPinch(EventData eventData)
        {
            if (IsPinchable && IsControllable)
            {
                mBaseCameraPinchService.OnPinch(eventData);
            }
        }

        void OnPinchEnd(EventData eventData)
        {
            if (IsPinchable && IsControllable)
            {
                mBaseCameraPinchService.OnPinchEnd();
            }
        }

        public Camera CurrentCamera
        {
            get
            {
                return mCamera;
            }
        }

        public void SetOrthoSize(float targetOrthograpphicSize)
        {
            mCamera.orthographicSize = targetOrthograpphicSize;
        }

        public void SetEulerAngle(Vector3 angle)
        {
            mCamera.transform.eulerAngles = angle;
        }

        public void DOOrthoSize(float targetOrthographicSize, float zoomDuration)
        {
            mCamera.DOOrthoSize(targetOrthographicSize, zoomDuration);
        }

        public void DOVerticalRotate(Vector3 pos, float angle, float rotateDuration, UnityAction onComplete = null)
        {
            mBaseCameraRotateService.DOVerticalRotate(pos, angle, rotateDuration, onComplete);
        }

        public void DOVerticalRotate(Vector3 pos, Vector3 angle, float rotateDuration, UnityAction onComplete = null)
        {
            mBaseCameraRotateService.DOVerticalRotate(pos, angle, rotateDuration, onComplete);
        }

        public void DOMove(Vector3 offset, float x, float y, float moveDuration, UnityAction onComplete)
        {
            Vector3 forward = mCamera.transform.forward;
            forward = new Vector3(forward.x, 0, forward.z).normalized;
            offset = offset + mCamera.orthographicSize * forward * x + Screen.width / (float)Screen.height * mCamera.orthographicSize * mCamera.transform.right * y;
            Vector3 targetPos = offset + mCamera.transform.position;
            mCamera.transform.DOMove(targetPos, moveDuration).OnComplete(() =>
            {
                if (onComplete != null)
                    onComplete();
            });
        }

        Vector3 GetOffsetPosition(GameObject go, Vector3 forword, float distance)
        {
            forword = new Vector3(forword.x, 0, forword.z).normalized;
            return go.transform.position - forword * distance;
        }

        Vector3 GetObjectOffset(GameObject go)
        {
            Vector3 position = GetCameraForwardPosition(mCamera);
            return go.transform.position - position;
        }

        public bool RaycastForward(Vector3 start, out RaycastHit raycastHit, int layer)
        {
            return Physics.Raycast(start, mCamera.transform.forward, out raycastHit, Mathf.Infinity, 1 << layer);
        }

        public Vector3 GetOrthographicToGridPositionWithOffset(int layer, float x, float y)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(GetOrthographicPositionWithOffset(x, y), mCamera.transform.forward, out raycastHit, Mathf.Infinity, 1 << layer))
            {
                return raycastHit.point;
            }
            return Vector3.zero;
        }

        Vector3 GetOrthographicPositionWithOffset(float x, float y)
        {
            float height = mCamera.orthographicSize * 2;
            float sizePerPixel = mCamera.orthographicSize * 2 / Screen.height;
            return mCamera.transform.position - mCamera.transform.up * y - mCamera.transform.right * x;
        }

        public Vector3 GetCameraForwardPosition()
        {
            return GetCameraForwardPosition(mCamera);
        }

        public static Vector3 GetCameraForwardPosition(Camera currentCamera)
        {
            RaycastHit raycastHit;
            Vector3 pos = Vector3.zero;
            if (Physics.Raycast(currentCamera.transform.position, currentCamera.transform.forward, out raycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
            {
                pos = raycastHit.point;
            }
            return pos;
        }

        public Vector3 ScreenPositionToOrthograhicCameraPosition()
        {
            float height = mCamera.orthographicSize * 2;
            float sizePerPixel = mCamera.orthographicSize * 2 / Screen.height;
            float x = (EasyInput.Instance.GetTouchPosition(0).x - Screen.width / 2) * sizePerPixel;
            float y = (EasyInput.Instance.GetTouchPosition(0).y - Screen.height / 2) * sizePerPixel;
            return mCamera.transform.position + mCamera.transform.up * y + mCamera.transform.right * x;
        }

        public bool GetWorldPositionByMousePosition(out RaycastHit raycastHit, int layer)
        {
            if (mCamera.orthographic)
            {
                return GetWorldTransFromMousePositionByOrthographicCamera(out raycastHit, layer);
            }
            else
            {
                return GetWorldTransFromMousePositionByPerspectiveCamera(out raycastHit, layer);
            }
        }

        bool GetWorldTransFromMousePositionByOrthographicCamera(out RaycastHit raycastHit, int layer)
        {
            Vector3 pos = ScreenPositionToOrthograhicCameraPosition();
            if (Physics.Raycast(pos, mCamera.transform.forward, out raycastHit, Mathf.Infinity, 1 << layer))
            {
                return true;
            }
            return false;
        }

        bool GetWorldTransFromMousePositionByPerspectiveCamera(out RaycastHit raycastHit, int layer)
        {
            Vector3 mousePosition = Input.mousePosition;

            mousePosition.z = 10;

            Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 forward = (position - Camera.main.transform.position).normalized;

            return Physics.Raycast(Camera.main.transform.position, forward, out raycastHit, Mathf.Infinity, 1 << layer);
        }

        public bool RaycastByOrthographicCamera(out RaycastHit raycastHit)
        {
            Vector3 pos = ScreenPositionToOrthograhicCameraPosition();
            if (Physics.Raycast(pos, mCamera.transform.forward, out raycastHit, Mathf.Infinity))
            {
                return true;
            }
            return false;
        }

        public RaycastHit[] RaycastAllByOrthographicCamera(int layer)
        {
            Vector3 pos = ScreenPositionToOrthograhicCameraPosition();
            return Physics.RaycastAll(pos, mCamera.transform.forward, Mathf.Infinity, 1 << layer);
        }

        public RaycastHit[] RaycastAllByOrthographicCameraWithLayers(int layers)
        {
            Vector3 pos = ScreenPositionToOrthograhicCameraPosition();
            return Physics.RaycastAll(pos, mCamera.transform.forward, Mathf.Infinity, layers);
        }

        public Vector3 GetCameraForward()
        {
            return new Vector3(mCamera.transform.forward.x, 0, mCamera.transform.forward.z).normalized;
        }

        public Vector3 GetCameraRight()
        {
            return new Vector3(mCamera.transform.right.x, 0, mCamera.transform.right.z).normalized;
        }

        public static Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
        {
            float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct, planeNormal);
            return d * direct.normalized + point;
        }

        void Update()
        {
            if (IsMoveable)
                mBaseCameraMoveService.OnUpdate();
            mBaseCameraPinchService.OnUpdate();
            mBaseCameraRotateService.OnUpdate();
        }

        void LateUpdate()
        {
            if (IsMoveable)
                mBaseCameraMoveService.OnLateUpdate();
            mBaseCameraPinchService.OnLateUpdate();
            mBaseCameraRotateService.OnLateUpdate();
        }

        void OnDrawGizmos()
        {
            if (mBaseCameraMoveService != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(mBaseCameraMoveService.pos0, 0.1f);
                Gizmos.DrawSphere(mBaseCameraMoveService.pos1, 0.1f);
                Gizmos.DrawSphere(mBaseCameraMoveService.pos2, 0.1f);
                Gizmos.DrawSphere(mBaseCameraMoveService.pos3, 0.1f);
                Gizmos.color = Color.white;
            }
        }
    }
}