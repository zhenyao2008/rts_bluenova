using UnityEngine;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UI.MainInterfacePanel;


public class Test : MonoBehaviour {

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
            Debug.Log(typeof(MainInterfacePanelView).ToString());
            //Framework.UIManager.Instance().TogglePanel(UIManager.UILayerType.Bottom, "MainInterfacePanel");

            Framework.UIManager.Instance().TogglePanel<MainInterfacePanelView>(UIManager.UILayerType.Bottom);

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Framework.UIManager.Instance().TogglePanel(UIManager.UILayerType.Mask, "MaskPanel");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Framework.UIManager.Instance().TogglePanel(UIManager.UILayerType.Common, "SettingPanel");
        }



    }






}
