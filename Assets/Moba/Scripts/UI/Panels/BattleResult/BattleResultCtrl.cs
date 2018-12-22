namespace BlueNoah.UI
{
    public class BattleResultCtrl : BaseCtrl
    {

        BattleResultPanelView mBattleResultPanelView;


        public void Win()
        {
            mBattleResultPanelView.btnWin.gameObject.SetActive(true);
            mBattleResultPanelView.btnFail.gameObject.SetActive(false);
        }

        public void Fail()
        {
            mBattleResultPanelView.btnWin.gameObject.SetActive(false);
            mBattleResultPanelView.btnFail.gameObject.SetActive(true);
        }
    }
}
