using UnityEngine;
using BlueNoah.Event;

namespace BlueNoah.CameraControl
{
    [System.Serializable]
    public class OrthographicsCameraPinchService : BaseCameraPinchService
    {

        private float mMinOrthographicSize = 2f;
        private float mMaxOrthographicSize = 5f;
        private float mTargetOrthographicSize;

        public override float minSize
        {
            get
            {
                return mMinOrthographicSize;
            }

            set
            {
                mMinOrthographicSize = value;
            }
        }

        public override float maxSize
        {
            get
            {
                return mMaxOrthographicSize;
            }

            set
            {
                mMaxOrthographicSize = value;
            }
        }

        public OrthographicsCameraPinchService(Camera camera)
        {
            mCamera = camera;
        }

		public override void Init()
		{
            
		}

		public override void OnPinchBegin()
        {
            mTargetOrthographicSize = mCamera.orthographicSize;
        }

        public override void OnPinch(EventData eventData)
        {
            float detalDistance = eventData.deltaPinchDistance;
            mTargetOrthographicSize -= detalDistance * mPinchRadiu;
            mTargetOrthographicSize = Mathf.Clamp(mTargetOrthographicSize, mMinOrthographicSize, mMaxOrthographicSize);
            mCamera.orthographicSize = mTargetOrthographicSize;
        }

        protected override void OnMouseScrollWheel()
        {
            mTargetOrthographicSize = mCamera.orthographicSize;
            EventData eventData = new EventData();
            eventData.deltaPinchDistance = Input.GetAxis("Mouse ScrollWheel") * 100f;
            OnPinch(eventData);
        }
    }
}

