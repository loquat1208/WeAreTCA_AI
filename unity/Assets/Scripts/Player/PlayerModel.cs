namespace AI.Player
{
    public class PlayerModel
    {
        private const int power = 100;
        private const int maxHp = 100;
        private const int maxMp = 100;

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
