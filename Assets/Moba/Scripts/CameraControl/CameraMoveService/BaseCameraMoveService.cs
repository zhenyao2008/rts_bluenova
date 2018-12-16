using UnityEngine;
using BlueNoah.Event;

namespace BlueNoah.CameraControl
{
    //Remain Distanceを使うと、操作の紐つりが少しだけなくなる。（Target Position を使わない）
    [System.Serializable]
    public abstract class BaseCameraMoveService
    {

        protected Camera mCamera;
        protected float mMoveSpeed = 2f;
        protected float mSmooth = 20f;
        protected float mSmoothDistance = 0.5f;
        protected Vector3 mRemainRightDistance;
        protected Vector3 mRemainForwardDistance;
        protected BoxCollider mMoveArea;
        protected bool mIsTouching;
        protected bool mKeyboardMoveable = true;
        protected float mKeyboardSpeed = 1000;

        public void CancelMove()
        {
            mIsTouching = false;
        }

        public bool IsKeyboardMoveable
        {
            get
            {
                return mKeyboardMoveable;
            }
            set
            {
                mKeyboardMoveable = value;
            }
        }

        public void MoveBegin(EventData eventData)
        {
            Debug.Log("MoveBegin");
            mIsTouching = true;
        }

        public void ClearRemainDistance()
        {
            mRemainRightDistance = Vector3.zero;
            mRemainForwardDistance = Vector3.zero;
        }

        public void SetMoveArea(BoxCollider boxCollider)
        {
            this.mMoveArea = boxCollider;
        }

        public void MoveEnd(EventData eventData)
        {
            mIsTouching = false;
        }

        public virtual void Init(){
            
        }

        public void OnUpdate()
        {

        }

        public void OnLateUpdate()
        {
            if (mKeyboardMoveable)
                KeyboardMove();

            Vector3 detalForwardMove = mRemainForwardDistance * Time.deltaTime * mSmooth;

            Vector3 detalRightMove = mRemainRightDistance * Time.deltaTime * mSmooth;

            Vector3 targetPos = mCamera.transform.position + detalForwardMove + detalRightMove;

            if (mRemainForwardDistance.magnitude < detalForwardMove.magnitude)
            {
                mRemainForwardDistance = Vector3.zero;
            }
            else
            {
                mRemainForwardDistance -= detalForwardMove;
            }

            if (mRemainRightDistance.magnitude < detalRightMove.magnitude)
            {
                mRemainRightDistance = Vector3.zero;
            }
            else
            {
                mRemainRightDistance -= detalRightMove;
            }

            mCamera.transform.position = targetPos;

            //Do move back
            if (!mIsTouching)
                MoveBack();
        }

        void MoveBack()
        {
            Vector3 offset = GetMoveAreOffset(mCamera.transform.position);
            mCamera.transform.position -= offset * Time.deltaTime * mSmooth / 2f;
        }

        public void Move(EventData eventData)
        {
            if (mIsTouching)
            {
                Move(eventData.deltaTouchPos0.x, eventData.deltaTouchPos0.y);
            }
        }

