using UnityEngine.UI;

namespace BlueNoah.RTS.UI
{
    public class UIManager : SingleMonoBehaviour<UIManager>
    {

        public Button btnBuilding;

        protected override void Awake()
        {
            base.Awake();
            btnBuilding.onClick.AddListener(()=>
            {


            });
        }

    }
}
