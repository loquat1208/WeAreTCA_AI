public class DummyWorld {
    public EnemyModel EnemyModel { get; set; }
    public PlayerModel PlayerModel { get; set; }

    void SetUp()
    {
        EnemyModel = new EnemyModel(1);
        PlayerModel = new PlayerModel();
    }
}
