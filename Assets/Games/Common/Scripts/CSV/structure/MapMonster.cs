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
        public int pos_x;
        [CsvColumn()]
        public int pos_y;
        [CsvColumn()]
        public int angle_y;
        [CsvColumn()]
        public int alignment;

        ActorCSVStructure mActorCSVStructure;

        public ActorCSVStructure ActorCSVStructure
        {
            get
            {
                if (mActorCSVStructure==null)
                {
                    mActorCSVStructure = CSVManager.Instance.actorDic[unit_id];
                }
                return mActorCSVStructure;
            }
        }


    }
}
