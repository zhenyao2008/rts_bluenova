using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Passive {
        public static string csvFilePath = "Configs/Passive";
        public static string[] columnNameArray = new string[11];
        public static List<Passive> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Passive> dataList = new List<Passive>();
            columnNameArray = new string[11];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Passive data = new Passive();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                data.skillinfo = csvFile.mapData[i].data[2];
                columnNameArray [2] = "skillinfo";
                data.parameter1 = csvFile.mapData[i].data[3];
                columnNameArray [3] = "parameter1";
                data.para1Info = csvFile.mapData[i].data[4];
                columnNameArray [4] = "para1Info";
                int.TryParse(csvFile.mapData[i].data[5],out data.para1Value1);
                columnNameArray [5] = "para1Value1";
                int.TryParse(csvFile.mapData[i].data[6],out data.para1Value2);
                columnNameArray [6] = "para1Value2";
                data.parameter2 = csvFile.mapData[i].data[7];
                columnNameArray [7] = "parameter2";
                data.para2Info = csvFile.mapData[i].data[8];
                columnNameArray [8] = "para2Info";
                int.TryParse(csvFile.mapData[i].data[9],out data.para2Value1);
                columnNameArray [9] = "para2Value1";
                int.TryParse(csvFile.mapData[i].data[10],out data.para2Value2);
                columnNameArray [10] = "para2Value2";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Passive GetByID (int id,List<Passive> data)
        {
            foreach (Passive item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public string name;//名称
        public string skillinfo;//技能说明
        public string parameter1;//效果1字段
        public string para1Info;//效果1说明
        public int para1Value1;//效果1数值1/万分比
        public int para1Value2;//效果1数值2
        public string parameter2;//效果2字段
        public string para2Info;//效果2说明
        public int para2Value1;//效果2数值1/万分比
        public int para2Value2;//效果2数值2
    }
}
