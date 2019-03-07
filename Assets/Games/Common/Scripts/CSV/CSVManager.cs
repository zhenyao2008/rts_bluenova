using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSV;
using System.IO;

namespace BlueNoah.CSV
{
    public class CSVManager : SingleMonoBehaviour<CSVManager>
    {
        const string CSV_MAP_MONSTER_ROOT = "monsters/";
        const string CSV_UNIT = "m_unit_1";
        const string CSV_BUILDING = "m_building";
        const string CSV_LANGUAGE = "m_localization";
        private bool mLoaded = false;
        private CsvContext mCsvContext;
        public List<TextAsset> monsterTextAssetList;
        public List<MapMonster> monsterList;
        public Dictionary<int, MapMonster> monsterDic;
        public List<KeyValueCSVStructure> languageList;
        public Dictionary<string, string> languageDic;
        public List<BuildingCSVStructure> buildingList;
        public Dictionary<int, BuildingCSVStructure> buildingDic;
        public List<UnitCSVStructure> unitList;
        public Dictionary<int, UnitCSVStructure> unitDic;
        public List<GeneralCSVStructure> ConventionList { get; private set; }
        public Dictionary<int, GeneralCSVStructure> ConventionDic { get; private set; }
        public List<GeneralCSVStructure> NgList { get; private set; }
        public Dictionary<int, GeneralCSVStructure> NgDic { get; private set; }

        protected override void Awake()
        {
            StartLoading();
        }

        byte[] GetCSV(string fileName)
        {
#if UNITY_EDITOR
            return Resources.Load<TextAsset>("CSV/" + fileName).bytes;
#else
		return ResourcesManager.Instance.GetCSV (fileName);
#endif
        }

        void StartLoading()
        {
            mCsvContext = new CsvContext();
            LoadAllMonsterConfigs();
            LoadLanguage();
            LoadUnit();
            //LoadBuilding();
            mLoaded = true;
        }

        void LoadUnit()
        {
            unitList = CreateCSVList<UnitCSVStructure>(CSV_UNIT);
            unitDic = GetDictionary<UnitCSVStructure>(unitList);
        }

        void LoadBuilding()
        {
            buildingList = CreateCSVList<BuildingCSVStructure>(CSV_BUILDING);
            buildingDic = GetDictionary<BuildingCSVStructure>(buildingList);
        }

        void LoadLanguage()
        {
            languageList = CreateCSVList<KeyValueCSVStructure>(CSV_LANGUAGE);
            languageDic = new Dictionary<string, string>();
            for (int i = 0; i < languageList.Count; i++)
            {
                if (!languageDic.ContainsKey(languageList[i].key))
                {
                    languageDic.Add(languageList[i].key, languageList[i].value1);
                }
            }
        }

        void LoadAllMonsterConfigs()
        {
            monsterTextAssetList = new List<TextAsset>();
            TextAsset[] textAssets = Resources.LoadAll<TextAsset>("CSV/" + CSV_MAP_MONSTER_ROOT);
            for (int i = 0; i < textAssets.Length; i++)
            {
                monsterTextAssetList.Add(textAssets[i]);
            }
        }

        public void SetMonsterCSV(int index)
        {
            if (monsterTextAssetList == null || monsterTextAssetList.Count == 0)
            {
                LoadAllMonsterConfigs();
            }
            index = Mathf.Clamp(index, 0, monsterTextAssetList.Count);
            monsterList = CreateCSVList<MapMonster>(monsterTextAssetList[index].text);
            monsterDic = GetDictionary<MapMonster>(monsterList);
        }

        public List<T> CreateCSVList<T>(string csvname) where T : BaseCSVStructure, new()
        {
            var stream = new MemoryStream(GetCSV(csvname));
            var reader = new StreamReader(stream);
            IEnumerable<T> list = mCsvContext.Read<T>(reader);
            return new List<T>(list);
        }

        Dictionary<int, T> GetDictionary<T>(List<T> list) where T : BaseCSVStructure
        {
            Dictionary<int, T> dic = new Dictionary<int, T>();
            foreach (T t in list)
            {
                if (!dic.ContainsKey(t.id))
                    dic.Add(t.id, t);
                else
                    Debug.Log(string.Format("Multi key:{0}{1}", typeof(T).ToString(), t.id).YellowColor());
            }
            return dic;
        }

        public BuildingCSVStructure GetBuildingById(int id)
        {
            BuildingCSVStructure building = new BuildingCSVStructure();
            building.id = id;
            building.building_name = "building_name:" + id;
            building.building_cost = id * 100;
            return building;
        }
    }
}