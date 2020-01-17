using CSV;

namespace BlueNoah.CSV
{
    [System.Serializable]
    public class KeyValueCSVStructure : BaseCSVStructure
    {

        [CsvColumn(CanBeNull = true)]
        public string key { get; set; }

        [CsvColumn(CanBeNull = true)]
        public string value { get; set; }

        [CsvColumn(CanBeNull = true)]
        public string value1 { get; set; }

        [CsvColumn(CanBeNull = true)]
        public string value2 { get; set; }

    }
}
