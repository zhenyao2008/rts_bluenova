//デッバグしやすいために、int から　enum に改善する。
//Editorが完成した後は、ストリングに変更必要があるかもしれない。
public enum FiniteStateConstant
{
    StandBy = 1,
    Walk = 2,
    Run = 3,
    Death = 4,
    Attack = 5
}

public enum FiniteConditionConstant
{
    StandBy = 1,
    Walk = 2,
    Run = 3,
    Death = 4,
    Attack = 5
}
