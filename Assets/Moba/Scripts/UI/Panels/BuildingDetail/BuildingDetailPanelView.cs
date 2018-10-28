using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrame
{
	public class BuildingDetailPanelView : PanelBase {

		public GridLayoutGroup grid_unit_detail;
		public Button btn_upgrade;
        public Text txt_upgrade;
        public Text txt_upgrade_price;
		public Button btn_upgrade1;
        public Text txt_upgrade1;
        public Text txt_upgrade1_price;
        public Button btn_sell;
        public Text txt_soild_name;
        public Text txt_build_corn;
        public Text txt_build_time;
        public Text txt_health;
        public Text txt_damage;
        public Text txt_attack_type;

        public Text txt_attack_speed;
        public Text txt_attack_range;
        public Text txt_armor;

        public Text txt_armor_type;
        public Text txt_corn;
        public Text txt_skill_info;

		public override void Awake()
		{
            //grid_unit_detail = transform.Find ("Root/grid_unit_detail").GetComponent<GridLayoutGroup>();
            btn_upgrade = transform.Find ("Root/btn_upgrade").GetComponent<Button>();
            txt_upgrade= transform.Find("Root/btn_upgrade/txt_upgrade").GetComponent<Text>();
            txt_upgrade_price= transform.Find("Root/btn_upgrade/txt_upgrade_price").GetComponent<Text>();
            btn_upgrade1 = transform.Find ("Root/btn_upgrade1").GetComponent<Button> ();
            txt_upgrade1 = transform.Find("Root/btn_upgrade1/txt_upgrade").GetComponent<Text>();
            txt_upgrade1_price = transform.Find("Root/btn_upgrade1/txt_upgrade_price").GetComponent<Text>();
            btn_sell = transform.Find("Root/btn_sell").GetComponent<Button>();
            txt_soild_name = transform.Find("Root/container_detail/item/txt_soild_name").GetComponent<Text>(); 
            txt_build_corn = transform.Find("Root/container_detail/item/txt_build_corn").GetComponent<Text>();
            txt_build_time = transform.Find("Root/container_detail/item/txt_build_time").GetComponent<Text>();
            txt_health = transform.Find("Root/container_detail/item/txt_health").GetComponent<Text>();
            txt_damage = transform.Find("Root/container_detail/item/txt_damage").GetComponent<Text>();
            txt_attack_type = transform.Find("Root/container_detail/item/txt_attack_type").GetComponent<Text>();
            txt_attack_speed = transform.Find("Root/container_detail/item/txt_attack_speed").GetComponent<Text>();
            txt_attack_range = transform.Find("Root/container_detail/item/txt_attack_range").GetComponent<Text>();
            txt_armor = transform.Find("Root/container_detail/item/txt_armor").GetComponent<Text>();
            txt_armor_type = transform.Find("Root/container_detail/item/txt_armor_type").GetComponent<Text>();
            txt_corn = transform.Find("Root/container_detail/item/txt_corn").GetComponent<Text>();
            txt_skill_info = transform.Find("Root/container_detail/item/txt_skill_info").GetComponent<Text>();
		}

		void OnDestroy()
		{
			
		}

	}
}
