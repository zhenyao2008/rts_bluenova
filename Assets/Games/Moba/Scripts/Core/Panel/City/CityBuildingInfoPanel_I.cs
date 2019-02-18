using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class CityBuildingInfoPanel_I : CityBasePanel_I {

	public Button closeButton;
	public Button returnButton;
	public Text buildingName;
	public Text soilderName;
	public Text buildCorn;
	public Text buildTime;
	public Text health;
	public Text damage;
	public Text attackType;
	public Button attackTypeBtn;
	public GameObject attackTypeInfo;
	public Button attackTypeCloseBtn;
	public Text attackSpeed;
	public Text attackRange;
	public Text armor;
	public Text armorType;
	public Button armorTypeBtn;
	public GameObject armorTypeInfo;
	public Button armorTypeCloseBtn;
	public Text corn;//突破奖励
	public Text skillInfo;
//	public List<GameObject> arrows;
	public Vector3 levelPrefabScale = new Vector3 (50, 50, 50);
	public Vector3 defaultLevelPrefabOffset = new Vector3 (0, -50, -50);

	public Transform prefabPoint;
	
	public List<Transform> unitPrefabPoints;

	public List<Button> unitButtons;


	public List<UnitButton> level0;
	public List<UnitButton> level1;
	public List<UnitButton> level2;

	static CityBuildingInfoPanel_I instance;
	public static CityBuildingInfoPanel_I SingleTon(){
		return instance;
	}

	void Awake(){
		for(int i=0;i<unitButtons.Count;i++)
		{
			unitButtons[i].onClick.AddListener(OnUnitButtonClick);
		}
		closeButton.onClick.AddListener (Close);
		instance = this;
	}

	void Close(){
		root.SetActive (false);
		CityPanel_I.SingleTon ().root.SetActive (true);
	}

	public void OnUnitButtonClick()
	{
		GameObject go = EventSystem.current.currentSelectedGameObject;
		Debug.Log (go.name);
	}

	public void SetBuildingInfo(CityBuilding cb)
	{
		SetCurrentLevelUnits (cb);
		UnitProperties ua = cb.unitProperties;
		buildingName.text = cb.buildingName + " (等级" + cb.level +")";
		SetUnitInfo (ua);
	}

	GameObject mCurrentPrefab;
	public void SetUnitInfo(UnitProperties ua)
	{
//		HideAttackInfo ();
//		HideArmorInfo ();
		this.soilderName.text = ua.unitName;
		//		this.buildCorn.text = ua.buildCorn.ToString();
		this.buildTime.text = ua.buildDuration.ToString () + "s";
		this.health.text = ua.baseHealth.ToString();
		this.damage.text = ua.minDamage + "-" + ua.maxDamage;
		switch(ua.attackType)
		{
		case AttackType.Normal:
			this.attackType.text = "普通";break;
		case AttackType.Puncture:
			this.attackType.text = "穿刺";break;
		case AttackType.Magic:
			this.attackType.text = "魔法";break;
		case AttackType.Siege:
			this.attackType.text = "攻城";break;
		case AttackType.Chaos:
			this.attackType.text = "混乱";break;
		}
		this.attackSpeed.text = ua.attackInterval + "s/次";
		this.attackRange.text = ua.attackRange + "/" + (ua.isMelee ? "近战" : "远程");
		this.armor.text = ua.armor.ToString();
		switch(ua.armorType)
		{
		case ArmorType.None:
			this.armorType.text = "无甲";break;
		case ArmorType.Light:
			this.armorType.text = "轻甲";break;
		case ArmorType.Middle:
			this.armorType.text = "中甲";break;
		case ArmorType.Heavy:
			this.armorType.text = "重甲";break;
		case ArmorType.Construction:
			this.armorType.text = "建筑";break;
		}
		this.corn.text = ua.killPrice.ToString();
		this.skillInfo.text = ua.skillInfo;
		
		if(mCurrentPrefab!=null){
			mCurrentPrefab.SetActive(false);
		}
		GameObject go = Instantiate (ua.gameObject) as GameObject;
		go.transform.parent = prefabPoint;
		UnitCityProperties ucp = go.GetComponent<UnitCityProperties> ();
		go.transform.localPosition = ucp.posOffsetInCityBuildingPanelLeft;
		go.transform.localEulerAngles = ucp.angleOffsetInCityBuildingPanelLeft;
		go.transform.localScale = ucp.localScaleInCityBuildingPanelLeft;
		Animation anim = go.GetComponentInChildren<Animation> ();
		if(anim && anim[ucp.animStateInCityBuildingPanelLeft])
		{
			anim.wrapMode = WrapMode.Loop;
			anim.Play(ucp.animStateInCityBuildingPanelLeft);
		}
		mCurrentPrefab = go;
	}

	void SetCurrentLevelUnits(CityBuilding cityBuilding)
	{
		cityBuilding.unitProperties = cityBuilding.level0 [0];
		SetBuildingUnit (level0 [0],cityBuilding.level0 [0]);
		SetBuildingUnit (level1 [0],cityBuilding.level1 [0]);
		SetBuildingUnit (level1 [1],cityBuilding.level1 [1]);
		SetBuildingUnit (level2 [0],cityBuilding.level2 [0]);
		SetBuildingUnit (level2 [1],cityBuilding.level2 [1]);
		SetBuildingUnit (level2 [2],cityBuilding.level2 [2]);
		SetBuildingUnit (level2 [3],cityBuilding.level2 [3]);
	}

	void SetBuildingUnit(UnitButton item,UnitProperties up)
	{
		
		if(item.unitProperties)
		{
			Destroy(item.unitProperties.gameObject);
		}
		if (!up)
			return;
		GameObject go = Instantiate (up.gameObject) as GameObject;
		item.unitProperties = go.GetComponent<UnitProperties>();//TODO,set current level and addition values
		go.transform.parent = item.button.transform;
		UnitCityProperties ucp = go.GetComponent<UnitCityProperties>();
		go.transform.localEulerAngles = new Vector3(0,180,0);
		go.transform.localPosition = defaultLevelPrefabOffset + ucp.offsetInCityBuildingPanelRight;
		go.transform.localScale = ucp.localScaleInCityBuildingPanelRight;
	}




}

[System.Serializable]
public class UnitButton
{
	public Button button;
	public UnitProperties unitProperties;
}




