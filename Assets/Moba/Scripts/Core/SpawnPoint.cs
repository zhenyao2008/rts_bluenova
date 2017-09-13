using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint: NetworkBehaviour {

	public List<LevelPrefabs> leveledSoilderPrefabs;
	public List<UnitRenderer> playerRenderers;


	public string buildingName;

	public Renderer mainRenderer;
	public Material selectedMat;
	public Material defaultMat;

	public int level;
	public int order;

	public Transform spawnPoint;
	[SyncVar]
	public int group;
	public int index;

	public bool showBuildingSpawn = true;

	public AudioClip startAudioClip;
	public AudioClip upgradingAudioClip;
	public AudioClip upgradeAudioClip;
	public AudioClip placeAudioClip;

	void Start(){
		if(NetworkServer.active)
		{
			RpcSetClientLayer(gameObject.layer,index);
			RpcSetClientGroup(group);

		}
		if (NetworkClient.active) {
			selectedMat = new Material (selectedMat);
			if (showBuildingSpawn && LocalEffectManager.SingleTon ().buildingSpawnPrefab != null) {
				Instantiate (LocalEffectManager.SingleTon ().buildingSpawnPrefab, transform.position, Quaternion.identity);
			}
		}
	}

	[ClientRpc]
	public void RpcSetClientGroup(int groupId){
		if(playerRenderers!=null && playerRenderers.Count>0)
		{
			foreach(UnitRenderer ur in playerRenderers)
			{
				if(groupId == 0)
				{
					ur.renderer.materials = ur.mats0;
				}
				else
				{
					ur.renderer.materials = ur.mats1;
				}

			}
		}
		if(groupId == 0)
		{
//			transform.eulerAngles = transform.eulerAngles + new Vector3(0,180,0);
		}
		else
		{
			transform.eulerAngles = transform.eulerAngles + new Vector3(0,180,0);
		}
		if(startAudioClip!=null && GetComponent<AudioSource>())
		{
			AudioSource audioSource = GetComponent<AudioSource>();
			audioSource.volume =1;
			audioSource.spatialBlend =1;
			audioSource.maxDistance = 150;
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.clip = placeAudioClip;
			audioSource.Play();
		}	
	}
	
	[Client]
	public void OnSelected(){
		mainRenderer.material = selectedMat;
		
		if (startAudioClip) {
			GetComponent<AudioSource> ().clip = startAudioClip;
			GetComponent<AudioSource>().Play();
		}	

	}

	[Client]
	public void OnUnSelected(){
		mainRenderer.material = defaultMat;
	}

	[Server]
	public void Upgrade(){
		level ++;
		RpcUpgrade (level);
	}

	[ClientRpc]
	public void RpcUpgrade(int lvl){
		this.level = lvl;
		Material mat = defaultMat;
		Material mat0 = new Material(Shader.Find("Standard"));
		mat0.SetTexture("_MainTex",mat.GetTexture("_MainTex"));
		mat0.SetTexture("_BumpMap",mat.GetTexture("_BumpMap"));
		if(level == 1)
		{
			mat0.SetColor("_Color",Color.red);
			selectedMat.SetColor("_Color",Color.red);
		}
		else if(level == 2)
		{
			mat0.SetColor("_Color",Color.blue);
			selectedMat.SetColor("_Color",Color.blue);
		}
		mainRenderer.material = mat0;
		defaultMat = mat0;

		GameObject prefab = LocalEffectManager.SingleTon ().buildingUpgradePrefab;
		GameObject go = Instantiate (prefab, transform.position, Quaternion.identity) as GameObject;
		go.transform.localScale = Vector3.one * 3;


		if (upgradeAudioClip != null) {
			AudioSource audioSource = GetComponent<AudioSource>();
			audioSource.clip = upgradeAudioClip;
			audioSource.Play();
		}

	}


	[ClientRpc]
	public void RpcSetClientLayer(int layer,int index){
		gameObject.layer = layer;
		this.index = index;
	}

	public bool GetNextPrefabs(out LevelPrefabs prefabs){
		if(leveledSoilderPrefabs.Count<=level+1)
		{
			prefabs = null;
			return false;
		}
		prefabs = leveledSoilderPrefabs [level + 1];
		return true;
	}

	//获取当前制造的士兵
	public GameObject GetCurrentPrefab(){
		LevelPrefabs lp = leveledSoilderPrefabs [level];
		return lp.soilderPrefabs[order];
	}

	public bool GetPrefab(int lvl,int odr,out GameObject prefabs){
		if(leveledSoilderPrefabs.Count <= lvl)
		{
			prefabs = null;
			return false;
		}
		LevelPrefabs lp = leveledSoilderPrefabs [lvl];
		if(lp.soilderPrefabs.Count<=odr)
		{
			prefabs = null;
			return false;
		}
		prefabs = lp.soilderPrefabs [odr];
		return true;
	}

	//获取某个士兵的制造价格
	public int GetCurrentPrice(){
		return GetCurrentPrefab().GetComponent<UnitAttribute> ().buildCorn;
	}

	public bool GetCurrentPrice(int lvl,int odr,out int price){
		GameObject prefab;
		if (GetPrefab (lvl, odr,out prefab)) {
			price = prefab.GetComponent<UnitAttribute>().buildCorn;
			return true;
		}
		price = 0;
		return false;
	}

}

[System.Serializable]
public class LevelPrefabs
{
	public List<GameObject> soilderPrefabs;
}



