using BlueNoah.Event;
using BlueNoah.SceneControl;
using UnityEngine;

namespace RTS
{
    public class RTSPlayerController 
    {
        ScreenSelectService mScreenSelectService;

        public int playerId = 1;

        public RTSPlayerController()
        {
            mScreenSelectService = new ScreenSelectService();

            EasyInput.Instance.AddListener(BlueNoah.Event.TouchType.Click,OnClick);
        }

        public void OnGUI()
        {
            mScreenSelectService.OnGUI();
        }

        void OnClick(EventData eventData)
        {
            RaycastHit raycastHit;
            if (BlueNoah.CameraControl.CameraController.Instance.GetWorldTransFromMousePosition(out raycastHit,LayerConstant.LAYER_GROUND))
            {
                RTSSceneController.Instance.SpawnActor(playerId, 1,raycastHit.point.ToFixedPointVector3());
            }
        }

        public void OnUpdate()
        {
         
        }
    }
}