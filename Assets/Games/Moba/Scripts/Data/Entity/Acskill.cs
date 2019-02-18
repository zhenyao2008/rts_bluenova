using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Acskill {
        public static string csvFilePath = "Configs/Acskill";
        public static string[] columnNameArray = new string[16];
        public static List<Acskill> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Acskill> dataList = new List<Acskill>();
            columnNameArray = new string[16];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Acskill data = new Acskill();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.type = csvFile.mapData[i].data[1];
                columnNameArray [1] = "type";
                int.TryParse(csvFile.mapData[i].data[2],out data.hitRate);
                columnNameArray [2] = "hitRate";
                int.TryParse(csvFile.mapData[i].data[3],out data.cdTime);
                columnNameArray [3] = "cdTime";
                data.buff1 = csvFile.mapData[i].data[4];
                columnNameArray [4] = "buff1";
                int.TryParse(csvFile.mapData[i].data[5],out data.buff1ID);
                columnNameArray [5] = "buff1ID";
                int.TryParse(csvFile.mapData[i].data[6],out data.buff1Rate);
                columnNameArray [6] = "buff1Rate";
                data.buff2 = csvFile.mapData[i].data[7];
                columnNameArray [7] = "buff2";
                int.TryParse(csvFile.mapData[i].data[8],out data.buff2ID);
                columnNameArray [8] = "buff2ID";
                int.TryParse(csvFile.mapData[i].data[9],out data.buff2Rate);
                columnNameArray [9] = "buff2Rate";
                data.buff3 = csvFile.mapData[i].data[10];
                columnNameArray [10] = "buff3";
                int.TryParse(csvFile.mapData[i].data[11],out data.buff3ID);
                columnNameArray [11] = "buff3ID";
                int.TryParse(csvFile.mapData[i].data[12],out data.buff3Rate);
                columnNameArray [12] = "buff3Rate";
                data.buff4 = csvFile.mapData[i].data[13];
                columnNameArray [13] = "buff4";
                int.TryParse(csvFile.mapData[i].data[14],out data.buff4ID);
                columnNameArray [14] = "buff4ID";
                int.TryParse(csvFile.mapData[i].data[15],out data.buff4Rate);
                columnNameArray [15] = "buff4Rate";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Acskill GetByID (int id,List<Acskill> data)
        {
            foreach (Acskill item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//技能编号
        public string type;//攻击类型
        public int hitRate;//基础命中率/万分比
        public int cdTime;//冷却时间/毫秒
        public string buff1;//附带BUFF1
        public int buff1ID;//附带BUFF1编号
        public int buff1Rate;//生效几率
        public string buff2;//附带BUFF2
        public int buff2ID;//附带BUFF2编号
        public int buff2Rate;//生效几率
        public string buff3;//附带BUFF3
        public int buff3ID;//附带BUFF3编号
        public int buff3Rate;//生效几率
        public string buff4;//附带BUFF4
        public int buff4ID;//附带BUFF4编号
        public int buff4Rate;//生效几率
    }
}
