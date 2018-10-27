using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSV;
using System.IO;


public class CSVManager :SingleMonoBehaviour<CSVManager>
{
	private bool loaded = false;
	private CsvContext mCsvContext;

    public List<KeyValueCSVStructure> languageList;
    public Dictionary<string, string> languageDic;

    public List<BuildingCSVStructure> buildingList;
    public Dictionary<int, BuildingCSVStructure> buildingDic;


	public List<GeneralCSVStructure> ConventionList { get; private set; }
	public Dictionary<int, GeneralCSVStructure> ConventionDic { get; private set; }
	public List<GeneralCSVStructure> NgList { get; private set; }
	public Dictionary<int, GeneralCSVStructure> NgDic { get; private set; }

	protected override void Awake ()
	{
		StartLoading ();
	}

	byte[] GetCSV (string fileName)
	{
		#if UNITY_EDITOR
		return Resources.Load<TextAsset> ("CSV/" + fileName).bytes;
		#else
		return ResourcesManager.Instance.GetCSV (fileName);
		#endif
	}

	void StartLoading ()
	{
		mCsvContext = new CsvContext ();
        //		LoadNG ();
        LoadLanguage();
		loaded = true;
	}

    void LoadBuilding(){
        buildingList = CreateCSVList<BuildingCSVStructure>("m_building");
        buildingDic = GetDictionary<BuildingCSVStructure>(buildingList);
    }

    void LoadLanguage(){
        languageList = CreateCSVList<KeyValueCSVStructure>("m_localization");
        languageDic = new Dictionary<string, string>();
        for (int i = 0; i < languageList.Count; i++)
        {
            if (!languageDic.ContainsKey(languageList[i].key))
            {
                languageDic.Add(languageList[i].key, languageList[i].value1);
            }
        }
    }

	public List<T> CreateCSVList<T> (string csvname) where T:BaseCSVStructure, new()
	{
		var stream = new MemoryStream (GetCSV (csvname));
		var reader = new StreamReader (stream);
		IEnumerable<T> list = mCsvContext.Read<T> (reader);
		return new List<T> (list);
	}

	Dictionary<int,T> GetDictionary<T> (List<T> list) where T : BaseCSVStructure
	{
		Dictionary<int,T> dic = new Dictionary<int, T> ();
		foreach (T t in list) {
			if (!dic.ContainsKey (t.id))
				dic.Add (t.id, t);
			else
				Debug.Log (string.Format ("Multi key:{0}{1}", typeof(T).ToString (), t.id).YellowColor ());
		}
		return dic;
	}


	public BuildingCSVStructure GetBuildingById(int id){
		BuildingCSVStructure building = new BuildingCSVStructure ();
		building.id = id;
		building.building_name = "building_name:" + id;
		building.building_cost = id * 100;
		return building;

	}

}

