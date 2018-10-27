using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrame
{
    public class BuildingDetailCtrl : BaseCtrl
    {
        BuildingDetailPanelView mBuildingDetailPanelView;

        public override void ShowPanel(Hashtable parameters)
        {
            base.ShowPanel(parameters);
            bool isCreate;
            mBuildingDetailPanelView = UIMgr.ShowPanel<BuildingDetailPanelView>(UIManager.UILayerType.Common, out isCreate);
            mBuildingDetailPanelView.root.SetActive(true);
            SetBuildInfo((SpawnPoint)parameters["data"]);
        }

        public void SetBuildInfo(SpawnPoint sp)
        {
            mBuildingDetailPanelView.btn_sell.onClick.RemoveAllListeners();
            mBuildingDetailPanelView.btn_sell.onClick.AddListener(()=>{
                PlayerController_III.instance.CmdDeleteBuilding();
                this.Close();
            });


            mBuildingDetailPanelView.btn_upgrade.gameObject.SetActive(false);
            mBuildingDetailPanelView.btn_upgrade1.gameObject.SetActive(false);
            mBuildingDetailPanelView.btn_close.onClick.AddListener(Close);
            UnitAttribute ua = sp.GetCurrentPrefab().GetComponent<UnitAttribute>();
            LevelPrefabs nextPrefabs;
            if (sp.GetNextPrefabs(out nextPrefabs))
            {
                if (nextPrefabs.soilderPrefabs.Count > 0)
                {
                    mBuildingDetailPanelView.btn_upgrade.onClick.RemoveAllListeners();
                    mBuildingDetailPanelView.btn_upgrade.gameObject.SetActive(true);
                    mBuildingDetailPanelView.btn_upgrade.onClick.AddListener(()=>{
                        PlayerController_III.instance.ShowUpgrade();
                        this.Close();
                    });
                    int unitId = nextPrefabs.soilderPrefabs[0].GetComponent<UnitAttribute>().unitId;
                    Debug.Log(unitId);
                    string unitName = CSVManager.Instance.languageDic["UNIT_NAME_" + unitId];
                    mBuildingDetailPanelView.btn_upgrade.GetComponentInChildren<Text>().text = "To:" +  unitName;
                }
                if (nextPrefabs.soilderPrefabs.Count > 1)
                {
                    mBuildingDetailPanelView.btn_upgrade1.onClick.RemoveAllListeners();
                    mBuildingDetailPanelView.btn_upgrade1.gameObject.SetActive(true);
                    mBuildingDetailPanelView.btn_upgrade1.onClick.AddListener(()=>{
                        PlayerController_III.instance.ShowUpgrade1();
                        Close();
                    });
                    int unitId = nextPrefabs.soilderPrefabs[0].GetComponent<UnitAttribute>().unitId;
                    string unitName = CSVManager.Instance.languageDic["UNIT_NAME_" + unitId];
                    mBuildingDetailPanelView.btn_upgrade1.GetComponentInChildren<Text>().text = "To:" + unitName;
                }
            }

            mBuildingDetailPanelView.txt_soild_name.text = ua.unitName;
            mBuildingDetailPanelView.txt_build_corn.text = ua.buildCorn.ToString();
            mBuildingDetailPanelView.txt_build_time.text = ua.buildDuration.ToString() + "s";
            mBuildingDetailPanelView.txt_health.text = ua.baseHealth.ToString();
            mBuildingDetailPanelView.txt_damage.text = ua.minDamage + "-" + ua.maxDamage;
            switch (ua.attackType)
            {
                case AttackType.Normal:
                    mBuildingDetailPanelView.txt_attack_type.text = "普通"; break;
                case AttackType.Puncture:
                    mBuildingDetailPanelView.txt_attack_type.text = "穿刺"; break;
                case AttackType.Magic:
                    mBuildingDetailPanelView.txt_attack_type.text = "魔法"; break;
                case AttackType.Siege:
                    mBuildingDetailPanelView.txt_attack_type.text = "攻城"; break;
                case AttackType.Chaos:
                    mBuildingDetailPanelView.txt_attack_type.text = "混乱"; break;
            }
            mBuildingDetailPanelView.txt_attack_speed.text = ua.attackInterval + "s/次";
            mBuildingDetailPanelView.txt_attack_range.text = ua.attackRange + "/" + (ua.isMelee ? "近战" : "远程");
            mBuildingDetailPanelView.txt_armor.text = ua.armor.ToString();
            switch (ua.armorType)
            {
                case ArmorType.None:
                    mBuildingDetailPanelView.txt_armor_type.text = "无甲"; break;
                case ArmorType.Light:
                    mBuildingDetailPanelView.txt_armor_type.text = "轻甲"; break;
                case ArmorType.Middle:
                    mBuildingDetailPanelView.txt_armor_type.text = "中甲"; break;
                case ArmorType.Heavy:
                    mBuildingDetailPanelView.txt_armor_type.text = "重甲"; break;
                case ArmorType.Construction:
                    mBuildingDetailPanelView.txt_armor_type.text = "建筑"; break;
            }
            mBuildingDetailPanelView.txt_corn.text = ua.killPrice.ToString();

            int currentUnitId = ua.unitId;
            string skillInfo = CSVManager.Instance.languageDic["UNIT_SKILL_" + currentUnitId];
            mBuildingDetailPanelView.txt_skill_info.text = skillInfo;
        }

        public override void Close()
        {
            base.Close();
            mBuildingDetailPanelView.Close();
        }
    }
}