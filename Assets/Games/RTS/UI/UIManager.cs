using System;
using System.Collections.Generic;
using BlueNoah.CSV;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BlueNoah.RTS.UI
{
    public class UIManager : SingleMonoBehaviour<UIManager>
    {

        public Button btnBuilding;

        public GameObject containerBuildingList;

        public GameObject btnBuidlingItem;

        public UnityAction<ActorCSVStructure> onSelectBuildingItem;

        protected override void Awake()
        {
            base.Awake();
            try {
                btnBuilding.onClick.AddListener(() =>
                {
                    ToggleBuildingList();
                });
                InitBuildingList();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        void InitBuildingList()
        {
            List<ActorCSVStructure> buildings = CSVManager.Instance.actorList;
            for (int i=0;i<buildings.Count;i++)
            {
                GameObject item = GameObject.Instantiate<GameObject>(btnBuidlingItem);
                item.GetComponent<UIBuildingItem>().SetData(buildings[i],SelectBuildingItem);
                item.transform.SetParent(btnBuidlingItem.transform.parent);
                item.SetActive(true);
            }
        }

        void SelectBuildingItem(ActorCSVStructure item)
        {
            try
            {
                if (onSelectBuildingItem!=null)
                {
                    onSelectBuildingItem(item);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        void ToggleBuildingList()
        {
            containerBuildingList.SetActive(!containerBuildingList.activeSelf);
        }

    }
}
