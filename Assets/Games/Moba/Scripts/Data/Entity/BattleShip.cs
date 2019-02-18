using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class BattleShip {
        public static string csvFilePath = "Configs/BattleShip";
        public static string[] columnNameArray = new string[17];
        public static List<BattleShip> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<BattleShip> dataList = new List<BattleShip>();
            columnNameArray = new string[17];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                BattleShip data = new BattleShip();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.group);
                columnNameArray [1] = "group";
                data.shipName = csvFile.mapData[i].data[2];
                columnNameArray [2] = "shipName";
                data.prefabName = csvFile.mapData[i].data[3];
                columnNameArray [3] = "prefabName";
                int.TryParse(csvFile.mapData[i].data[4],out data.shipType);
                columnNameArray [4] = "shipType";
                int.TryParse(csvFile.mapData[i].data[5],out data.column);
                columnNameArray [5] = "column";
                int.TryParse(csvFile.mapData[i].data[6],out data.row);
                columnNameArray [6] = "row";
                int.TryParse(csvFile.mapData[i].data[7],out data.duradle);
                columnNameArray [7] = "duradle";
                int.TryParse(csvFile.mapData[i].data[8],out data.armor);
                columnNameArray [8] = "armor";
                int.TryParse(csvFile.mapData[i].data[9],out data.torpedoArmor);
                columnNameArray [9] = "torpedoArmor";
                int.TryParse(csvFile.mapData[i].data[10],out data.artilleryAc);
                columnNameArray [10] = "artilleryAc";
                int.TryParse(csvFile.mapData[i].data[11],out data.torpedoAc);
                columnNameArray [11] = "torpedoAc";
                int.TryParse(csvFile.mapData[i].data[12],out data.airDef);
                columnNameArray [12] = "airDef";
                int.TryParse(csvFile.mapData[i].data[13],out data.precision);
                columnNameArray [13] = "precision";
                int.TryParse(csvFile.mapData[i].data[14],out data.maneuver);
                columnNameArray [14] = "maneuver";
                int.TryParse(csvFile.mapData[i].data[15],out data.investigation);
                columnNameArray [15] = "investigation";
                int.TryParse(csvFile.mapData[i].data[16],out data.airControl);
                columnNameArray [16] = "airControl";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static BattleShip GetByID (int id,List<BattleShip> data)
        {
            foreach (BattleShip item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public int group;//编组
        public string shipName;//名称
        public string prefabName;//资源名称
        public int shipType;//战舰类型
        public int column;//列号
        public int row;//行号
        public int duradle;//耐久
        public int armor;//装甲
        public int torpedoArmor;//鱼雷装甲
        public int artilleryAc;//火力
        public int torpedoAc;//鱼雷
        public int airDef;//防空
        public int precision;//精确
        public int maneuver;//机动
        public int investigation;//侦查
        public int airControl;//制空
    }
}
