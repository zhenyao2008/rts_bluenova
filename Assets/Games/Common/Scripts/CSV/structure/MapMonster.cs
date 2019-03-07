using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSV;

namespace BlueNoah.CSV
{
    [System.Serializable]
    public class MapMonster : BaseCSVStructure
    {
        [CsvColumn()]
        public string name;
        [CsvColumn()]
        public int unit_id;
        [CsvColumn()]
        public int action;
        [CsvColumn()]
        public float pos_x;
        [CsvColumn()]
        public float pos_y;
        [CsvColumn()]
        public float angle_y;
        [CsvColumn()]
        public int alignment;
    }
}
