using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class Item {
        public static string csvFilePath = "Configs/Item";
        public static string[] columnNameArray = new string[14];
        public static List<Item> LoadDatas(){
            CSVFileReader csvFile = new CSVFileReader();
            csvFile.Open (csvFilePath);
            List<Item> dataList = new List<Item>();
//            string[] strs;
//            string[] strsTwo;
//            List<int> listChild;
            columnNameArray = new string[14];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                Item data = new Item();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                data.name = csvFile.mapData[i].data[1];
                columnNameArray [1] = "name";
                data.icon = csvFile.mapData[i].data[2];
                columnNameArray [2] = "icon";
                int.TryParse(csvFile.mapData[i].data[3],out data.type);
                columnNameArray [3] = "type";
                int.TryParse(csvFile.mapData[i].data[4],out data.quality);
                columnNameArray [4] = "quality";
                data.property = csvFile.mapData[i].data[5];
                columnNameArray [5] = "property";
                int.TryParse(csvFile.mapData[i].data[6],out data.propertyParam);
                columnNameArray [6] = "propertyParam";
                int.TryParse(csvFile.mapData[i].data[7],out data.priceOne);
                columnNameArray [7] = "priceOne";
                int.TryParse(csvFile.mapData[i].data[8],out data.priceTwo);
                columnNameArray [8] = "priceTwo";
                int.TryParse(csvFile.mapData[i].data[9],out data.priceThree);
                columnNameArray [9] = "priceThree";
                int.TryParse(csvFile.mapData[i].data[10],out data.priceFour);
                columnNameArray [10] = "priceFour";
                int.TryParse(csvFile.mapData[i].data[11],out data.sellPrice);
                columnNameArray [11] = "sellPrice";
                data.dest = csvFile.mapData[i].data[12];
                columnNameArray [12] = "dest";
                int.TryParse(csvFile.mapData[i].data[13],out data.itemGroupId);
                columnNameArray [13] = "itemGroupId";
                dataList.Add(data);
            }
            return dataList;
        }
  
        public static Item GetByID (int id,List<Item> data)
        {
            foreach (Item item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
        public int id;//物品编号
        public string name;//物品名称
        public string icon;//物品图标
        public int type;//物品类型
        public int quality;//物品品质
        public string property;//属性字段
        public int propertyParam;//属性参数
        public int priceOne;//购买价格1
        public int priceTwo;//购买价格2
        public int priceThree;//购买价格3
        public int priceFour;//购买价格4
        public int sellPrice;//出售价格
        public string dest;//背景介绍
        public int itemGroupId;//物品组编号
    }
}
