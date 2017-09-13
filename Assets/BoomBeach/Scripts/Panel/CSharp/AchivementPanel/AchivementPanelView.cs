//==========================================
// Created By yingyugang At 1/29/2016 8:58:41 PM
//==========================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.AchivementPanel
{
    ///<summary>
    ///
    ///</summary>
    public class AchivementPanelView : PanelBase
    {
        ///////////////////////////////////以下为静态成员//////////////////////////////

        ///////////////////////////////////以下为非静态成员///////////////////////////
        public Transform m_Trans;
        public Button m_btnClose;
        public Transform m_gridCarditems;
        public GameObject m_scrollCarditems;
        public Button m_btnPlayers;
        public Transform m_gridPlayers;
        public GameObject m_containerPlayers;
        public Button m_btnFriends;


        // Use this for initialization
        public override void Awake()
        {
            m_Trans = transform;
            m_btnClose = m_Trans.FindChild("#btn_close").GetComponent<Button>();
            m_gridCarditems = m_Trans.FindChild("#scroll_carditems/#grid_carditems");
            m_scrollCarditems = m_Trans.FindChild("#scroll_carditems").gameObject;
            m_btnPlayers = m_Trans.FindChild("#btn_players").GetComponent<Button>();
            m_gridPlayers = m_Trans.FindChild("#container_players/#grid_players");
            m_containerPlayers = m_Trans.FindChild("#container_players").gameObject;
            m_btnFriends = m_Trans.FindChild("#btn_friends").GetComponent<Button>();

        }

        // Update is called once per frame
        void Update()
        {

        }

		void OnDestroy()
		{
            m_btnClose = null;
            m_gridCarditems = null;
            m_scrollCarditems = null;
            m_btnPlayers = null;
            m_gridPlayers = null;
            m_containerPlayers = null;
            m_btnFriends = null;

		}
    }
}
