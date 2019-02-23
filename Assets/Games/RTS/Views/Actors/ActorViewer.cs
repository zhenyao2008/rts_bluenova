/*
 *　2019.2.23 午７時４分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using BlueNoah.AI.RTS;
using UnityEngine;

namespace BlueNoah.AI.View.RTS
{
    //main class for actor view. 
    //don't control the core class.
    public class ActorViewer : MonoBehaviour
    {
        public ActorCore actorCore;
        public ActorAnimation actorAnimation;
        public ActorAsset actorAsset;
        public ActorHeadUI actorHeadUI;
        public Vector3 screenPosition;

        void Awake()
        {
            actorAnimation = gameObject.GetOrAddComponent<ActorAnimation>();

            actorAsset = gameObject.GetOrAddComponent<ActorAsset>();
        }
        public void Init(ActorCore actorCore)
        {
#if UNITY_EDITOR
            if (actorCore == null)
            {
                Debug.LogError("UnitCore is Null.");
            }
#endif
            this.actorCore = actorCore;
            UpdateTransform();
        }
        void Update()
        {
            UpdateTransform();
        }

       public void UpdateTransform()
        {
            if (actorCore != null)
            {
                transform.position = actorCore.transform.position.ToVector3();
                transform.eulerAngles = actorCore.transform.eulerAngles.ToVector3();
            }
        }
        //Selection condition.
        void LateUpdate()
        {
            if (actorCore != null)
            {
                screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            }
        }
        public bool IsInScreen()
        {
            return screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < 1 && screenPosition.y > 0 && screenPosition.y < 1;
        }
    }
}
