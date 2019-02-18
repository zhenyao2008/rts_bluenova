using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Warship {
        public static string csvFilePath = "Configs/Warship";
        public static string[] columnNameArray = new string[36];
        public static List<Warship> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Warship> dataList = new List<Warship>();
            columnNameArray = new string[36];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Warship data = new Warship();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.shipName = csvFile.mapData[i].data[1];
                columnNameArray [1] = "shipName";
                data.icon = csvFile.mapData[i].data[2];
                columnNameArray [2] = "icon";
                data.prefabName = csvFile.mapData[i].data[3];
                columnNameArray [3] = "prefabName";
                int.TryParse(csvFile.mapData[i].data[4],out data.country);
                columnNameArray [4] = "country";
                int.TryParse(csvFile.mapData[i].data[5],out data.shipType);
                columnNameArray [5] = "shipType";
                int.TryParse(csvFile.mapData[i].data[6],out data.quality);
                columnNameArray [6] = "quality";
                int.TryParse(csvFile.mapData[i].data[7],out data.level);
                columnNameArray [7] = "level";
                int.TryParse(csvFile.mapData[i].data[8],out data.starLev);
                columnNameArray [8] = "starLev";
                int.TryParse(csvFile.mapData[i].data[9],out data.equipTreeId);
                columnNameArray [9] = "equipTreeId";
                int.TryParse(csvFile.mapData[i].data[10],out data.duradle);
                columnNameArray [10] = "duradle";
                int.TryParse(csvFile.mapData[i].data[11],out data.armor);
                columnNameArray [11] = "armor";
                int.TryParse(csvFile.mapData[i].data[12],out data.torpedoArmor);
                columnNameArray [12] = "torpedoArmor";
                int.TryParse(csvFile.mapData[i].data[13],out data.artilleryAc);
                columnNameArray [13] = "artilleryAc";
                int.TryParse(csvFile.mapData[i].data[14],out data.torpedoAc);
                columnNameArray [14] = "torpedoAc";
                int.TryParse(csvFile.mapData[i].data[15],out data.airDef);
                columnNameArray [15] = "airDef";
                int.TryParse(csvFile.mapData[i].data[16],out data.precision);
                columnNameArray [16] = "precision";
                int.TryParse(csvFile.mapData[i].data[17],out data.maneuver);
                columnNameArray [17] = "maneuver";
                int.TryParse(csvFile.mapData[i].data[18],out data.investigation);
                columnNameArray [18] = "investigation";
                int.TryParse(csvFile.mapData[i].data[19],out data.airControl);
                columnNameArray [19] = "airControl";
                int.TryParse(csvFile.mapData[i].data[20],out data.duradleAdd);
                columnNameArray [20] = "duradleAdd";
                int.TryParse(csvFile.mapData[i].data[21],out data.armorAdd);
                columnNameArray [21] = "armorAdd";
                int.TryParse(csvFile.mapData[i].data[22],out data.torpedoArmorAdd);
                columnNameArray [22] = "torpedoArmorAdd";
                int.TryParse(csvFile.mapData[i].data[23],out data.artilleryAcAdd);
                columnNameArray [23] = "artilleryAcAdd";
                int.TryParse(csvFile.mapData[i].data[24],out data.torpedoAcAdd);
                columnNameArray [24] = "torpedoAcAdd";
                int.TryParse(csvFile.mapData[i].data[25],out data.airDefAdd);
                columnNameArray [25] = "airDefAdd";
                int.TryParse(csvFile.mapData[i].data[26],out data.precisionAdd);
                columnNameArray [26] = "precisionAdd";
                int.TryParse(csvFile.mapData[i].data[27],out data.maneuverAdd);
                columnNameArray [27] = "maneuverAdd";
                int.TryParse(csvFile.mapData[i].data[28],out data.investigationAdd);
                columnNameArray [28] = "investigationAdd";
                int.TryParse(csvFile.mapData[i].data[29],out data.airControlAdd);
                columnNameArray [29] = "airControlAdd";
                int.TryParse(csvFile.mapData[i].data[30],out data.itemId1);
                columnNameArray [30] = "itemId1";
                int.TryParse(csvFile.mapData[i].data[31],out data.itemId);
                columnNameArray [31] = "itemId";
                int.TryParse(csvFile.mapData[i].data[32],out data.itemId2);
                columnNameArray [32] = "itemId2";
                int.TryParse(csvFile.mapData[i].data[33],out data.itemId2Num);
                columnNameArray [33] = "itemId2Num";
                int.TryParse(csvFile.mapData[i].data[34],out data.itemId3);
                columnNameArray [34] = "itemId3";
                int.TryParse(csvFile.mapData[i].data[35],out data.itemId3Num);
                columnNameArray [35] = "itemId3Num";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Warship GetByID (int id,List<Warship> data)
        {
            foreach (Warship item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//战舰编号
        public string shipName;//战舰名称
        public string icon;//图标
        public string prefabName;//资源名称
        public int country;//所属国家
        public int shipType;//战舰类型
        public int quality;//战舰品质
        public int level;//战舰等级
        public int starLev;//战舰星级
        public int equipTreeId;//装备树编号
        public int duradle;//耐久
        public int armor;//装甲
        public int torpedoArmor;//鱼雷装甲
        public int artilleryAc;//火力
        public int torpedoAc;//鱼雷
        public int airDef;//防空
        public int precision;//精准
        public int maneuver;//机动
        public int investigation;//侦查
        public int airControl;//制空
        public int duradleAdd;//耐久加值
        public int armorAdd;//装甲加值
        public int torpedoArmorAdd;//鱼雷装甲加值
        public int artilleryAcAdd;//火力加值
        public int torpedoAcAdd;//鱼雷加值
        public int airDefAdd;//防空加值
        public int precisionAdd;//精准加值
        public int maneuverAdd;//机动加值
        public int investigationAdd;//侦查加值
        public int airControlAdd;//制空加值
        public int itemId1;//产出1编号
        public int itemId;//产出1数量
        public int itemId2;//产出2编号
        public int itemId2Num;//产出2数量
        public int itemId3;//产出3编号
        public int itemId3Num;//产出3数量
    }
}
