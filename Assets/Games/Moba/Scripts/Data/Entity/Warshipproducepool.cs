using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Warshipproducepool {
        public static string csvFilePath = "Configs/Warshipproducepool";
        public static string[] columnNameArray = new string[4];
        public static List<Warshipproducepool> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Warshipproducepool> dataList = new List<Warshipproducepool>();
            columnNameArray = new string[4];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Warshipproducepool data = new Warshipproducepool();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.productPoolId);
                columnNameArray [1] = "productPoolId";
                int.TryParse(csvFile.mapData[i].data[2],out data.shipId);
                columnNameArray [2] = "shipId";
                float.TryParse(csvFile.mapData[i].data[3],out data.successPercent);
                columnNameArray [3] = "successPercent";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Warshipproducepool GetByID (int id,List<Warshipproducepool> data)
        {
            foreach (Warshipproducepool item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public int productPoolId;//生产池编号
        public int shipId;//生产战舰编号
        public float successPercent;//生产战舰权重
    }
}
