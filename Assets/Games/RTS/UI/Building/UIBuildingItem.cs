using BlueNoah.CSV;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BlueNoah.RTS.UI
{
    public class UIBuildingItem : MonoBehaviour {

        public Button btnBuilding;

        public Text txtBuilding;

        public BuildingCSVStructure buildingCSVStructure;

        public UnityAction<BuildingCSVStructure> onClick;

        private void Awake()
        {
            btnBuilding.onClick.AddListener(() => {
                if(onClick!=null && buildingCSVStructure!=null)
                    onClick(buildingCSVStructure);
            });
        }

        public void SetData(BuildingCSVStructure buildingCSVStructure,UnityAction<BuildingCSVStructure> onClick)
        {
            this.buildingCSVStructure = buildingCSVStructure;
            this.onClick = onClick;
            txtBuilding.text = buildingCSVStructure.name;
        }
    }
}