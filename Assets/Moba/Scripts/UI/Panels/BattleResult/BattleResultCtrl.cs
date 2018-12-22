using System.Collections;
using UIFrame;

namespace BlueNoah.UI
{
    public class BattleResultCtrl : UIFrame.BaseCtrl
    {

        BattleResultPanelView mBattleResultPanelView;

        bool mIsInit;

        public override void ShowPanel(Hashtable parameters)
        {
            base.ShowPanel(parameters);
            bool isCreate;
            mBattleResultPanelView = UIMgr.ShowPanel<BattleResultPanelView>(UIManager.UILayerType.Common, out isCreate);
            mBattleResultPanelView.root.SetActive(true);
            if(!mIsInit){
                mIsInit = true;
                mBattleResultPanelView.btnRestart.onClick.AddListener(ServerController_III.instance.StopHost);
            }
        }

		public void Win()
        {
            ShowPanel(null);
            mBattleResultPanelView.containerWin.gameObject.SetActive(true);
            mBattleResultPanelView.containerLost.gameObject.SetActive(false);
        }

        public void Fail()
        {
            ShowPanel(null);
            mBattleResultPanelView.containerWin.gameObject.SetActive(false);
            mBattleResultPanelView.containerLost.gameObject.SetActive(true);
        }
    }
}
