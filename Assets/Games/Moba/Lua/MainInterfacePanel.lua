
local m_Trans;
local gameObject;

MainInterfacePanel = {};
local this = MainInterfacePanel;



--构造函数--
function MainInterfacePanel.Awake(object) 
	gameObject = object;
	m_Trans = gameObject.transform;
	this.InitVarCode();	
	warn("Awake lua--->>"..m_Trans.name);
end

--初始化变量代码--
function MainInterfacePanel.InitVarCode() 
    MainInterfacePanel.imgBg = m_Trans:FindChild("#btn_shop/#img_bg"):GetComponent('Image');
    MainInterfacePanel.imgIcon = m_Trans:FindChild("#btn_shop/#img_icon"):GetComponent('Image');
    MainInterfacePanel.btnShop = m_Trans:FindChild("#btn_shop"):GetComponent('Button');
    MainInterfacePanel.imgBg = m_Trans:FindChild("#btn_map/#img_bg"):GetComponent('Image');
    MainInterfacePanel.imgIcon = m_Trans:FindChild("#btn_map/#img_icon"):GetComponent('Image');
    MainInterfacePanel.btnMap = m_Trans:FindChild("#btn_map"):GetComponent('Button');
    MainInterfacePanel.txtName = m_Trans:FindChild("#container_info/#txt_name"):GetComponent('Text');
    MainInterfacePanel.imgLevelbg = m_Trans:FindChild("#container_info/#container_level/#img_levelbg"):GetComponent('Image');
    MainInterfacePanel.imgLevelbar = m_Trans:FindChild("#container_info/#container_level/#img_levelbar"):GetComponent('Image');
    MainInterfacePanel.txtLevel = m_Trans:FindChild("#container_info/#container_level/#txt_level"):GetComponent('Text');
    MainInterfacePanel.containerLevel = m_Trans:FindChild("#container_info/#container_level").gameObject;
    MainInterfacePanel.imgMedalbg = m_Trans:FindChild("#container_info/#container_medal/#img_medalbg"):GetComponent('Image');
    MainInterfacePanel.imgMedal = m_Trans:FindChild("#container_info/#container_medal/#img_medal"):GetComponent('Image');
    MainInterfacePanel.txtMedal = m_Trans:FindChild("#container_info/#container_medal/#txt_medal"):GetComponent('Text');
    MainInterfacePanel.containerMedal = m_Trans:FindChild("#container_info/#container_medal").gameObject;
    MainInterfacePanel.containerInfo = m_Trans:FindChild("#container_info").gameObject;
    MainInterfacePanel.imgDiamondbg = m_Trans:FindChild("#container_resourcebar/#container_diamondbar/#img_diamondbg"):GetComponent('Image');
    MainInterfacePanel.btnAdddiamond = m_Trans:FindChild("#container_resourcebar/#container_diamondbar/#btn_adddiamond"):GetComponent('Button');
    MainInterfacePanel.imgDiamond = m_Trans:FindChild("#container_resourcebar/#container_diamondbar/#img_diamond"):GetComponent('Image');
    MainInterfacePanel.txtDiamond = m_Trans:FindChild("#container_resourcebar/#container_diamondbar/#txt_diamond"):GetComponent('Text');
    MainInterfacePanel.containerDiamondbar = m_Trans:FindChild("#container_resourcebar/#container_diamondbar").gameObject;
    MainInterfacePanel.imgIronbg = m_Trans:FindChild("#container_resourcebar/#container_ironbar/#img_ironbg"):GetComponent('Image');
    MainInterfacePanel.imgIronbar = m_Trans:FindChild("#container_resourcebar/#container_ironbar/#img_ironbar"):GetComponent('Image');
    MainInterfacePanel.txtIron = m_Trans:FindChild("#container_resourcebar/#container_ironbar/#txt_iron"):GetComponent('Text');
    MainInterfacePanel.imgIron = m_Trans:FindChild("#container_resourcebar/#container_ironbar/#img_iron"):GetComponent('Image');
    MainInterfacePanel.containerIronbar = m_Trans:FindChild("#container_resourcebar/#container_ironbar").gameObject;
    MainInterfacePanel.imgStonebg = m_Trans:FindChild("#container_resourcebar/#container_stonebar/#img_stonebg"):GetComponent('Image');
    MainInterfacePanel.imgStonebar = m_Trans:FindChild("#container_resourcebar/#container_stonebar/#img_stonebar"):GetComponent('Image');
    MainInterfacePanel.txtStone = m_Trans:FindChild("#container_resourcebar/#container_stonebar/#txt_stone"):GetComponent('Text');
    MainInterfacePanel.imgStone = m_Trans:FindChild("#container_resourcebar/#container_stonebar/#img_stone"):GetComponent('Image');
    MainInterfacePanel.containerStonebar = m_Trans:FindChild("#container_resourcebar/#container_stonebar").gameObject;
    MainInterfacePanel.imgWoodbg = m_Trans:FindChild("#container_resourcebar/#container_woodbar/#img_woodbg"):GetComponent('Image');
    MainInterfacePanel.imgWoodbar = m_Trans:FindChild("#container_resourcebar/#container_woodbar/#img_woodbar"):GetComponent('Image');
    MainInterfacePanel.txtWood = m_Trans:FindChild("#container_resourcebar/#container_woodbar/#txt_wood"):GetComponent('Text');
    MainInterfacePanel.imgWood = m_Trans:FindChild("#container_resourcebar/#container_woodbar/#img_wood"):GetComponent('Image');
    MainInterfacePanel.containerWoodbar = m_Trans:FindChild("#container_resourcebar/#container_woodbar").gameObject;
    MainInterfacePanel.imgGoldbg = m_Trans:FindChild("#container_resourcebar/#container_goldbar/#img_goldbg"):GetComponent('Image');
    MainInterfacePanel.imgGoldbar = m_Trans:FindChild("#container_resourcebar/#container_goldbar/#img_goldbar"):GetComponent('Image');
    MainInterfacePanel.txtGold = m_Trans:FindChild("#container_resourcebar/#container_goldbar/#txt_gold"):GetComponent('Text');
    MainInterfacePanel.imgGold = m_Trans:FindChild("#container_resourcebar/#container_goldbar/#img_gold"):GetComponent('Image');
    MainInterfacePanel.containerGoldbar = m_Trans:FindChild("#container_resourcebar/#container_goldbar").gameObject;
    MainInterfacePanel.containerResourcebar = m_Trans:FindChild("#container_resourcebar").gameObject;
    MainInterfacePanel.imgAchivement = m_Trans:FindChild("#container_mainbtns/#btn_achivement/#img_achivement"):GetComponent('Image');
    MainInterfacePanel.btnAchivement = m_Trans:FindChild("#container_mainbtns/#btn_achivement"):GetComponent('Button');
    MainInterfacePanel.imgFriend = m_Trans:FindChild("#container_mainbtns/#btn_friend/#img_friend"):GetComponent('Image');
    MainInterfacePanel.btnFriend = m_Trans:FindChild("#container_mainbtns/#btn_friend"):GetComponent('Button');
    MainInterfacePanel.imgSetting = m_Trans:FindChild("#container_mainbtns/#btn_setting/#img_setting"):GetComponent('Image');
    MainInterfacePanel.btnSetting = m_Trans:FindChild("#container_mainbtns/#btn_setting"):GetComponent('Button');
    MainInterfacePanel.containerMainbtns = m_Trans:FindChild("#container_mainbtns").gameObject;

