using UnityEngine;
using System.Collections;
using Framework;
using UI.MainInterfacePanel;
using UI.SettingPanel;
using UI.MaskPanel;
using UI.PlayerListPanel;
using UI.ShopPanel;

public class MainInterfaceCtrl : BaseCtrl
{

    MainInterfacePanelView mMainInterfacePanelView;

    public override void ShowPanel()
    {
        bool isCreate;
        mMainInterfacePanelView = UIManager.Instance().ShowPanel<MainInterfacePanelView>(UIManager.UILayerType.Fixed,out isCreate);
        if (isCreate)
        {
            OnCreatePanel();
        }
    }

    void OnCreatePanel()
    {
        mMainInterfacePanelView.m_btnSetting.onClick.AddListener(ShowSettingPanel);
        mMainInterfacePanelView.m_btnFriend.onClick.AddListener(ShowPlayerListPanel);
        mMainInterfacePanelView.m_btnShop.onClick.AddListener(ShowShopPanel);
        mMainInterfacePanelView.m_btnTeam.onClick.AddListener(ShowTeamListPanel);
        mMainInterfacePanelView.m_btnAchivement.onClick.AddListener(ShowAchivementPanel);
    }

    void ShowSettingPanel()
    {
        UIMgr.settingCtrl.ShowPanel();
    }

    void ShowPlayerListPanel()
    {
        UIMgr.playerListCtrl.ShowPanel();
    }

    void ShowTeamListPanel()
    {
        UIMgr.teamListCtrl.ShowPanel();
    }

    void ShowShopPanel()
    {
        UIMgr.shopCtrl.ShowPanel();
    }

    void ShowAchivementPanel()
    {
        UIMgr.achivementCtrl.ShowPanel();
    }

}
