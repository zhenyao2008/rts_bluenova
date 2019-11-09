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
        public int pos_x;
        [CsvColumn()]
        public int pos_y;
        [CsvColumn()]
        public int angle_y;
        [CsvColumn()]
        public int alignment;
        [CsvColumn()]
        public int size_x;
        [CsvColumn()]
        public int size_y;
        [CsvColumn()]
        public int is_building;
        [CsvColumn()]
        public string layers;
        [CsvColumn()]
        public int move_speed;

        public int[] LayerInt {
            get
            {
                string[] subLayers = layers.Split('|');
                int[] subLayerInts = new int[subLayers.Length];
                for (int i=0;i<subLayers.Length;i++)
                {
                    int layer;
                    if (int.TryParse(subLayers[i],out layer))
                    {
                        subLayerInts[i] = layer;
                    }
                }
                return subLayerInts;
            }
        }
    }
}
