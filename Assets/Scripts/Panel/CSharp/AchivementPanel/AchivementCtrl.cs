using UnityEngine;
using System.Collections;
using UI.SettingPanel;
using Framework;
using UI.AchivementPanel;

public class AchivementCtrl : BaseCtrl
{

    AchivementPanelView mAchivementPanelView;

    public override void ShowPanel()
    {
        bool isCreate;
        mAchivementPanelView = UIMgr.ShowPanel<AchivementPanelView>(UIManager.UILayerType.Common, out isCreate);
        if (isCreate)
        {
            OnCreatePanel();
        }
        UIMgr.maskCtrl.ShowPanel(new UnityEngine.Events.UnityAction(Close));
    }

    void OnCreatePanel()
    {
        mAchivementPanelView.m_btnClose.onClick.AddListener(Close);
        mAchivementPanelView.m_btnClose.onClick.AddListener(CloseMask);
    }

    public override void Close()
    {
        UIMgr.ClosePanel("AchivementPanel");
    }

    public void CloseMask()
    {
        UIMgr.maskCtrl.Close();
    }

}
