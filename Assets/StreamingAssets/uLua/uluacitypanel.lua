UnityRaw    5.x.x 5.2.2f1   \   <              \   4    CAB-13aa3ec0a48e8ebccfe1cb08e0df287c    4  ě     ˛  ě          5.2.2f1       1   g9ô_&ý¤ępňČĘÔ         O 7  ˙˙˙˙         H Ť ˙˙˙˙      1  1  ˙˙˙˙   @    Ţ              Q  j             H ę ˙˙˙˙     1  1  ˙˙˙˙   @   Ţ             Q  j            H     ˙˙˙˙	      1  1  ˙˙˙˙
   @    Ţ              Q  j           m_PathName    ňŤśňßëíŽ+kPCh,   Ă          7  ˙˙˙˙         H Ť ˙˙˙˙      1  1  ˙˙˙˙   @    Ţ              Q  j             Ő    ˙˙˙˙        1  1  ˙˙˙˙         Ţ               y j              Ţ        	        . $      
        ń  -   ˙˙˙˙       1  1  ˙˙˙˙        Ţ                j  ˙˙˙˙        H   ˙˙˙˙       1  1  ˙˙˙˙   @    Ţ              Q  j             9   
             Ţ  C               Ţ  P               y \               Ţ                . $              9   b               Ţ  C               Ţ  P               y \               Ţ                . $              Ś n               H    ˙˙˙˙        1  1  ˙˙˙˙!   @    Ţ      "        Q  j     #        Ő    ˙˙˙˙$       1  1  ˙˙˙˙%        Ţ      &         H j  ˙˙˙˙'       1  1  ˙˙˙˙(   @    Ţ      )        Q  j     *        L  Ś      +    @  AssetBundle m_PreloadTable m_FileID m_PathID m_Container AssetInfo preloadIndex preloadSize asset m_MainAsset m_RuntimeCompatibility m_AssetBundleName m_Dependencies m_IsStreamedSceneAssetBundle     IzĆLtŕ    \  1   1 ˙˙           `         ˙˙                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       uluacitypanel   ?  function OnSettingButtonClick()
	CityPanelGameObject.name = "abcd"
end

function OnShopButtonClick()
	CityShopPanelRoot.gameObject:SetActive(true)
	root:SetActive (false)
end

function OnBattleButtonClick()

end


function Init()
    CityShopPanelRoot = CityShopPanelGameObject.transform:Find("root")
    root=CityPanelGameObject.transform:Find("root")
    
    shopButton = CityPanelGameObject.transform:Find("root/Shop"):GetComponent("Button")
    shopButton.onClick:AddListener(OnShopButtonClick)
    
    battleButton = CityPanelGameObject.transform:Find("root/Shop"):GetComponent("Button")
    battleButton.onClick:AddListener(OnBattleButtonClick);
    
	userName = CityPanelGameObject.transform:Find("root/UserInfo/Slider/UserName"):GetComponent("Text")
	userName.text = userInfo.userName
	userLevel = CityPanelGameObject.transform:Find("root/UserInfo/Slider/UserLevel"):GetComponent("Text")
	userLevel.text = userInfo.userLevel
	userExp = CityPanelGameObject.transform:Find("root/UserInfo/Slider/UserExp"):GetComponent("Text")
	userExp.text = userInfo.userExp
	workNum = CityPanelGameObject.transform:Find("root/UserInfo/Slider1/WorkNum"):GetComponent("Text")
	workNum.text = userInfo.workNum.."/5";
	protectTime = CityPanelGameObject.transform:Find("root/UserInfo/Slider2/ProtectTime"):GetComponent("Text")
	protectTime.text = userInfo.protectTime / 3600 .. "ĺ°ćś" .. userInfo.protectTime % 3600 / 60 .. "ĺé";
	
	cornSlider = CityPanelGameObject.transform:Find("root/UserInfo/Slider3"):GetComponent("Slider")
	cornSlider.value = userInfo.currentCorn / userInfo.maxCorn
	currentCorn = CityPanelGameObject.transform:Find("root/UserInfo/Slider3/CurrentCorn"):GetComponent("Text")
	currentCorn.text = userInfo.currentCorn;
	maxCorn = CityPanelGameObject.transform:Find("root/UserInfo/Slider3/MaxCorn"):GetComponent("Text")
	maxCorn.text=userInfo.maxCorn;
	
	woodSlider = CityPanelGameObject.transform:Find("root/UserInfo/Slider4"):GetComponent("Slider")
	woodSlider.value = userInfo.currentWood / userInfo.maxWood
	currentWood = CityPanelGameObject.transform:Find("root/UserInfo/Slider4/CurrentWood"):GetComponent("Text")
	currentWood.text = userInfo.currentWood
	maxWood = CityPanelGameObject.transform:Find("root/UserInfo/Slider4/MaxWood"):GetComponent("Text")
	maxWood.text = userInfo.maxWood;
	
	currentStone = CityPanelGameObject.transform:Find("root/UserInfo/Slider5/CurrentStone"):GetComponent("Text")
	maxStone = CityPanelGameObject.transform:Find("root/UserInfo/Slider5/MaxStone"):GetComponent("Text")
	stoneSlider = CityPanelGameObject.transform:Find("root/UserInfo/Slider5"):GetComponent("Slider")
	currentStone.text = userInfo.currentStone
	maxStone.text = userInfo.maxStone
	stoneSlider.value = userInfo.currentStone / userInfo.maxStone
end


function doClick()
     print('Button Click:>>>')
end                    IzĆLtŕ   "   assets/_moba/lua/uluacitypanel.txt             IzĆLtŕ                          uluacitypanel.lua           