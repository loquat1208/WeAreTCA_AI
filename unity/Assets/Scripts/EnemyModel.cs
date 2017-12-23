using UnityEngine;

namespace AI.Enemy
{
    public class EnemyModel
    {
        // NOTE: 性格
        public enum TENDENCY
        {
            TIMID,  // 臆病
            BLAVE,  // 勇気
            VULGAR, // 卑怯
        }

        public int Power { get; set; }
        public int Speed { get; set; }
        public int Hp { get; set; }
        public float SearchLength { get; set; }
        public TENDENCY Tendency { get; set; }
        public Vector3 Pos { get; set; }

        public EnemyModel(int power, int speed, int hp, float searchLength, TENDENCY tendency)
        {
            Power = power;
            Speed = speed;
            Hp = hp;
            SearchLength = searchLength;
            Tendency = tendency;
        }
    }
}
