using UnityEngine;
using UnityEngine.UI;

namespace BlueNoah.UI
{
    public class BattleResultPanelView : UIFrame.PanelBase
    {

        public GameObject containerWin;

        public GameObject containerLost;

        public Button btnRestart;

        public override void Awake()
        {
            base.Awake();

            containerWin = transform.Find("Root/container_win").gameObject;

            containerLost = transform.Find("Root/container_lost").gameObject;

            btnRestart = transform.Find("Root/btn_restart").GetComponent<Button>();
        }

    }
}
