using System.Collections.Generic;

using AI.Behavior;

namespace AI.Unit.Enemy
{
    public class EnemyModel
    {
        private const int maxHp = 100;
        private const int maxMp = 100;

        public readonly double MpRecoveryTime = 10;
        public readonly float SearchAngle = 60f;
        public readonly float SearchLength = 10f;
        public readonly int Power = 1;
        public readonly int Speed = 1;
        public int Hp { get; set; }
        public int Mp { get; set; }

        public Skill.Type Skill;
        public Dictionary<int, AIModel> Behaviors = new Dictionary<int, AIModel>();

        public EnemyModel()
        {
            Skill = Behavior.Skill.Type.Dash;
            Hp = maxHp;
            Mp = maxMp;
            Behaviors.Add(0, new AIModel(AIModel.Subject.Enemy, AIModel.Criterion.Hp, 0, 100, AIModel.Behavior.Skill));
            Behaviors.Add(1, new AIModel(AIModel.Subject.Enemy, AIModel.Criterion.Mp, 0, 100, AIModel.Behavior.Skill));
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
