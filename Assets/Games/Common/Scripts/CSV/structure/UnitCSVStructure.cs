using CSV;

namespace BlueNoah.CSV
{
    public class UnitCSVStructure : BaseCSVStructure
    {

        [CsvColumn(CanBeNull = true)]
        public string resourceName;

        [CsvColumn(CanBeNull = true)]
        public int buildCorn;

        [CsvColumn(CanBeNull = true)]
        public string unitName;

        [CsvColumn(CanBeNull = true)]
        public int buildDuration;

        [CsvColumn(CanBeNull = true)]
        public int minDamage;

        [CsvColumn(CanBeNull = true)]
        public int maxDamage;

        [CsvColumn(CanBeNull = true)]
        public string attackType;

        [CsvColumn(CanBeNull = true)]
        public int attackInterval;

        [CsvColumn(CanBeNull = true)]
        public int attackRange;

        [CsvColumn(CanBeNull = true)]
        public bool isMelee;

        [CsvColumn(CanBeNull = true)]
        public int baseHealth;

        [CsvColumn(CanBeNull = true)]
        public int armor;

        [CsvColumn(CanBeNull = true)]
        public string armorType;

        [CsvColumn(CanBeNull = true)]
        public string skillInfo;

        [CsvColumn(CanBeNull = true)]
        public int killPrice;

        [CsvColumn(CanBeNull = true)]
        public int healthRecover;

        [CsvColumn(CanBeNull = true)]
        public int mana;

        [CsvColumn(CanBeNull = true)]
        public int manaRecover;

        [CsvColumn(CanBeNull = true)]
        public int maxHealth;

        [CsvColumn(CanBeNull = true)]
        public int killExp;

        [CsvColumn(CanBeNull = true)]
        public int levelUpExp;

        [CsvColumn(CanBeNull = true)]
        public int corn;

    }
}
