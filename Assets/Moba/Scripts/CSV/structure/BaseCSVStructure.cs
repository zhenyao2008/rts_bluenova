using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSV;
[System.Serializable]
public class BaseCSVStructure  {

	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

}
