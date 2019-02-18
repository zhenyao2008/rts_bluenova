using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.UI
{
    public class UnitUIManager : SingleMonoBehaviour<UnitUIManager>
    {

        GameObject mUnitArrowUI;

        Canvas mUnitUICanvas;

        GameObject mUnitUI;

		protected override void Awake()
		{
            base.Awake();
            mUnitArrowUI = ResourcesManager.Instance.GetUnitArrowUI();
            mUnitUICanvas = GameObject.Find("UnitUICanvas").GetComponent<Canvas>();
            mUnitUI = ResourcesManager.Instance.GetUnitUI();
		}

        public GameObject CreateUnitUI(){
            return Instantiate(mUnitUI);
        }

        public void CreateUnitArrowUI(UnitBase unitBase){
            GameObject go = Instantiate(mUnitArrowUI);
            go.transform.SetParent(mUnitUICanvas.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            UnitUI unitUI =  go.AddMissingComponent<UnitUI>();
            unitUI.SetUnit(unitBase.gameObject);
            unitBase.onDeadAction = () => {
                Destroy(go);
            };
        }
		
    }
}

