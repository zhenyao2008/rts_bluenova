using BlueNoah.Event;
using UnityEngine;

namespace BlueNoah.CameraControl
{
    [System.Serializable]
    public class OrthographicCameraMoveService : BaseCameraMoveService
    {

        public OrthographicCameraMoveService(Camera camera)
        {
            mCamera = camera;
        }

		protected override Vector3 GetMoveAreOffset(Vector3 targetPos)
		{
            Vector3 offset = Vector3.zero;
            if (mMoveArea != null)
            {
#if UNITY_EDITOR
                //デッバグ用ソースコード。
                RaycastHit raycastHit;
                if (CameraController.Instance.RaycastForward(CameraLeftTop(targetPos), out raycastHit, LayerConstant.LAYER_GROUND))
                {
                    pos0 = raycastHit.point;
                }
                if (CameraController.Instance.RaycastForward(CameraLeftBottom(targetPos), out raycastHit, LayerConstant.LAYER_GROUND))
                {
                    pos1 = raycastHit.point;
                }
                if (CameraController.Instance.RaycastForward(CameraRightTop(targetPos), out raycastHit, LayerConstant.LAYER_GROUND))
                {
                    pos2 = raycastHit.point;
                }
                if (CameraController.Instance.RaycastForward(CameraRightBottom(targetPos), out raycastHit, LayerConstant.LAYER_GROUND))
                {
                    pos3 = raycastHit.point;
                }
#endif
                Vector3 offset0 = GetOffset(CameraLeftTop(targetPos));
                Vector3 offset1 = GetOffset(CameraLeftBottom(targetPos));
                Vector3 offset2 = GetOffset(CameraRightTop(targetPos));
                Vector3 offset3 = GetOffset(CameraRightBottom(targetPos));
                if (offset0.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset0;
                }
                if (offset1.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset1;
                }
                if (offset2.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset2;
                }
                if (offset3.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset3;
                }
            }
            return offset;
		}

        Vector3 GetOffset(Vector3 startPos)
        {
            Vector3 offset = Vector3.zero;
            RaycastHit raycastHit;
            if (CameraController.Instance.RaycastForward(startPos, out raycastHit, LayerConstant.LAYER_GROUND))
            {
                Vector3 pos = raycastHit.point;
                Vector3 closePos = mMoveArea.ClosestPoint(pos);
                offset = pos - closePos;
            }
            return offset;
        }
    }
}