using UnityEngine;
using System.Collections;
using UI.TeamListPanel;
using Framework;

public class TeamListCtrl : BaseCtrl {

    TeamListPanelView mTeamListPanelView;

    public override void ShowPanel()
    {
        bool isCreate;
        mTeamListPanelView = UIMgr.ShowPanel<TeamListPanelView>(UIManager.UILayerType.Common, out isCreate);
        if (isCreate)
        {
            OnCreatePanel();
        }
        UIMgr.maskCtrl.ShowPanel(new UnityEngine.Events.UnityAction(Close));
    }

    void OnCreatePanel()
    {
        mTeamListPanelView.m_btnClose.onClick.AddListener(Close);
        mTeamListPanelView.m_btnClose.onClick.AddListener(CloseMask);
    }

    public override void Close()
    {
        UIMgr.ClosePanel("TeamListPanel");
    }

    public void CloseMask()
    {
        UIMgr.maskCtrl.Close();
    }
}
