using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Unit_Info {
        public static string csvFilePath = "Configs/Unit_Info";
        public static string[] columnNameArray = new string[5];
        public static List<Unit_Info> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Unit_Info> dataList = new List<Unit_Info>();
//            string[] strs;
//            string[] strsTwo;
//            List<int> listChild;
            columnNameArray = new string[5];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Unit_Info data = new Unit_Info();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.u_name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "u_name";
                int.TryParse(csvFile.mapData[i].data[2],out data.u_group);
                columnNameArray [2] = "u_group";
                int.TryParse(csvFile.mapData[i].data[3],out data.u_level);
                columnNameArray [3] = "u_level";
                data.u_prefab = csvFile.mapData[i].data[4];
                columnNameArray [4] = "u_prefab";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Unit_Info GetByID (int id,List<Unit_Info> data)
        {
            foreach (Unit_Info item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//
        public string u_name;//
        public int u_group;//
        public int u_level;//
        public string u_prefab;//
    }
}
