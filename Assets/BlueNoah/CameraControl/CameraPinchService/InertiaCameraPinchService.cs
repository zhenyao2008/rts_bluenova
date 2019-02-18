using BlueNoah.Event;
using UnityEngine;

namespace BlueNoah.CameraControl
{
    //TODO まだ完成していない
    //public class InertiaCameraPinchService : BaseCameraPinchService
    //{
    //    float mRemainOrthographicSize;
    //    //慣性ために
    //    float mMaxSpeed = 20f;
    //    float mMinSpeed = 5f;
    //    float mDeltaSize = 0;
    //    float mT;
    //    float mSpeedDownSize = 2f;
    //    float mSmooth = 0.999f;
    //    float mLastTargetSize;
    //    float mLastStartSize;

    //    public InertiaCameraPinchService(Camera camera)
    //    {
    //        this.mCamera = camera;
    //    }

    //    public override void OnUpdate()
    //    {
    //        mIsPinching = false;
    //        if (Input.GetKey(KeyCode.A))
    //        {
    //            mIsPinching = true;
    //            mRemainOrthographicSize += 0.001f;
    //        }
    //        if (Input.GetKey(KeyCode.D))
    //        {
    //            mIsPinching = true;
    //            mRemainOrthographicSize += -0.001f;
    //        }
    //    }

    //    public override void OnLateUpdate()
    //    {

    //    }

    //    public override void OnPinch(EventData eventData)
    //    {
    //    }

    //    public override void OnPinchBegin()
    //    {
    //    }

    //    void SmoothPinch()
    //    {
    //        mDeltaSize = 0;
    //        if (mRemainOrthographicSize > 0)
    //        {
    //            if (!mIsPinching && mRemainOrthographicSize < mSpeedDownSize)
    //            {
    //                mT += Time.deltaTime / 0.5f;
    //                mT = Mathf.Clamp(mT, 0, 1);
    //                float lastTargetSize = Mathf.Lerp(mLastTargetSize, 0, Mathf.Cos(mT * Mathf.PI / 2));
    //                mDeltaSize = lastTargetSize * Time.deltaTime;
    //            }
    //            else
    //            {
    //                mDeltaSize = Mathf.Min(mRemainOrthographicSize, mMaxSpeed * Time.deltaTime);
    //                mLastTargetSize = mRemainOrthographicSize;
    //                mT = 0;
    //            }
    //        }
    //        else if (mRemainOrthographicSize < 0)
    //        {
    //            if (!mIsPinching && mRemainOrthographicSize > -mSpeedDownSize)
    //            {
    //                mT += Time.deltaTime / 0.5f;
    //                mT = Mathf.Clamp(mT, 0, 1);
    //                float lastTargetSize = Mathf.Lerp(mLastTargetSize, 0, Mathf.Cos(mT * Mathf.PI / 2));
    //                mDeltaSize = lastTargetSize * Time.deltaTime;
    //            }
    //            else
    //            {
    //                mDeltaSize = Mathf.Max(mRemainOrthographicSize, -mMaxSpeed * Time.deltaTime);
    //                mLastTargetSize = mRemainOrthographicSize;
    //                mT = 0;
    //            }
    //        }
    //        mRemainOrthographicSize -= mDeltaSize;
    //        mCamera.orthographicSize -= mDeltaSize;
    //        mCamera.orthographicSize = Mathf.Clamp(mCamera.orthographicSize, mMinOrthographicSize, mMaxOrthographicSize);
    //    }
    //}
}
