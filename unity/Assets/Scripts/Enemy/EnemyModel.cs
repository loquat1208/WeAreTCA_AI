using System.Collections.Generic;

using AI.Behavior;

namespace AI.Unit.Enemy
{
    public class EnemyModel
    {
        public float MaxHp = 100;
        public float MaxMp = 100;
        public readonly double MpRecoveryTime = 10;
        public readonly float SearchAngle = 120f;
        public readonly float SearchLength = 10f;
        public readonly float AttackLength = 2f;
        public int Power = 1;
        public int Speed = 1;
        public int Id;
        public float Hp { get; set; }
        public float Mp { get; set; }

        public Skill.Type Skill;
        public List<AIModel> Behaviors = new List<AIModel>();

        public EnemyModel()
        {
            Skill = Behavior.Skill.Type.Heal;
            Hp = MaxHp;
            Mp = MaxMp;
            Behaviors.Add(new AIModel(AIModel.Subject.Enemy, AIModel.Criterion.Hp, 0, 100, AIModel.Behavior.Attack));
            Behaviors.Add(new AIModel(AIModel.Subject.Enemy, AIModel.Criterion.Mp, 0, 100, AIModel.Behavior.Attack));
        }

        public EnemyModel(Skill.Type skill, List<AIModel> behaviors)
        {
            Skill = skill;
            Hp = MaxHp;
            Mp = MaxMp;
            Behaviors = behaviors;
        }
        public EnemyModel(int id, float maxHP, float maxMP, int power, int speed, Skill.Type skill, List<AIModel>behavios ){
            Power = power; 
            Speed = speed;
            Id = id;
            MaxHp = maxHP;
            MaxMp = maxMP;
            Behaviors = behavios;
            Skill = skill;
            Hp = MaxHp;
            Mp = MaxMp;
        }
    }
}
