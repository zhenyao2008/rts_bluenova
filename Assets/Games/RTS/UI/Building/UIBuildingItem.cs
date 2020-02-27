using BlueNoah.CSV;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BlueNoah.RTS.UI
{
    public class UIBuildingItem : MonoBehaviour {

        public Button btnBuilding;

        public Text txtBuilding;

        public ActorCSVStructure buildingCSVStructure;

        public UnityAction<ActorCSVStructure> onClick;

        private void Awake()
        {
            btnBuilding.onClick.AddListener(() => {
                if(onClick!=null && buildingCSVStructure!=null)
                    onClick(buildingCSVStructure);
            });
        }

        public void SetData(ActorCSVStructure buildingCSVStructure,UnityAction<ActorCSVStructure> onClick)
        {
            this.buildingCSVStructure = buildingCSVStructure;
            this.onClick = onClick;
            txtBuilding.text = buildingCSVStructure.name;
        }
    }
}