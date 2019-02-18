using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class GreateBuildingSprite : MonoBehaviour {

	public int sizeX = 200;
	public int sizeY = 250;

	public int row = 2;
	public float paddingX = 10;
	public int column = 4;
	public float paddingY = 10;

	public Vector3 offset = new Vector3 (0,0,0);

	public Vector3 backSize = new Vector3 (100,100);
	public Vector3 frantSize = new Vector3 (100, 100);
	public Vector3 colliderSize = new Vector3 (100,100,0);
	public Vector3 labelPos = new Vector3(0,0,0);
	public Vector3 infoBtnPos = new Vector3(0,0,0);

	public BuildingPanel bp;
	public BuildingItemUI itemPrefab;

	public bool load;

	void Update()
	{
		if(load)
		{
			load = false;
			Load();
		}
	}

	void Load()
	{
		if(bp==null)
			bp = FindObjectOfType<BuildingPanel>();
		List<UIEventTrigger> buildings = new List<UIEventTrigger> ();
		BuildingItemUI[] itemPrefabs = GetComponentsInChildren<BuildingItemUI>();
		List<BuildingItemUI> items = new List<BuildingItemUI> ();
		foreach(BuildingItemUI bi in itemPrefabs)
		{
			DestroyImmediate(bi.gameObject);
		}
		for(int i = 0;i<row;i++)
		{
			for(int j = 0;j<column;j++)
			{
				GameObject go = Instantiate(itemPrefab.gameObject) as GameObject;
				go.transform.parent = transform;
				go.name = "Item_" + i + "_" + j;
				go.transform.localPosition = offset + new Vector3((j-column/2+0.5f) * (sizeX + paddingX) , (row/2 - i  - 0.5f) * (sizeY + paddingY));
				go.transform.localScale = Vector3.one;
				if(go.GetComponent<BoxCollider>()){
					go.GetComponent<BoxCollider>().size = colliderSize;
				}
				BuildingItemUI item = go.GetComponent<BuildingItemUI>();
				if(item)
				{
					if(item.buildingName)
					{
						item.buildingName.transform.localPosition = labelPos;
					}
					if(item.detailTrigger)
					{
						item.detailTrigger.transform.localPosition = infoBtnPos;
					}
					if(item.backSprite)
					{
						item.backSprite.width = (int)backSize.x;
						item.backSprite.height = (int)backSize.y;
					}
					if(item.frantSprite)
					{
						item.frantSprite.width = (int)frantSize.x;
						item.frantSprite.height = (int)frantSize.y;
					}
				}
				items.Add(item);
				buildings.Add(go.GetComponent<UIEventTrigger>());
			}
		}
		bp.buildingItems = items;
		bp.buildings = buildings;

	}

}
