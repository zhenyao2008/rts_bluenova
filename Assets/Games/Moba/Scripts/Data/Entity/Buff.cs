using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Buff {
        public static string csvFilePath = "Configs/Buff";
        public static string[] columnNameArray = new string[9];
        public static List<Buff> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Buff> dataList = new List<Buff>();
            columnNameArray = new string[9];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Buff data = new Buff();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                data.info = csvFile.mapData[i].data[2];
                columnNameArray [2] = "info";
                data.parameter = csvFile.mapData[i].data[3];
                columnNameArray [3] = "parameter";
                data.paraInfo = csvFile.mapData[i].data[4];
                columnNameArray [4] = "paraInfo";
                int.TryParse(csvFile.mapData[i].data[5],out data.value1);
                columnNameArray [5] = "value1";
                int.TryParse(csvFile.mapData[i].data[6],out data.value2);
                columnNameArray [6] = "value2";
                int.TryParse(csvFile.mapData[i].data[7],out data.time);
                columnNameArray [7] = "time";
                data.tips = csvFile.mapData[i].data[8];
                columnNameArray [8] = "tips";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Buff GetByID (int id,List<Buff> data)
        {
            foreach (Buff item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public string name;//BUFF名称
        public string info;//效果说明
        public string parameter;//效果字段
        public string paraInfo;//字段说明
        public int value1;//效果数值1/万分比
        public int value2;//效果数值2
        public int time;//持续时间
        public string tips;//提示信息
    }
}
