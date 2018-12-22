using UnityEngine.UI;

namespace BlueNoah.UI
{
    public class BattleResultPanelView : PanelBase
    {

        public Button btnWin;

        public Button btnFail;

        public override void Awake()
        {
            base.Awake();

            btnWin = transform.Find("Root/btn_win").GetComponent<Button>();

            btnFail = transform.Find("Root/btn_fail").GetComponent<Button>();

            btnWin.onClick.AddListener(ServerController_III.instance.StopHost);

            btnFail.onClick.AddListener(ServerController_III.instance.StopHost);
        }

    }
}
