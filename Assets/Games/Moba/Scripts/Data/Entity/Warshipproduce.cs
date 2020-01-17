using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Warshipproduce {
        public static string csvFilePath = "Configs/Warshipproduce";
        public static string[] columnNameArray = new string[10];
        public static List<Warshipproduce> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Warshipproduce> dataList = new List<Warshipproduce>();
            columnNameArray = new string[10];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Warshipproduce data = new Warshipproduce();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                data.icon = csvFile.mapData[i].data[2];
                columnNameArray [2] = "icon";
                int.TryParse(csvFile.mapData[i].data[3],out data.shipType);
                columnNameArray [3] = "shipType";
                int.TryParse(csvFile.mapData[i].data[4],out data.consumeResource1);
                columnNameArray [4] = "consumeResource1";
                int.TryParse(csvFile.mapData[i].data[5],out data.consumeResource2);
                columnNameArray [5] = "consumeResource2";
                int.TryParse(csvFile.mapData[i].data[6],out data.consumeResource3);
                columnNameArray [6] = "consumeResource3";
                int.TryParse(csvFile.mapData[i].data[7],out data.consumeResource4);
                columnNameArray [7] = "consumeResource4";
                int.TryParse(csvFile.mapData[i].data[8],out data.productPoolId);
                columnNameArray [8] = "productPoolId";
                int.TryParse(csvFile.mapData[i].data[9],out data.consumeTime);
                columnNameArray [9] = "consumeTime";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Warshipproduce GetByID (int id,List<Warshipproduce> data)
        {
            foreach (Warshipproduce item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//图纸编号
        public string name;//图纸名称
        public string icon;//图纸图标
        public int shipType;//战舰类型
        public int consumeResource1;//消耗钢材数量
        public int consumeResource2;//消耗铝材数量
        public int consumeResource3;//消耗弹药数量
        public int consumeResource4;//消耗燃料数量
        public int productPoolId;//生产池编号
        public int consumeTime;//生产时间/分
    }
}