end

--销毁事件--
function MainInterfacePanel.OnDestroy()
    MainInterfacePanel.imgBg = nil;
    MainInterfacePanel.imgIcon = nil;
    MainInterfacePanel.btnShop = nil;
    MainInterfacePanel.imgBg = nil;
    MainInterfacePanel.imgIcon = nil;
    MainInterfacePanel.btnMap = nil;
    MainInterfacePanel.txtName = nil;
    MainInterfacePanel.imgLevelbg = nil;
    MainInterfacePanel.imgLevelbar = nil;
    MainInterfacePanel.txtLevel = nil;
    MainInterfacePanel.containerLevel = nil;
    MainInterfacePanel.imgMedalbg = nil;
    MainInterfacePanel.imgMedal = nil;
    MainInterfacePanel.txtMedal = nil;
    MainInterfacePanel.containerMedal = nil;
    MainInterfacePanel.containerInfo = nil;
    MainInterfacePanel.imgDiamondbg = nil;
    MainInterfacePanel.btnAdddiamond = nil;
    MainInterfacePanel.imgDiamond = nil;
    MainInterfacePanel.txtDiamond = nil;
    MainInterfacePanel.containerDiamondbar = nil;
    MainInterfacePanel.imgIronbg = nil;
    MainInterfacePanel.imgIronbar = nil;
    MainInterfacePanel.txtIron = nil;
    MainInterfacePanel.imgIron = nil;
    MainInterfacePanel.containerIronbar = nil;
    MainInterfacePanel.imgStonebg = nil;
    MainInterfacePanel.imgStonebar = nil;
    MainInterfacePanel.txtStone = nil;
    MainInterfacePanel.imgStone = nil;
    MainInterfacePanel.containerStonebar = nil;
    MainInterfacePanel.imgWoodbg = nil;
    MainInterfacePanel.imgWoodbar = nil;
    MainInterfacePanel.txtWood = nil;
    MainInterfacePanel.imgWood = nil;
    MainInterfacePanel.containerWoodbar = nil;
    MainInterfacePanel.imgGoldbg = nil;
    MainInterfacePanel.imgGoldbar = nil;
    MainInterfacePanel.txtGold = nil;
    MainInterfacePanel.imgGold = nil;
    MainInterfacePanel.containerGoldbar = nil;
    MainInterfacePanel.containerResourcebar = nil;
    MainInterfacePanel.imgAchivement = nil;
    MainInterfacePanel.btnAchivement = nil;
    MainInterfacePanel.imgFriend = nil;
    MainInterfacePanel.btnFriend = nil;
    MainInterfacePanel.imgSetting = nil;
    MainInterfacePanel.btnSetting = nil;
    MainInterfacePanel.containerMainbtns = nil;

	warn("OnDestroy----->>");
end