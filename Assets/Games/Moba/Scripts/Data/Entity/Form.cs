using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Form {
        public static string csvFilePath = "Configs/Form";
        public static string[] columnNameArray = new string[12];
        public static List<Form> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Form> dataList = new List<Form>();
            columnNameArray = new string[12];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Form data = new Form();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.formName = csvFile.mapData[i].data[1];
                columnNameArray [1] = "formName";
                data.pos1 = csvFile.mapData[i].data[2];
                columnNameArray [2] = "pos1";
                data.pos2 = csvFile.mapData[i].data[3];
                columnNameArray [3] = "pos2";
                data.pos3 = csvFile.mapData[i].data[4];
                columnNameArray [4] = "pos3";
                data.pos4 = csvFile.mapData[i].data[5];
                columnNameArray [5] = "pos4";
                data.pos5 = csvFile.mapData[i].data[6];
                columnNameArray [6] = "pos5";
                data.pos6 = csvFile.mapData[i].data[7];
                columnNameArray [7] = "pos6";
                data.pos7 = csvFile.mapData[i].data[8];
                columnNameArray [8] = "pos7";
                int.TryParse(csvFile.mapData[i].data[9],out data.fLv);
                columnNameArray [9] = "fLv";
                int.TryParse(csvFile.mapData[i].data[10],out data.mLv);
                columnNameArray [10] = "mLv";
                int.TryParse(csvFile.mapData[i].data[11],out data.bLv);
                columnNameArray [11] = "bLv";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Form GetByID (int id,List<Form> data)
        {
            foreach (Form item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//阵型编号
        public string formName;//名称
        public string pos1;//前部位置1
        public string pos2;//前部位置2
        public string pos3;//中部位置3
        public string pos4;//中部位置4
        public string pos5;//中部位置5
        public string pos6;//后部位置6
        public string pos7;//后部位置7
        public int fLv;//前部开启等级
        public int mLv;//中部开启等级
        public int bLv;//后部开启等级
    }
}
