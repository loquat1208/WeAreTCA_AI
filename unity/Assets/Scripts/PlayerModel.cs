using UnityEngine;

namespace AI.Player
{
    public class PlayerModel
    {
        public int Power { get; set; }
        public int Speed { get; set; }
        public int Hp { get; set; }
        public Vector3 Pos { get; set; }

        public PlayerModel(int power, int speed, int hp)
        {
            Power = power;
            Speed = speed;
            Hp = hp;
        }
    }
}
