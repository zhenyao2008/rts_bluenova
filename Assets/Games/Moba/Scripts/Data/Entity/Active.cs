using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Active {
        public static string csvFilePath = "Configs/Active";
        public static string[] columnNameArray = new string[8];
        public static List<Active> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Active> dataList = new List<Active>();
            columnNameArray = new string[8];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Active data = new Active();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                data.skillinfo = csvFile.mapData[i].data[2];
                columnNameArray [2] = "skillinfo";
                int.TryParse(csvFile.mapData[i].data[3],out data.cdTime);
                columnNameArray [3] = "cdTime";
                data.parameter = csvFile.mapData[i].data[4];
                columnNameArray [4] = "parameter";
                data.paraInfo = csvFile.mapData[i].data[5];
                columnNameArray [5] = "paraInfo";
                int.TryParse(csvFile.mapData[i].data[6],out data.value1);
                columnNameArray [6] = "value1";
                int.TryParse(csvFile.mapData[i].data[7],out data.value2);
                columnNameArray [7] = "value2";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Active GetByID (int id,List<Active> data)
        {
            foreach (Active item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public string name;//名称
        public string skillinfo;//技能说明
        public int cdTime;//冷却时间/毫秒
        public string parameter;//效果字段
        public string paraInfo;//效果说明
        public int value1;//效果数值1/万分比
        public int value2;//效果数值2
    }
}