        void Move(float x, float y)
        {
            Vector3 forward = GetCameraForward();

            Vector3 right = GetCameraRight();

            float angle = Vector3.Angle(mCamera.transform.forward, new Vector3(0, -1, 0));

            Vector3 offset = GetMoveAreOffset(mCamera.transform.position + mRemainForwardDistance + mRemainRightDistance);

            bool isSameForwardDirect = false;

            bool isSameRightDirect = false;

            if ((Vector3.Dot(Vector3.Project(offset, forward).normalized, forward) > 0 && y < 0) || (Vector3.Dot(Vector3.Project(offset, forward).normalized, forward) < 0 && y > 0))
            {
                isSameForwardDirect = true;
            }

            if ((Vector3.Dot(Vector3.Project(offset, right).normalized, right) > 0 && x < 0) || (Vector3.Dot(Vector3.Project(offset, right).normalized, right) < 0 && x > 0))
            {
                isSameRightDirect = true;
            }

            float forwardFloat = 0;

            if (isSameForwardDirect)
            {
                forwardFloat = Vector3.Project(offset, forward).magnitude;
            }

            float rightFloat = 0;

            if (isSameRightDirect)
            {
                rightFloat = Vector3.Project(offset, right).magnitude;
            }

            float forwardRadiu = Mathf.Cos(angle / 180f * Mathf.PI);

            mRemainForwardDistance -= forward * y * Mathf.Max(0, Mathf.Cos((mSmoothDistance / forwardRadiu - forwardFloat) / 2f * Mathf.PI / (mSmoothDistance * 2 / forwardRadiu))) * mCamera.orthographicSize / Screen.height * mMoveSpeed / forwardRadiu;
            //mRemainForwardDistance -= forward * y * Mathf.Max(0,Mathf.Cos((mSmoothDistance / forwardRadiu - forwardFloat) / 2f * Mathf.PI / (mSmoothDistance * 2 / forwardRadiu) )) * DistancePerPixel * CameraSpeed / forwardRadiu;
            //mRemainForwardDistance -= forward * y * Mathf.Max(0, Mathf.Cos((mSmoothDistance / forwardRadiu - forwardFloat) / 2f * Mathf.PI / (mSmoothDistance * 2 / forwardRadiu))) * mCamera.orthographicSize / Screen.height * mMoveSpeed / forwardRadiu;
            mRemainRightDistance -= right * x * Mathf.Max(0, Mathf.Cos((mSmoothDistance - rightFloat) / 2f * Mathf.PI / (mSmoothDistance * 2))) * mCamera.orthographicSize / Screen.height * mMoveSpeed;

        }

        void KeyboardMove()
        {
            //orthographicSizeと角度に基ついて移す速さを計算する、そして、どんなorthographicSizeでも、移動スピードがほぼ同じ。
            float x = 0;

            float y = 0;

            if (Input.GetKey(KeyCode.A))
            {
                x = mKeyboardSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                x = -mKeyboardSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                y = -mKeyboardSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                y = mKeyboardSpeed * Time.deltaTime;
            }
            Move(x, y);
        }

        protected Vector3 GetCameraForward()
        {
            return new Vector3(mCamera.transform.forward.x, 0, mCamera.transform.forward.z).normalized;
        }

        protected Vector3 GetCameraRight()
        {
            return new Vector3(mCamera.transform.right.x, 0, mCamera.transform.right.z).normalized;
        }

        protected Vector3 CameraLeftTop(Vector3 pos)
        {
            float width = (float)Screen.width / Screen.height * mCamera.orthographicSize;
            pos = pos - mCamera.transform.right * width + mCamera.transform.up * mCamera.orthographicSize;
            return pos;
        }

        protected Vector3 CameraLeftBottom(Vector3 pos)
        {
            float width = (float)Screen.width / Screen.height * mCamera.orthographicSize;
            pos = pos - mCamera.transform.right * width - mCamera.transform.up * mCamera.orthographicSize;
            return pos;
        }

        protected Vector3 CameraRightTop(Vector3 pos)
        {
            float width = (float)Screen.width / Screen.height * mCamera.orthographicSize;
            pos = pos + mCamera.transform.right * width + mCamera.transform.up * mCamera.orthographicSize;
            return pos;
        }

        protected Vector3 CameraRightBottom(Vector3 pos)
        {
            float width = (float)Screen.width / Screen.height * mCamera.orthographicSize;
            pos = pos + mCamera.transform.right * width - mCamera.transform.up * mCamera.orthographicSize;
            return pos;
        }

        protected abstract Vector3 GetMoveAreOffset(Vector3 targetPos);

        public Vector3 pos0;
        public Vector3 pos1;
        public Vector3 pos2;
        public Vector3 pos3;
    }
}