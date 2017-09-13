//==========================================
// Created By yingyugang At 1/28/2016 9:05:28 PM
//==========================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainInterfacePanel
{
    ///<summary>
    ///
    ///</summary>
    public class MainInterfacePanelView : PanelBase
    {
        ///////////////////////////////////以下为静态成员//////////////////////////////

        ///////////////////////////////////以下为非静态成员///////////////////////////
        public Transform m_Trans;
        public Text m_txtName;
        public Image m_imgLevelbg;
        public Image m_imgLevelbar;
        public Text m_txtLevel;
        public GameObject m_containerLevel;
        public Image m_imgMedalbg;
        public Image m_imgMedal;
        public Text m_txtMedal;
        public GameObject m_containerMedal;
        public GameObject m_containerInfo;
        public Button m_btnShop;
        public Button m_btnMap;
        public Text m_txtGold;
        public Image m_imgGold;
        public Slider m_sliderGold;
        public Text m_txtWood;
        public Image m_imgWood;
        public Slider m_sliderWood;
        public Text m_txtStone;
        public Image m_imgStone;
        public Slider m_sliderStone;
        public Text m_txtIron;
        public Image m_imgIron;
        public Slider m_sliderIron;
        public GameObject m_containerResourcebar;
        public Image m_imgDiamondbg;
        public Button m_btnAdddiamond;
        public Image m_imgDiamond;
        public Text m_txtDiamond;
        public GameObject m_containerDiamondbar;
        public Button m_btnAchivement;
        public Button m_btnFriend;
        public Button m_btnTeam;
        public Image m_imgSetting;
        public Button m_btnSetting;
        public Image m_imgMail;
        public Button m_btnMail;
        public GameObject m_containerMainbtns;
        public Button m_btnChat;
        public Button m_btnTeambattle;
        public Text m_txtLevelpopboxtitle;
        public Text m_txtProgress_to_next_level;
        public GameObject m_containerLevelpopbox;
        public Text m_txtMedalpopboxtitle;
        public GameObject m_containerMedalpopbox;
        public Text m_txtTitle;
        public Text m_txtProduction_per_hour;
        public Text m_txtFrom_base;
        public Text m_txtFrom_freed_villages;
        public Text m_txtStorage_capacity;
        public Text m_txtProtected_by_vault;
        public GameObject m_containerResourcepopbox;


        // Use this for initialization
        public override void Awake()
        {
            m_Trans = transform;
            m_txtName = m_Trans.FindChild("#container_info/#txt_name").GetComponent<Text>();
            m_imgLevelbg = m_Trans.FindChild("#container_info/#container_level/#img_levelbg").GetComponent<Image>();
            m_imgLevelbar = m_Trans.FindChild("#container_info/#container_level/#img_levelbar").GetComponent<Image>();
            m_txtLevel = m_Trans.FindChild("#container_info/#container_level/#txt_level").GetComponent<Text>();
            m_containerLevel = m_Trans.FindChild("#container_info/#container_level").gameObject;
            m_imgMedalbg = m_Trans.FindChild("#container_info/#container_medal/#img_medalbg").GetComponent<Image>();
            m_imgMedal = m_Trans.FindChild("#container_info/#container_medal/#img_medal").GetComponent<Image>();
            m_txtMedal = m_Trans.FindChild("#container_info/#container_medal/#txt_medal").GetComponent<Text>();
            m_containerMedal = m_Trans.FindChild("#container_info/#container_medal").gameObject;
            m_containerInfo = m_Trans.FindChild("#container_info").gameObject;
            m_btnShop = m_Trans.FindChild("#btn_shop").GetComponent<Button>();
            m_btnMap = m_Trans.FindChild("#btn_map").GetComponent<Button>();
            m_txtGold = m_Trans.FindChild("#container_resourcebar/#slider_gold/#txt_gold").GetComponent<Text>();
            m_imgGold = m_Trans.FindChild("#container_resourcebar/#slider_gold/#img_gold").GetComponent<Image>();
            m_sliderGold = m_Trans.FindChild("#container_resourcebar/#slider_gold").GetComponent<Slider>();
            m_txtWood = m_Trans.FindChild("#container_resourcebar/#slider_wood/#txt_wood").GetComponent<Text>();
            m_imgWood = m_Trans.FindChild("#container_resourcebar/#slider_wood/#img_wood").GetComponent<Image>();
            m_sliderWood = m_Trans.FindChild("#container_resourcebar/#slider_wood").GetComponent<Slider>();
            m_txtStone = m_Trans.FindChild("#container_resourcebar/#slider_stone/#txt_stone").GetComponent<Text>();
            m_imgStone = m_Trans.FindChild("#container_resourcebar/#slider_stone/#img_stone").GetComponent<Image>();
            m_sliderStone = m_Trans.FindChild("#container_resourcebar/#slider_stone").GetComponent<Slider>();
            m_txtIron = m_Trans.FindChild("#container_resourcebar/#slider_iron/#txt_iron").GetComponent<Text>();
            m_imgIron = m_Trans.FindChild("#container_resourcebar/#slider_iron/#img_iron").GetComponent<Image>();
            m_sliderIron = m_Trans.FindChild("#container_resourcebar/#slider_iron").GetComponent<Slider>();
            m_containerResourcebar = m_Trans.FindChild("#container_resourcebar").gameObject;
            m_imgDiamondbg = m_Trans.FindChild("#container_diamondbar/#img_diamondbg").GetComponent<Image>();
            m_btnAdddiamond = m_Trans.FindChild("#container_diamondbar/#btn_adddiamond").GetComponent<Button>();
            m_imgDiamond = m_Trans.FindChild("#container_diamondbar/#img_diamond").GetComponent<Image>();
            m_txtDiamond = m_Trans.FindChild("#container_diamondbar/#txt_diamond").GetComponent<Text>();
            m_containerDiamondbar = m_Trans.FindChild("#container_diamondbar").gameObject;
            m_btnAchivement = m_Trans.FindChild("#container_mainbtns/#btn_achivement").GetComponent<Button>();
            m_btnFriend = m_Trans.FindChild("#container_mainbtns/#btn_friend").GetComponent<Button>();
            m_btnTeam = m_Trans.FindChild("#container_mainbtns/#btn_team").GetComponent<Button>();
            m_imgSetting = m_Trans.FindChild("#container_mainbtns/#btn_setting/#img_setting").GetComponent<Image>();
            m_btnSetting = m_Trans.FindChild("#container_mainbtns/#btn_setting").GetComponent<Button>();
            m_imgMail = m_Trans.FindChild("#container_mainbtns/#btn_mail/#img_mail").GetComponent<Image>();
            m_btnMail = m_Trans.FindChild("#container_mainbtns/#btn_mail").GetComponent<Button>();
            m_containerMainbtns = m_Trans.FindChild("#container_mainbtns").gameObject;
            m_btnChat = m_Trans.FindChild("#btn_chat").GetComponent<Button>();
            m_btnTeambattle = m_Trans.FindChild("#btn_teambattle").GetComponent<Button>();
            m_txtLevelpopboxtitle = m_Trans.FindChild("#container_levelpopbox/#txt_levelpopboxtitle").GetComponent<Text>();
            m_txtProgress_to_next_level = m_Trans.FindChild("#container_levelpopbox/#txt_progress_to_next_level").GetComponent<Text>();
            m_containerLevelpopbox = m_Trans.FindChild("#container_levelpopbox").gameObject;
            m_txtMedalpopboxtitle = m_Trans.FindChild("#container_medalpopbox/#txt_medalpopboxtitle").GetComponent<Text>();
            m_containerMedalpopbox = m_Trans.FindChild("#container_medalpopbox").gameObject;
            m_txtTitle = m_Trans.FindChild("#container_resourcepopbox/#txt_title").GetComponent<Text>();
            m_txtProduction_per_hour = m_Trans.FindChild("#container_resourcepopbox/#txt_production_per_hour").GetComponent<Text>();
            m_txtFrom_base = m_Trans.FindChild("#container_resourcepopbox/#txt_from_base").GetComponent<Text>();
            m_txtFrom_freed_villages = m_Trans.FindChild("#container_resourcepopbox/#txt_from_freed_villages").GetComponent<Text>();
            m_txtStorage_capacity = m_Trans.FindChild("#container_resourcepopbox/#txt_storage_capacity").GetComponent<Text>();
            m_txtProtected_by_vault = m_Trans.FindChild("#container_resourcepopbox/#txt_protected_by_vault").GetComponent<Text>();
            m_containerResourcepopbox = m_Trans.FindChild("#container_resourcepopbox").gameObject;

        }

        // Update is called once per frame
        void Update()
        {

        }

		void OnDestroy()
		{
            m_txtName = null;
            m_imgLevelbg = null;
            m_imgLevelbar = null;
            m_txtLevel = null;
            m_containerLevel = null;
            m_imgMedalbg = null;
            m_imgMedal = null;
            m_txtMedal = null;
            m_containerMedal = null;
            m_containerInfo = null;
            m_btnShop = null;
            m_btnMap = null;
            m_txtGold = null;
            m_imgGold = null;
            m_sliderGold = null;
            m_txtWood = null;
            m_imgWood = null;
            m_sliderWood = null;
            m_txtStone = null;
            m_imgStone = null;
            m_sliderStone = null;
            m_txtIron = null;
            m_imgIron = null;
            m_sliderIron = null;
            m_containerResourcebar = null;
            m_imgDiamondbg = null;
            m_btnAdddiamond = null;
            m_imgDiamond = null;
            m_txtDiamond = null;
            m_containerDiamondbar = null;
            m_btnAchivement = null;
            m_btnFriend = null;
            m_btnTeam = null;
            m_imgSetting = null;
            m_btnSetting = null;
            m_imgMail = null;
            m_btnMail = null;
            m_containerMainbtns = null;
            m_btnChat = null;
            m_btnTeambattle = null;
            m_txtLevelpopboxtitle = null;
            m_txtProgress_to_next_level = null;
            m_containerLevelpopbox = null;
            m_txtMedalpopboxtitle = null;
            m_containerMedalpopbox = null;
            m_txtTitle = null;
            m_txtProduction_per_hour = null;
            m_txtFrom_base = null;
            m_txtFrom_freed_villages = null;
            m_txtStorage_capacity = null;
            m_txtProtected_by_vault = null;
            m_containerResourcepopbox = null;

		}
    }
}
