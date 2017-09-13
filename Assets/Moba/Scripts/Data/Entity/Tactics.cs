using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Tactics {
        public static string csvFilePath = "Configs/Tactics";
        public static string[] columnNameArray = new string[9];
        public static List<Tactics> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Tactics> dataList = new List<Tactics>();
            columnNameArray = new string[9];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Tactics data = new Tactics();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                data.skillinfo = csvFile.mapData[i].data[2];
                columnNameArray [2] = "skillinfo";
                data.parameter = csvFile.mapData[i].data[3];
                columnNameArray [3] = "parameter";
                data.paraInfo = csvFile.mapData[i].data[4];
                columnNameArray [4] = "paraInfo";
                int.TryParse(csvFile.mapData[i].data[5],out data.value1);
                columnNameArray [5] = "value1";
                int.TryParse(csvFile.mapData[i].data[6],out data.value2);
                columnNameArray [6] = "value2";
                data.tip1 = csvFile.mapData[i].data[7];
                columnNameArray [7] = "tip1";
                data.tip2 = csvFile.mapData[i].data[8];
                columnNameArray [8] = "tip2";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Tactics GetByID (int id,List<Tactics> data)
        {
            foreach (Tactics item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public string name;//名称
        public string skillinfo;//技能说明
        public string parameter;//效果字段
        public string paraInfo;//效果说明
        public int value1;//效果数值1/万分比
        public int value2;//效果数值2
        public string tip1;//激活提示
        public string tip2;//效果提示
    }
}
