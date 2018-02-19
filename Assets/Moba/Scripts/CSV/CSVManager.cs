using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSV;
using System.IO;


public class CSVManager :SingleMonoBehaviour<CSVManager>
{
	private bool loaded = false;
	private const string CSV_CONVENTION = "m_convention";
	private const string CSV_NG = "m_ng";
	private const string CSV_QA = "m_qa";
	private const string CSV_HELP = "m_help";

	private const string CSV_SKILL = "m_skill";
	private CsvContext mCsvContext;
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
		loaded = true;
	}

	void LoadNG ()
	{
		NgList = CreateCSVList<GeneralCSVStructure> (CSV_NG);
		NgDic = GetDictionary (NgList);
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

