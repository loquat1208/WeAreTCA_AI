public class PlayerModel : UnitModel{
    public int Power = 1;
    public Status Status { get { return status; } set { status = value; } }

    // TODO : ZenjectかInterfaceの依存性注入でnewを消したい
    private UnitModel model = new UnitModel();
    private Status status;

    public PlayerModel()
    {
        status = model.Status;

        status.Power = Power;
    }
}