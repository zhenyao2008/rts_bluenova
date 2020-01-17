using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Troops {
        public static string csvFilePath = "Configs/Troops";
        public static string[] columnNameArray = new string[15];
        public static List<Troops> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Troops> dataList = new List<Troops>();
            columnNameArray = new string[15];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Troops data = new Troops();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.ship1ID);
                columnNameArray [1] = "ship1ID";
                data.ship1pos = csvFile.mapData[i].data[2];
                columnNameArray [2] = "ship1pos";
                int.TryParse(csvFile.mapData[i].data[3],out data.ship2ID);
                columnNameArray [3] = "ship2ID";
                data.ship2pos = csvFile.mapData[i].data[4];
                columnNameArray [4] = "ship2pos";
                int.TryParse(csvFile.mapData[i].data[5],out data.ship3ID);
                columnNameArray [5] = "ship3ID";
                data.ship3pos = csvFile.mapData[i].data[6];
                columnNameArray [6] = "ship3pos";
                int.TryParse(csvFile.mapData[i].data[7],out data.ship4ID);
                columnNameArray [7] = "ship4ID";
                data.ship4pos = csvFile.mapData[i].data[8];
                columnNameArray [8] = "ship4pos";
                int.TryParse(csvFile.mapData[i].data[9],out data.ship5ID);
                columnNameArray [9] = "ship5ID";
                data.ship5pos = csvFile.mapData[i].data[10];
                columnNameArray [10] = "ship5pos";
                int.TryParse(csvFile.mapData[i].data[11],out data.ship6ID);
                columnNameArray [11] = "ship6ID";
                data.ship6pos = csvFile.mapData[i].data[12];
                columnNameArray [12] = "ship6pos";
                int.TryParse(csvFile.mapData[i].data[13],out data.ship7ID);
                columnNameArray [13] = "ship7ID";
                data.ship7pos = csvFile.mapData[i].data[14];
                columnNameArray [14] = "ship7pos";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Troops GetByID (int id,List<Troops> data)
        {
            foreach (Troops item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//编号
        public int ship1ID;//战舰1编号
        public string ship1pos;//战舰1位置
        public int ship2ID;//战舰2编号
        public string ship2pos;//战舰2位置
        public int ship3ID;//战舰3编号
        public string ship3pos;//战舰3位置
        public int ship4ID;//战舰4编号
        public string ship4pos;//战舰4位置
        public int ship5ID;//战舰5编号
        public string ship5pos;//战舰5位置
        public int ship6ID;//战舰6编号
        public string ship6pos;//战舰6位置
        public int ship7ID;//战舰7编号
        public string ship7pos;//战舰7位置
    }
}
