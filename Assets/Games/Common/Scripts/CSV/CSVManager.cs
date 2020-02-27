using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSV;
using System.IO;

namespace BlueNoah.CSV
{
    public class CSVManager : SingleMonoBehaviour<CSVManager>
    {
        //Original Data
        const string CSV_UNIT = "m_unit_1";
        //Original Data
        const string CSV_ACTOR = "actors/m_actor";
        const string CSV_MAP_MONSTER_ROOT = "monsters/";
        const string CSV_MAP_BUILDING_ROOT = "buildings/";
        const string CSV_LANGUAGE = "m_localization";
        private bool mLoaded = false;
        private CsvContext mCsvContext;
        public List<TextAsset> monsterTextAssetList;
        public List<MapMonster> monsterList;
        public Dictionary<int, MapMonster> monsterDic;

        public List<TextAsset> mapBuildingTextAssetList;
        public List<MapMonster> mapBuildingList;
        public Dictionary<int, MapMonster> mapBuildingDic;

        public List<KeyValueCSVStructure> languageList;
        public Dictionary<string, string> languageDic;
        public List<ActorCSVStructure> actorList;
        public Dictionary<int, ActorCSVStructure> actorDic;
        //public List<UnitCSVStructure> unitList;
        //public Dictionary<int, UnitCSVStructure> unitDic;
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
            Debug.Log(fileName);
		    return ResourcesManager.Instance.GetCSV (fileName);
        }

        void StartLoading()
        {
            mCsvContext = new CsvContext();
            LoadAllMonsterConfigs();
            LoadAllBuildingConfigs();
            LoadLanguage();
            //LoadUnit();
            LoadActor();
            mLoaded = true;
        }

        //void LoadUnit()
        //{
        //    unitList = CreateCSVList<UnitCSVStructure>(CSV_UNIT);
        //    unitDic = GetDictionary<UnitCSVStructure>(unitList);
        //}

        void LoadActor()
        {
            actorList = CreateCSVList<ActorCSVStructure>(CSV_ACTOR);
            actorDic = GetDictionary<ActorCSVStructure>(actorList);
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
            Debug.Log("<color=yellow>Load stages from:" + "CSV/" + CSV_MAP_MONSTER_ROOT + "</color>");
            for (int i = 0; i < textAssets.Length; i++)
            {
                monsterTextAssetList.Add(textAssets[i]);
            }
        }

        void LoadAllBuildingConfigs()
        {
            mapBuildingTextAssetList = new List<TextAsset>();
            TextAsset[] textAssets = Resources.LoadAll<TextAsset>("CSV/" + CSV_MAP_BUILDING_ROOT);
            Debug.Log("<color=yellow>Load stages from:" + "CSV/" + CSV_MAP_BUILDING_ROOT + "</color>");
            for (int i = 0; i < textAssets.Length; i++)
            {
                mapBuildingTextAssetList.Add(textAssets[i]);
            }
        }

        public List<MapMonster> LoadMapMonsterCSV(int index)
        {
            if (monsterTextAssetList == null || monsterTextAssetList.Count == 0)
            {
                LoadAllMonsterConfigs();
            }
            index = Mathf.Clamp(index, 0, monsterTextAssetList.Count);
            monsterList = CreateCSVList<MapMonster>(monsterTextAssetList[index].bytes);
            monsterDic = GetDictionary<MapMonster>(monsterList);
            return monsterList;
        }

        public List<MapMonster> LoadMapBuildingCSV(int index)
        {
            if (mapBuildingTextAssetList == null || mapBuildingTextAssetList.Count == 0)
            {
                LoadAllBuildingConfigs();
            }
            index = Mathf.Clamp(index, 0, mapBuildingTextAssetList.Count);
            mapBuildingList = CreateCSVList<MapMonster>(mapBuildingTextAssetList[index].bytes);
            mapBuildingDic = GetDictionary<MapMonster>(mapBuildingList);
            return mapBuildingList;
        }

        public List<T> CreateCSVList<T>(byte[] bytes) where T : BaseCSVStructure, new()
        {
            var stream = new MemoryStream(bytes);
            var reader = new StreamReader(stream);
            IEnumerable<T> list = mCsvContext.Read<T>(reader);
            return new List<T>(list);
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

        public ActorCSVStructure GetBuildingData(int id)
        {
            if (actorDic.ContainsKey(id))
                return this.actorDic[id];
            else
                return null;
        }
    }
}