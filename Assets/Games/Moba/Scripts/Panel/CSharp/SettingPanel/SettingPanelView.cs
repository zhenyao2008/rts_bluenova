//==========================================
// Created By yingyugang At 1/29/2016 12:15:47 PM
//==========================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingPanel
{
    ///<summary>
    ///
    ///</summary>
    public class SettingPanelView : PanelBase
    {
        ///////////////////////////////////以下为静态成员//////////////////////////////

        ///////////////////////////////////以下为非静态成员///////////////////////////
        public Transform m_Trans;
        public Button m_btnClose;
        public Text m_txtMusic;
        public Button m_btnMusic;
        public Text m_txtSound;
        public Button m_btnSound;
        public Button m_btnLanguage;
        public Button m_btnService;
        public Button m_btnAccount;
        public Button m_btnProductor;
        public Button m_btnHelp;


        // Use this for initialization
        public override void Awake()
        {
            m_Trans = transform;
            m_btnClose = m_Trans.Find("#btn_close").GetComponent<Button>();
            m_txtMusic = m_Trans.Find("#btn_music/#txt_music").GetComponent<Text>();
            m_btnMusic = m_Trans.Find("#btn_music").GetComponent<Button>();
            m_txtSound = m_Trans.Find("#btn_sound/#txt_sound").GetComponent<Text>();
            m_btnSound = m_Trans.Find("#btn_sound").GetComponent<Button>();
            m_btnLanguage = m_Trans.Find("#btn_language").GetComponent<Button>();
            m_btnService = m_Trans.Find("#btn_service").GetComponent<Button>();
            m_btnAccount = m_Trans.Find("#btn_account").GetComponent<Button>();
            m_btnProductor = m_Trans.Find("#btn_productor").GetComponent<Button>();
            m_btnHelp = m_Trans.Find("#btn_help").GetComponent<Button>();

        }

        // Update is called once per frame
        void Update()
        {

        }

		void OnDestroy()
		{
            m_btnClose = null;
            m_txtMusic = null;
            m_btnMusic = null;
            m_txtSound = null;
            m_btnSound = null;
            m_btnLanguage = null;
            m_btnService = null;
            m_btnAccount = null;
            m_btnProductor = null;
            m_btnHelp = null;

		}
    }
}
