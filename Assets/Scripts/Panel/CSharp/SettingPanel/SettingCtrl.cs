using UnityEngine;
using System.Collections;
using UI.SettingPanel;
using Framework;

public class SettingCtrl : BaseCtrl
{

    SettingPanelView mSettingPanelView;

    public override void ShowPanel()
    {
        bool isCreate;
        mSettingPanelView = UIMgr.ShowPanel<SettingPanelView>(UIManager.UILayerType.Common, out isCreate);
        if (isCreate)
        {
            OnCreatePanel();
        }
        UIMgr.maskCtrl.ShowPanel(new UnityEngine.Events.UnityAction(Close));
    }

    void OnCreatePanel()
    {
        mSettingPanelView.m_btnClose.onClick.AddListener(Close);
        mSettingPanelView.m_btnClose.onClick.AddListener(CloseMask);
    }

    public override void Close()
    {
        UIMgr.ClosePanel("SettingPanel");
    }

    public void CloseMask()
    {
        UIMgr.maskCtrl.Close();
    }

}
