public class EnemyModel {
    public Status Status { get { return status; } set { status = value; } } 

    // TODO : ZenjectかInterfaceの依存性注入でnewを消したい
    private UnitModel model = new UnitModel();
    private Status status;

    public EnemyModel(int power)
    {
        status = model.Status;

        status.Power = power;
    }
}