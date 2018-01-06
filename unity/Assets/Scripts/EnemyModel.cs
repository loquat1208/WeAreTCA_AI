using System.Collections.Generic;

using AI.Behavior;

namespace AI.Enemy
{
    public class EnemyModel
    {
        private const int power = 100;
        private const int maxHp = 100;
        private const int maxMp = 100;
        private const int speed = 1;
        private const int searchLength = 10;
        private const int searchAngle = 30;

        public int Hp { get; set; }
        public int Mp { get; set; }

        private Skill.Type Skill;
        private Dictionary<int, AIModel> Behaviors = new Dictionary<int, AIModel>();

        public EnemyModel()
        {
            Skill = Behavior.Skill.Type.None;
            Hp = maxHp;
            Mp = maxMp;
            Behaviors.Add(0, new AIModel());
        }

        public EnemyModel(Skill.Type skill, Dictionary<int, AIModel> behaviors)
        {
            Skill = skill;
            Hp = maxHp;
            Mp = maxMp;
            Behaviors = behaviors;
        }
    }
}
