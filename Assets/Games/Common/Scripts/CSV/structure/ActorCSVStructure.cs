using CSV;

namespace BlueNoah.CSV
{
    public class ActorCSVStructure : BaseCSVStructure
    {
        [CsvColumn(CanBeNull = true)]
        public string name { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int action { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int move_speed { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int size_x { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int size_y { get; set; }

        [CsvColumn(CanBeNull = true)]
        public string layers { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int is_building { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int is_wall { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int wall_height { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int is_stair { get; set; }

        [CsvColumn(CanBeNull = true)]
        public int stair_direct { get; set; }

        [CsvColumn(CanBeNull = true)]
        public string resource_path { get; set; }


        public int[] LayerInt
        {
            get
            {
                string[] subLayers = layers.Split('|');
                int[] subLayerInts = new int[subLayers.Length];
                for (int i = 0; i < subLayers.Length; i++)
                {
                    int layer;
                    if (int.TryParse(subLayers[i], out layer))
                    {
                        subLayerInts[i] = layer;
                    }
                }
                return subLayerInts;
            }
        }
    }
}