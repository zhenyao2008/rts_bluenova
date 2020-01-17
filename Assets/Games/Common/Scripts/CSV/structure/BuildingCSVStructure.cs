using CSV;

namespace BlueNoah.CSV
{
    public class BuildingCSVStructure : BaseCSVStructure
    {

        [CsvColumn(CanBeNull = true)]
        public string building_name { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int building_cost { get; set; }

    }
}