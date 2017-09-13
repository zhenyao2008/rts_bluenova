using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildInfoPanel : MonoBehaviour {

	public GameObject root;
	public UILabel soilderName;
	public UILabel buildCorn;
	public UILabel buildTime;
	public UILabel health;
	public UILabel damage;

	public UILabel attackType;
	public UILabel attackSpeed;
	public UILabel attackRange;
	public UILabel armor;
	public UILabel armorType;
	public UILabel corn;//突破奖励
	public UILabel skillInfo;

	public UIButton closeBtn;
	public UIButton upgradeBtn;
	public UIButton upgradeBtn1;

	public UIButton returnBtn;

	public List<Transform> levelPrefabPoints;
	public List<GameObject> arrows;
	public Vector3 levelPrefabScale = new Vector3 (50, 50, 50);
	public Transform prefabPoint;
	GameObject mCurrentPrefab;

	List<GameObject> preLevelPrefabs = new List<GameObject>();

	void Start(){
		closeBtn.onClick.Add (new EventDelegate(Close));
	}

	void Close(){
		root.SetActive (false);
	}

	public void ShowUpgrade(SpawnPoint spawnPoint){
		List<LevelPrefabs> levelPrefabs = spawnPoint.leveledSoilderPrefabs;

		foreach (GameObject go in preLevelPrefabs) {
			Destroy(go);
		}
		preLevelPrefabs.Clear ();

		foreach(GameObject go in arrows)
		{
			go.SetActive(false);
		}


		if(levelPrefabs.Count > 0)
		{
			List<GameObject> prefabs = levelPrefabs[0].soilderPrefabs;
			GameObject go = Instantiate(prefabs[0]) as GameObject;
			go.transform.parent = levelPrefabPoints[0];
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = levelPrefabScale;
			go.GetComponent<UnitBase>().anim.wrapMode = WrapMode.Loop;
			if(go.GetComponent<UnitBase>().isFlying)
			{
				go.transform.localPosition = new Vector3(0,-200,200);
			}


			Destroy(go.GetComponent<UnitBase>());
			Destroy(go.GetComponent<UnityEngine.AI.NavMeshAgent>());
			preLevelPrefabs.Add(go);
		}
		if(levelPrefabs.Count > 1)
		{
			List<GameObject> prefabs = levelPrefabs[1].soilderPrefabs;
			GameObject go = Instantiate(prefabs[0]) as GameObject;
			go.transform.parent = levelPrefabPoints[2];
			go.transform.localPosition = Vector3.zero;
			if(go.GetComponent<UnitBase>().isFlying)
			{
				go.transform.localPosition = new Vector3(0,-200,200);
			}

			go.transform.localEulerAngles =Vector3.zero;
			go.transform.localScale = levelPrefabScale;
			arrows[0].gameObject.SetActive(true);
			go.GetComponent<UnitBase>().anim.wrapMode = WrapMode.Loop;
			Destroy(go.GetComponent<UnitBase>());
			Destroy(go.GetComponent<UnityEngine.AI.NavMeshAgent>());
			preLevelPrefabs.Add(go);
			if(prefabs.Count > 1)
			{
				go = Instantiate(prefabs[1]) as GameObject;
				go.transform.parent = levelPrefabPoints[3];
				go.transform.localPosition = Vector3.zero;
				if(go.GetComponent<UnitBase>().isFlying)
				{
					go.transform.localPosition = new Vector3(0,-200,200);
				}

				go.transform.localEulerAngles = Vector3.zero;
				go.transform.localScale = levelPrefabScale;
				arrows[1].gameObject.SetActive(true);
				go.GetComponent<UnitBase>().anim.wrapMode = WrapMode.Loop;
				Destroy(go.GetComponent<UnitBase>());
				Destroy(go.GetComponent<UnityEngine.AI.NavMeshAgent>());
				preLevelPrefabs.Add(go);
			}
		}
		if (levelPrefabs.Count > 2) {
			List<GameObject> prefabs = levelPrefabs[2].soilderPrefabs;
			GameObject go = Instantiate(prefabs[0]) as GameObject;
			go.transform.parent = levelPrefabPoints[4];
			go.transform.localPosition = Vector3.zero;
			if(go.GetComponent<UnitBase>().isFlying)
			{
				go.transform.localPosition = new Vector3(0,-200,200);
			}

			go.transform.localEulerAngles =Vector3.zero;
			go.transform.localScale = levelPrefabScale;
			arrows[3].gameObject.SetActive(true);
			go.GetComponent<UnitBase>().anim.wrapMode = WrapMode.Loop;
			Destroy(go.GetComponent<UnitBase>());
			Destroy(go.GetComponent<UnityEngine.AI.NavMeshAgent>());
			preLevelPrefabs.Add(go);
			if(prefabs.Count > 1)
			{
				go = Instantiate(prefabs[1]) as GameObject;
				go.transform.parent = levelPrefabPoints[5];
				go.transform.localPosition = Vector3.zero;
				if(go.GetComponent<UnitBase>().isFlying)
				{
					go.transform.localPosition = new Vector3(0,-200,200);
				}

				go.transform.localEulerAngles = Vector3.zero;
				go.transform.localScale = levelPrefabScale;
				go.GetComponent<UnitBase>().anim.wrapMode = WrapMode.Loop;
				Destroy(go.GetComponent<UnitBase>());
				Destroy(go.GetComponent<UnityEngine.AI.NavMeshAgent>());
				preLevelPrefabs.Add(go);
				if(levelPrefabs[1].soilderPrefabs.Count==1)
				{
					arrows[4].gameObject.SetActive(true);
				}
				else
				{
					arrows[5].gameObject.SetActive(true);
				}
			}
		}
	}

	public void SetBuildInfo(UnitAttribute ua)
	{
		upgradeBtn.gameObject.SetActive(false);
		upgradeBtn1.gameObject.SetActive(false);
		this.soilderName.text = ua.unitName;
		this.buildCorn.text = ua.buildCorn.ToString();
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
		go.GetComponentInChildren<Animation> ().wrapMode = WrapMode.Loop;
		go.GetComponentInChildren<Animation> ().Play (go.GetComponent<Enemy> ().idleAnimStateName);
//		go.GetComponent<UnitBase> ().enabled = false;
		go.GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
		go.transform.parent = prefabPoint;
		go.transform.localPosition = Vector3.zero;
		if(go.GetComponent<UnitBase>().isFlying)
		{
			go.transform.localPosition = new Vector3(0,-420,0);
		}
		go.transform.localEulerAngles = new Vector3 (0,-25,0);
		go.transform.localScale = new Vector3 (100,100,100);
		go.layer = 5;
		mCurrentPrefab = go;
		Destroy (go.GetComponent<UnitBase>());
	}


}
