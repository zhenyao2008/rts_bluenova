using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Commander {
        public static string csvFilePath = "Configs/Commander";
        public static string[] columnNameArray = new string[9];
        public static List<Commander> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Commander> dataList = new List<Commander>();
            columnNameArray = new string[9];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Commander data = new Commander();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                int.TryParse(csvFile.mapData[i].data[2],out data.starLv);
                columnNameArray [2] = "starLv";
                data.rank = csvFile.mapData[i].data[3];
                columnNameArray [3] = "rank";
                int.TryParse(csvFile.mapData[i].data[4],out data.tactics);
                columnNameArray [4] = "tactics";
                int.TryParse(csvFile.mapData[i].data[5],out data.active);
                columnNameArray [5] = "active";
                int.TryParse(csvFile.mapData[i].data[6],out data.passive1);
                columnNameArray [6] = "passive1";
                int.TryParse(csvFile.mapData[i].data[7],out data.passive2);
                columnNameArray [7] = "passive2";
                data.info = csvFile.mapData[i].data[8];
                columnNameArray [8] = "info";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Commander GetByID (int id,List<Commander> data)
        {
            foreach (Commander item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public string name;//名字
        public int starLv;//星级
        public string rank;//军衔
        public int tactics;//战术
        public int active;//主动技能
        public int passive1;//被动技能1
        public int passive2;//被动技能2
        public string info;//背景
    }
}
