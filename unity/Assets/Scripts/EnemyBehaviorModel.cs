using UnityEngine;

namespace AI.Enemy
{
    public class EnemyBehaviorModel
    {
        public const float POWER_CRITERION = 0.5f; // 比較するPOWER基準
        public const float HP_CRITERION = 0.2f;    // 比較するHP基準

        // NOTE: 行動
        public enum Behavior
        {
            Attack,
            Escape,
            StandBy,
        }
    }
}
