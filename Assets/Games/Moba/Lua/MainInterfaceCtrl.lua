--require "Common/define"
--require "Common/functions"
--require "Logic/CS_Framework"
--require "Table/RoleLevelTable"
--require "Controller/Dungeon/DungeonAgent"
--require "Controller/Pet/PetAgent"
--require "Controller/WorldMinningCtrl"

--主界面控制器，主要控制主界面上的元素和子界面的排版、显示、隐藏等
MainInterfaceCtrl = {};
local this = MainInterfaceCtrl;

local trans;
local prompt;
local mainInterfaceUI;


--新建对象--
function MainInterfaceCtrl.New()
	warn("mainInterfaceUICtrl.New--->>");
    
	return this;
end
--mainInterfaceUICtrl.IsInCommonBattle = false;
--mainInterfaceUICtrl.IsInFubenBattle = false;
--mainInterfaceUICtrl.IFViewInited = false;
--启动事件--
function MainInterfaceCtrl.Awake()
	warn("mainInterfaceUICtrl.Awake--->>");
    --uiMgr:ShowPanel(CtrlNames.MAIN_UI_CTRL, Const.ConstPanel, UILayerType.Fixed, true);
end

--创建面板回调函数--
function MainInterfaceCtrl.OnShow()
end

--创建面板回调函数--
function MainInterfaceCtrl.OnViewCreated(obj)
	gameObject = obj;
	trans = gameObject.transform;
	--uiMgr:ShowPanel(Const.QueuePanel, CtrlNames.MAIN_UI_CTRL, true);
    --this.SwitchToCommon();
	mainInterfaceUI = gameObject:GetComponent('LuaPanelBase');
    --MainInterfacePanel.containerActivitytop.gameObject:SetActive(false);
    --MainInterfacePanel.btnDebug.gameObject:SetActive(true);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnAchivement, this.OnFubenClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnInfo, this.OnHeadClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnDebug, this.OnDebugClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnShishen, this.OnShishenClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnPvp, this.OnPVP);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnPartner, this.OnPartner);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnTask, this.OnTaskClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnSwitch1, this.OnBagClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnSkillbtn, this.OnSkillClick);
    --mainInterfaceUI:AddToggleClick(MainInterfacePanel.toggleSwitch2, this.OnBattleModelSwitchClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnPeace_stance, this.OnBattleModelSwitchClick);
    --mainInterfaceUI:AddClick(MainInterfacePanel.btnCombat_stance, this.OnBattleModelSwitchClick);
    --[[
    mainInterfaceUICtrl.mainMenuCtrl = UI.Component.MainMenuController.Create();
    mainInterfaceUICtrl.mainMenuCtrl:AddHorizontalItem(MainInterfacePanel.btnPackage);
    mainInterfaceUICtrl.mainMenuCtrl:AddHorizontalItem(MainInterfacePanel.btnSkillbtn);
    mainInterfaceUICtrl.mainMenuCtrl:AddHorizontalItem(MainInterfacePanel.btnPartner);
    mainInterfaceUICtrl.mainMenuCtrl:AddHorizontalItem(MainInterfacePanel.btnRune);
    
    mainInterfaceUICtrl.mainMenuCtrl:AddVerticalItem(MainInterfacePanel.btnShop);
    mainInterfaceUICtrl.mainMenuCtrl:AddVerticalItem(MainInterfacePanel.btnMount);

    mainInterfaceUICtrl.mainMenuCtrl:SetSwitchBtn(MainInterfacePanel.btnSwitch1);
    ]]--

    --mainInterfaceUI:AddClick(MainInterfacePanel.btnExit, this.ExitBattleScene);
    --if mainInterfaceUICtrl.IsInCommonBattle then
    --    mainInterfaceUICtrl.SwitchToCommonBattle();
    --elseif mainInterfaceUICtrl.IsInFubenBattle then
    --     mainInterfaceUICtrl.SwitchToFubenBattle();
    -- else
    --    mainInterfaceUICtrl.SwitchToCommon();
    -- end

    --mainInterfaceUICtrl.IFViewInited = true;
    --mainInterfaceUICtrl.InitShowData();
end

--初始化面板--
function MainInterfaceCtrl.InitPanel(prefab)
	
end

--获取面板--
function MainInterfaceCtrl.GetPanelName()
	return "MainInterfacePanel";
end

function MainInterfaceCtrl.GetCtrlName()
    return "MainInterfaceCtrl";
end

--单击事件--
function MainInterfaceCtrl.OnClick(obj)
	this.Close();
	warn("mainInterfaceUICtrl.OnClick---->>>"..obj.name);
end

--单击滚动项--
function MainInterfaceCtrl.OnItemClick(obj)
	warn("mainInterfaceUICtrl.OnItemClick-->>"..obj.name);
end

--关闭提示面板控制器--
function MainInterfaceCtrl.Close()
	uiMgr:ClosePanel(Const.ConstPanel, CtrlNames.MAIN_UI_CTRL, true);
end

