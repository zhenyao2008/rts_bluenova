using BlueNoah.Event;
using UnityEngine;

namespace BlueNoah.RPG
{
    public class RPGPlayerController  {

        public int playerId = 1;

        GameObject mSelectActor;

        public RPGPlayerController()
        {

            EasyInput.Instance.AddListener(Event.TouchType.Click, 0, OnClick);

            CameraControl.CameraController.Instance.MoveSpeed = TD.Config.InGameConfig.Single.cameraDragSpeed;
        }

        void OnClick(EventData eventData)
        {
            Debug.Log("OnClick");
            RaycastHit raycastHit;
            if (CameraControl.CameraController.Instance.GetWorldPositionByMousePosition(out raycastHit, LayerConstant.playerLayer0))
            {
                OnSelectActor(raycastHit.collider.gameObject);
            }
            else
            {

            }
        }

        void OnSelectActor(GameObject actor)
        {
            mSelectActor = actor;
        }

        void UnSelectActor()
        {
            mSelectActor = null;
        }

        public void OnUpdate()
        {
           
        }
    }
}