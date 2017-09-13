using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LocalEffectManager : MonoBehaviour {

	public List<GameObject> ringPrefabs;//可以根据服务器端发过来的预制的index来在客户端生成对应的物体。

	public GameObject buildingUpgradePrefab;
	public GameObject buildingSpawnPrefab;


	//省略版单例
	public static LocalEffectManager SingleTon(){
		return instance;
	}

	static LocalEffectManager instance;

	void Awake()
	{
		instance = this;
	}

	//心灵之火buff特效
	public GameObject xinLinZhiHuoPrefab;



}
