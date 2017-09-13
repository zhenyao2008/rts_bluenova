//==========================================
// Created By yingyugang At 1/29/2016 11:02:39 AM
//==========================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MsgPanel
{
    ///<summary>
    ///
    ///</summary>
    public class MsgPanelView : PanelBase
    {
        ///////////////////////////////////以下为静态成员//////////////////////////////

        ///////////////////////////////////以下为非静态成员///////////////////////////
        public Transform m_Trans;
        public Text m_txtTitle;
        public Button m_btnConfirm;
        public Text m_txtMsg;


        // Use this for initialization
        public override void Awake()
        {
            m_Trans = transform;
            m_txtTitle = m_Trans.FindChild("#txt_title").GetComponent<Text>();
            m_btnConfirm = m_Trans.FindChild("#btn_confirm").GetComponent<Button>();
            m_txtMsg = m_Trans.FindChild("#txt_msg").GetComponent<Text>();

        }

        // Update is called once per frame
        void Update()
        {

        }

		void OnDestroy()
		{
            m_txtTitle = null;
            m_btnConfirm = null;
            m_txtMsg = null;

		}
    }
}
