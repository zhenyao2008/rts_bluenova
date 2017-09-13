using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Level {
        public static string csvFilePath = "Configs/Level";
        public static string[] columnNameArray = new string[14];
        public static List<Level> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Level> dataList = new List<Level>();
            columnNameArray = new string[14];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Level data = new Level();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                int.TryParse(csvFile.mapData[i].data[2],out data.LevelID);
                columnNameArray [2] = "LevelID";
                data.LevelName = csvFile.mapData[i].data[3];
                columnNameArray [3] = "LevelName";
                int.TryParse(csvFile.mapData[i].data[4],out data.Type);
                columnNameArray [4] = "Type";
                int.TryParse(csvFile.mapData[i].data[5],out data.Power);
                columnNameArray [5] = "Power";
                int.TryParse(csvFile.mapData[i].data[6],out data.Count);
                columnNameArray [6] = "Count";
                int.TryParse(csvFile.mapData[i].data[7],out data.Active);
                columnNameArray [7] = "Active";
                int.TryParse(csvFile.mapData[i].data[8],out data.Access);
                columnNameArray [8] = "Access";
                int.TryParse(csvFile.mapData[i].data[9],out data.Last);
                columnNameArray [9] = "Last";
                int.TryParse(csvFile.mapData[i].data[10],out data.Next);
                columnNameArray [10] = "Next";
                int.TryParse(csvFile.mapData[i].data[11],out data.Enemy);
                columnNameArray [11] = "Enemy";
                int.TryParse(csvFile.mapData[i].data[12],out data.AwardExp);
                columnNameArray [12] = "AwardExp";
                int.TryParse(csvFile.mapData[i].data[13],out data.Gold);
                columnNameArray [13] = "Gold";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Level GetByID (int id,List<Level> data)
        {
            foreach (Level item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//区域编号
        public string name;//区域名称
        public int LevelID;//关卡编号
        public string LevelName;//关卡名称
        public int Type;//关卡类型
        public int Power;//消耗体力
        public int Count;//每日次数
        public int Active;//激活条件
        public int Access;//进入条件
        public int Last;//前置关卡
        public int Next;//后续关卡
        public int Enemy;//关卡敌人配置
        public int AwardExp;//奖励经验
        public int Gold;//奖励金币
    }
}
