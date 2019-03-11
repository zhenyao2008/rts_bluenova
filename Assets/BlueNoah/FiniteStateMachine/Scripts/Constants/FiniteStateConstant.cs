//デッバグしやすいために、int から　enum に改善する。
//Editorが完成した後は、ストリングに変更必要があるかもしれない。
public enum FiniteStateConstant
{
    StandBy = 0,
    Walk = 1,
    Run = 2,
    Death = 3,
    Attack = 4
}

public enum FiniteConditionConstant
{
    StandBy = 0,
    Walk = 1,
    Run = 2,
    Death = 3,
    Attack = 4
}
