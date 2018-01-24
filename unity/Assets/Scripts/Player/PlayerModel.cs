namespace AI.Unit.Player
{
    public class PlayerModel
    {
        public readonly float MaxHp = 100f;
        public readonly float MaxMp = 100f;
        public readonly double MpRecoveryTime = 10;
        public readonly int Power = 10;
        public readonly float Speed = 25 * 0.2f;
        public readonly int RotateSpeed = 100;
        public float Hp { get; set; }
        public float Mp { get; set; }

        public PlayerModel()
        {
            Hp = MaxHp;
            Mp = MaxMp;
        }
    }
}
