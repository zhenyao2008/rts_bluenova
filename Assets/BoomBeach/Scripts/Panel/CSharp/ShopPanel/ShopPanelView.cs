//==========================================
// Created By yingyugang At 1/29/2016 2:51:00 PM
//==========================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShopPanel
{
    ///<summary>
    ///
    ///</summary>
    public class ShopPanelView : PanelBase
    {
        ///////////////////////////////////以下为静态成员//////////////////////////////

        ///////////////////////////////////以下为非静态成员///////////////////////////
        public Transform m_Trans;
        public Button m_btnDefence;
        public Button m_btnSurport;
        public Button m_btnResource;
        public GameObject m_tabCardtypes;
        public Transform m_gridCarditems;
        public GameObject m_scrollCarditems;


        // Use this for initialization
        public override void Awake()
        {
            m_Trans = transform;
            m_btnDefence = m_Trans.FindChild("#tab_cardtypes/#btn_defence").GetComponent<Button>();
            m_btnSurport = m_Trans.FindChild("#tab_cardtypes/#btn_surport").GetComponent<Button>();
            m_btnResource = m_Trans.FindChild("#tab_cardtypes/#btn_resource").GetComponent<Button>();
            m_tabCardtypes = m_Trans.FindChild("#tab_cardtypes").gameObject;
            m_gridCarditems = m_Trans.FindChild("#scroll_carditems/#grid_carditems");
            m_scrollCarditems = m_Trans.FindChild("#scroll_carditems").gameObject;

        }

        // Update is called once per frame
        void Update()
        {

        }

		void OnDestroy()
		{
            m_btnDefence = null;
            m_btnSurport = null;
            m_btnResource = null;
            m_tabCardtypes = null;
            m_gridCarditems = null;
            m_scrollCarditems = null;

		}
    }
}
