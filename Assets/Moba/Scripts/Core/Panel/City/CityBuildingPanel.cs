using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class CityBuildingPanel : PanelBase {

	public UILabel buildingName;
	public UILabel soilderName;
	public UILabel buildCorn;
	public UILabel buildTime;
	public UILabel health;
	public UILabel damage;
	
	public UILabel attackType;
	public UIButton attackTypeBtn;
	public GameObject attackTypeInfo;
	public UIButton attackTypeCloseBtn;

	public UILabel attackSpeed;
	public UILabel attackRange;
	public UILabel armor;
	public UILabel armorType;
	public UIButton armorTypeBtn;
	public GameObject armorTypeInfo;
	public UIButton armorTypeCloseBtn;

	public UILabel corn;//突破奖励
	public UILabel skillInfo;

	public UIButton upgradeBtn;
	public UIButton upgradeBtn1;

	public List<GameObject> arrows;
	public Vector3 levelPrefabScale = new Vector3 (50, 50, 50);
	public Transform prefabPoint;

	public List<Transform> unitPrefabPoints;
	public List<UIEventTrigger> unitPrefabTriggers;

	public List<CityBuildingInfoItem> level0;
	public List<CityBuildingInfoItem> level1;
	public List<CityBuildingInfoItem> level2;



	public List<GameObject> unitPrefabs;

	public Transform roundEffect;

	GameObject mCurrentPrefab;
//	List<GameObject> preLevelPrefabs = new List<GameObject>();

//	public bool load;

	public static CityBuildingPanel SingleTon(){
		return instance;
	}

	public static CityBuildingPanel instance;

	void Awake()
	{
		instance = this;
		for(int i=0;i<unitPrefabTriggers.Count;i++)
		{
			unitPrefabTriggers[i].onClick.Add(new EventDelegate(SelectUnit));
		}
		attackTypeCloseBtn.onClick.Add (new EventDelegate(HideAttackInfo));
		armorTypeCloseBtn.onClick.Add (new EventDelegate(HideArmorInfo));
		attackTypeBtn.onClick.Add (new EventDelegate(OnAttackBtnClick));
		armorTypeBtn.onClick.Add (new EventDelegate(OnArmorBtnClick));
	}

	public void SetCurrentBuilding(CityBuilding cityBuilding)
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

	void SetBuildingUnit(CityBuildingInfoItem item,UnitProperties up)
	{

		if(item.unitProperties)
		{
			Destroy(item.unitProperties.gameObject);
		}
		if (!up)
			return;
		GameObject go = Instantiate (up.gameObject) as GameObject;
		item.unitProperties = go.GetComponent<UnitProperties>();//TODO,set current level and addition values
		go.transform.parent = item.transform;
		UnitCityProperties ucp = go.GetComponent<UnitCityProperties>();
		go.transform.localEulerAngles = new Vector3(0,180,0);
		go.transform.localPosition =new Vector3(0,-50,-50) + ucp.offsetInCityBuildingPanelRight;
		go.transform.localScale = ucp.localScaleInCityBuildingPanelRight;
	}



	GameObject preSelectUnitPrefab;
	UIEventTrigger preSelectUnitTrigger;
	void SelectUnit(){
//		int index = unitPrefabTriggers.IndexOf (UIEventTrigger.current);

		CityBuildingInfoItem item = UIEventTrigger.current.GetComponent<CityBuildingInfoItem>();

//		if (unitPrefabs.Count > index) {
//			if(selectUnitPrefab)
//				selectUnitPrefab.GetComponent<UnitRes>().ShowNormalMaterial();
			preSelectUnitPrefab = item.unitProperties.gameObject; 

			UnitRes unitRes = preSelectUnitPrefab.GetComponent<UnitRes>();
			unitRes.PlayAnim(unitRes.attackAnimStateName);

			if(preSelectUnitTrigger)
				preSelectUnitTrigger.GetComponent<UISprite>().color = Color.green;
			preSelectUnitTrigger = UIEventTrigger.current;
			preSelectUnitTrigger.GetComponent<UISprite>().color = Color.white;
			roundEffect.gameObject.SetActive(true);
			roundEffect.position = preSelectUnitTrigger.transform.position;

			SetUnitInfo(unitRes.GetComponent<UnitProperties>());
//			selectUnitPrefab.GetComponent<UnitRes>().ShowOutlineMaterial();
//		}
	}

	public void SetBuildingInfo(CityBuilding cb)
	{
		UnitProperties ua = cb.unitProperties;
		buildingName.text = cb.buildingName + " (等级" + cb.level +")";
		SetUnitInfo (ua);
	}

	public void SetLevelPrefabs(CityBuilding cb)
	{
//		List<LevelPrefabs> levelPrefabs = cb.leveledSoilderPrefabs;
//		List<GameObject> prefabs = levelPrefabs [0].soilderPrefabs;

	}

	public void SetUnitInfo(UnitProperties ua)
	{
		HideAttackInfo ();
		HideArmorInfo ();
		upgradeBtn.gameObject.SetActive(false);
		upgradeBtn1.gameObject.SetActive(false);
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

//		go.GetComponentInChildren<Animation> ().wrapMode = WrapMode.Loop;
//		go.GetComponentInChildren<Animation> ().Play (go.GetComponent<Enemy> ().idleAnimStateName);
		//		go.GetComponent<UnitBase> ().enabled = false;
//		go.GetComponent<NavMeshAgent> ().enabled = false;
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
//		if(go.GetComponent<UnitBase>().isFlying)
//		{
//			go.transform.localPosition = new Vector3(0,-420,0);
//		}
//		go.transform.localEulerAngles = new Vector3 (0,-25,0);
//		go.transform.localScale = new Vector3 (100,100,100);
//		go.layer = 5;
		mCurrentPrefab = go;
//		Destroy (go.GetComponent<UnitBase>());
	}

	public override void Active()
	{
		base.Active();
		HideAttackInfo ();
		HideArmorInfo ();
	}

	void OnAttackBtnClick()
	{
		HideArmorInfo ();
		if(attackTypeInfo )
		{
			if(attackTypeInfo.activeInHierarchy)
				HideAttackInfo();
			else
				ShowAttackInfo();
		}
	}


	void OnArmorBtnClick()
	{
		HideAttackInfo ();
		if(armorTypeInfo)
		{
			if(armorTypeInfo.activeInHierarchy)
				HideArmorInfo();
			else
				ShowArmorInfo();
		}
	}

	public void ShowAttackInfo()
	{
		if(attackTypeInfo)
		{
			attackTypeInfo.SetActive(true);
		}
	}

	public void ShowArmorInfo()
	{
		HideAttackInfo ();
		if (armorTypeInfo) 
		{
			armorTypeInfo.SetActive(true);
		}
	}

	public void HideAttackInfo()
	{
		if(attackTypeInfo)
		{
			attackTypeInfo.SetActive(false);
		}
	}

	public void HideArmorInfo()
	{
		if (armorTypeInfo) 
		{
			armorTypeInfo.SetActive(false);
		}
	}


	void Update()
	{
//		if (load) {
//		
//			BuildInfoPanel bip = GetComponent<BuildInfoPanel> ();
//			soilderName = bip.soilderName;
//			buildCorn = bip.buildCorn;
//			buildTime = bip.buildTime;
//			health= bip.health;
//			damage= bip.damage;
//			attackType= bip.attackType;
//			attackSpeed= bip.attackSpeed;
//			attackRange= bip.attackRange;
//			armor= bip.armor;
//			armorType= bip.armorType;
//			corn= bip.corn;//突破奖励
//			skillInfo= bip.skillInfo;
//			upgradeBtn= bip.upgradeBtn;
//			upgradeBtn1= bip.upgradeBtn1;
//			levelPrefabPoints= bip.levelPrefabPoints;
//			arrows= bip.arrows;
//			prefabPoint= bip.prefabPoint;
//			load = false;
//		}
	}

}


