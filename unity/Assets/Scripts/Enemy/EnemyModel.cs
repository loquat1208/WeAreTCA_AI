﻿using System.Collections.Generic;

using AI.Behavior;

namespace AI.Unit.Enemy
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

        public Skill.Type Skill;
        public Dictionary<int, AIModel> Behaviors = new Dictionary<int, AIModel>();

        public EnemyModel()
        {
            Skill = Behavior.Skill.Type.None;
            Hp = maxHp;
            Mp = maxMp;
            Behaviors.Add(0, new AIModel(AIModel.Subject.Enemy, AIModel.Criterion.Hp, 0, 100, AIModel.Behavior.None));
            Behaviors.Add(1, new AIModel(AIModel.Subject.Enemy, AIModel.Criterion.Mp, 0, 100, AIModel.Behavior.Chase));
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
