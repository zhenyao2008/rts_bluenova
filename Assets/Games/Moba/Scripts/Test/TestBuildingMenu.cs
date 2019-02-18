using UnityEngine;
using System.Collections;

public class TestBuildingMenu : MonoBehaviour {

	public BuildingPanel buildingPanel;
	public BuildingPanel resourcePanel;
	public BuildingPanel warBuildingPanel;

	// Use this for initialization
	void Start () {
		buildingPanel = GetComponent<BuildingPanel> ();
		buildingPanel.buildingItems [0].buildingName.text = "资源";
		buildingPanel.buildingItems [0].backSprite.color = Color.gray;
		buildingPanel.buildingItems [1].buildingName.text = "军队";//军队和城镇卫兵
		buildingPanel.buildingItems [2].buildingName.text = "领袖";//阵型和策略
		buildingPanel.buildingItems [2].backSprite.color = Color.gray;
		//不同等级有不同的科技,不同种族有不同的卫兵
		buildingPanel.buildingItems [3].buildingName.text = "科技";
		buildingPanel.buildingItems [3].backSprite.color = Color.gray;
		buildingPanel.buildingItems [4].buildingName.text = "保护";
		buildingPanel.buildingItems [4].backSprite.color = Color.gray;
		buildingPanel.buildingItems [5].buildingName.text = "----";
		buildingPanel.buildingItems [5].backSprite.color = Color.gray;
		buildingPanel.buildingItems [6].buildingName.text = "----";
		buildingPanel.buildingItems [6].backSprite.color = Color.gray;
		buildingPanel.buildingItems [7].buildingName.text = "----";
		buildingPanel.buildingItems [7].backSprite.color = Color.gray;

		buildingPanel.buildingItems [0].GetComponent<UIEventTrigger> ().onClick.Add (new EventDelegate(ShowResourceBuilding));
		buildingPanel.buildingItems [1].GetComponent<UIEventTrigger> ().onClick.Add (new EventDelegate(ShowWarBuilding));
	}

	public void ShowResourceBuilding()
	{
		resourcePanel.Active();
		resourcePanel.preBuildingPanel = buildingPanel;
		buildingPanel.root.SetActive (false);
	}

	public void ShowWarBuilding()
	{
		warBuildingPanel.Active();
		warBuildingPanel.preBuildingPanel = buildingPanel;
		buildingPanel.root.SetActive (false);
	}

}
