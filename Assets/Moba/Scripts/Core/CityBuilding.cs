using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityBuilding : MonoBehaviour {

	public int level;
	public UnitProperties unitProperties;

	//进化序列，这个进化跟刀塔里面进化很像，需要相关的材料才行
	public List<LevelPrefabs> leveledSoilderPrefabs;

	public List<UnitProperties> level0;//1个
	public List<UnitProperties> level1;//0 - 2个
	public List<UnitProperties> level2;//0 - 4个

	public List<UnitProperties> availLevel0;
	public List<UnitProperties> availLevel1;
	public List<UnitProperties> availLevel2;

	public Renderer buildingRenderer;
	public Material defaultMat;
	public Material disableMat;

	public GameObject placeEffectPrefab;
	public GameObject arrawTips;

	public string buildingName;
	public string buildingDetail;

	public GameObject availUnits;
	public List<CityUnitGroupIndex> cityUnitGroups;

	public bool isResouceBuilding;
	public int resouceType;//0 or 1


	public GameObject buildingUIPrefab;
	public BuildingUI buildingUI;

//	CityBuildingAttribute mCityBuildingAttribute;

	AudioSource mAudioSource;
	BuildingRes mBuildingRes;

	void Awake(){
		mBuildingRes = GetComponent<BuildingRes> ();
//		mCityBuildingAttribute = GetComponent<CityBuildingAttribute> ();
		mAudioSource = GetComponent<AudioSource> ();
		defaultMat = buildingRenderer.material;
		Shader shader = Shader.Find("Standard");
		disableMat = new Material (defaultMat);
		disableMat.shader = shader;
		disableMat.SetColor ("_Color",Color.red);
		TweenScale tweenScale = GetComponent<TweenScale> ();
		if(tweenScale)
		{
			tweenScale.onFinished.Add(new EventDelegate(tweenScale.PlayReverse));
		}
	}

	public void ShowDisable()
	{
		buildingRenderer.enabled = true;
		buildingRenderer.material = disableMat;
	}

	public void ShowDefault()
	{
//		buildingRenderer.material = defaultMat;
		buildingRenderer.enabled = false;
	}

	public void Select()
	{
//		Debug.Log ("Select");
		arrawTips.SetActive (true);
		if(mBuildingRes.selectAudioClip && mAudioSource)
		{
			mAudioSource.clip = mBuildingRes.selectAudioClip;
			mAudioSource.Play();
		}
		TweenScale tweenScale = GetComponent<TweenScale> ();
		if(tweenScale)
		{
			tweenScale.PlayForward();
		}
	}

	public void DeSelect()
	{
		arrawTips.SetActive (false);
	}

	public void Place()
	{
		Debug.Log ("Place");
		if(mBuildingRes.placeAudioClip && mAudioSource)
		{
			mAudioSource.clip = mBuildingRes.placeAudioClip;
			mAudioSource.Play();
		}
		if(placeEffectPrefab)
		{
			GameObject effect = Instantiate(placeEffectPrefab,transform.position,transform.rotation) as GameObject;
			Destroy(effect,3);
		}
		TweenScale tweenScale = GetComponent<TweenScale> ();
		if(tweenScale)
		{
			tweenScale.PlayForward();
		}
		arrawTips.SetActive (false);
		if (buildingUIPrefab) {
			GameObject go = Instantiate (buildingUIPrefab) as GameObject;
			buildingUI = go.GetComponent<BuildingUI>();
		}
		StartCoroutine (_Active());
	}

	IEnumerator _Active(){
		yield return new WaitForSeconds (0.5f);
		GetComponent<Collider> ().enabled = true;
	}

	public int buildingSize = 5;
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 pos0 =transform.position +transform.forward * buildingSize + transform.right * buildingSize;
		Gizmos.DrawSphere (pos0,1);
		pos0 = transform.position + transform.forward * buildingSize - transform.right * buildingSize;
		Gizmos.DrawSphere (pos0,1);
		pos0 = transform.position - transform.forward * buildingSize - transform.right * buildingSize;
		Gizmos.DrawSphere (pos0,1);
		pos0 = transform.position - transform.forward * buildingSize + transform.right * buildingSize;
		Gizmos.DrawSphere (pos0,1);

	}

}

public class CityUnitGroupIndex
{
	public List<int> group;
}

