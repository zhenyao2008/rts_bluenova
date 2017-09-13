using UnityEngine;
using System.Collections;
using UI.ShopPanel;
using Framework;

public class ShopCtrl : BaseCtrl
{

    ShopPanelView mShopPanelView;

    public override void ShowPanel()
    {
        bool isCreate;
        mShopPanelView = UIMgr.ShowPanel<ShopPanelView>(UIManager.UILayerType.Common, out isCreate);
        if (isCreate)
        {
            OnCreatePanel();
        }
        UIMgr.maskCtrl.ShowPanel(new UnityEngine.Events.UnityAction(Close));
    }

    void OnCreatePanel()
    {
        
    }

    public override void Close()
    {
        UIMgr.ClosePanel("ShopPanel");
    }

    public void CloseMask()
    {
        UIMgr.maskCtrl.Close();
    }
}
