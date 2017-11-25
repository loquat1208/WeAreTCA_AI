public struct Status
{
    public int Power;
}

public class UnitModel {
    public Status Status;
}

public interface IUnit
{
    void Attack();
    void Escape();
}