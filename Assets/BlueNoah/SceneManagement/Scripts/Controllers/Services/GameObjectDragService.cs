using BlueNoah.CameraControl;
using BlueNoah.Event;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.Control.Service
{
    //位置など関するもの
    public class GameObjectDragService
    {
        bool mDraging;

        GameObject mDragGameObject;

        Vector3 mDragOffset;

        public Vector3 commonOffset;

        public bool isTargetEulerAngle;

        public Vector3 targetEulerAngle;

        public int groundLayer;

        public int unitLayer;

        public UnityAction<GameObject> onTouchBegin;

        public UnityAction<GameObject> onDragBegin;

        public UnityAction<GameObject> onDrag;

        public UnityAction<GameObject> onDragEnd;

        public void Init()
        {
            EasyInput.Instance.AddListener(Event.TouchType.TouchBegin, 0, OnTouchBegin);
            EasyInput.Instance.AddListener(Event.TouchType.LongPressBegin, 0, DragChecking);
            EasyInput.Instance.AddListener(Event.TouchType.Touch, 0, OnDrag);
            EasyInput.Instance.AddListener(Event.TouchType.TouchEnd, 0, OnDragEnd);
        }

        //キャラをストップなど
        void OnTouchBegin(EventData eventData)
        {
            RaycastHit[] raycastHits = BlueNoah.CameraControl.CameraController.Instance.RaycastAllByOrthographicCamera(unitLayer);
            if (raycastHits.Length > 0)
            {
                GameObject go = raycastHits[0].transform.gameObject;
                if (onTouchBegin != null)
                    onTouchBegin(go);
            }
        }

        public void DragChecking(EventData eventData)
        {
            RaycastHit[] raycastHits = BlueNoah.CameraControl.CameraController.Instance.RaycastAllByOrthographicCamera(unitLayer);
            if (raycastHits.Length > 0)
            {
                GameObject go = raycastHits[0].transform.gameObject;
                OnDragBegin(go);
            }
        }

        void OnDragBegin(GameObject dragGameObject)
        {
            mDraging = true;

            mDragGameObject = dragGameObject;

            mDragOffset = Vector3.zero;

            if (isTargetEulerAngle)
            {
                mDragGameObject.transform.eulerAngles = targetEulerAngle;
            }

            if (onDragBegin != null)
            {
                onDragBegin(dragGameObject);
            }
        }

        void OnDrag(EventData eventData)
        {
            if (mDraging)
            {
                Vector3 pinchPosition = GetDragPosition();

                Vector3 targetPos = pinchPosition - mDragOffset + commonOffset;

                mDragGameObject.transform.position = targetPos;

                if (onDrag != null)
                {
                    onDrag(mDragGameObject);
                }
            }
        }

        void OnDragEnd(EventData eventData)
        {
            if (mDraging)
            {
                mDraging = false;

                if (onDragEnd != null)
                {
                    onDragEnd(mDragGameObject);
                }
            }
        }

        Vector3 GetDragPosition()
        {
            RaycastHit raycastHit;

            if (BlueNoah.CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, groundLayer))
            {
                return raycastHit.point;
            }

            return Vector3.zero;
        }
    }
}