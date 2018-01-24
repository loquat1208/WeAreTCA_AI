namespace AI.Unit.Player
{
    public class PlayerModel
    {
        private const int maxHp = 100;
        private const int maxMp = 100;

        public readonly double MpRecoveryTime = 10;
        public readonly int Power = 10;
        public readonly int Speed = 10;
        public int Hp { get; set; }
        public int Mp { get; set; }

        public PlayerModel()
        {
            Hp = maxHp;
            Mp = maxMp;
        }
    }
}
