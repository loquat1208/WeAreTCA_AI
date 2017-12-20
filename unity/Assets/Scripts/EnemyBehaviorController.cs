using UnityEngine;

using AI.Player;

namespace AI.Enemy
{
    using Behavior = EnemyBehaviorModel.Behavior;

    public class EnemyBehaviorController
    {
        // TODO: 依存性がやばい。PlayerModel、EnemyModelを直接じゃなく何か必要
        public Behavior GetBehavior(PlayerModel player, EnemyModel enemy)
        {
            if (Vector3.Distance(player.Pos, enemy.Pos) > enemy.SearchLength)
                return Behavior.StandBy;

            switch (enemy.Tendency)
            {
                case EnemyModel.TENDENCY.BLAVE:
                    return Behavior.Attack;
                case EnemyModel.TENDENCY.TIMID:
                    return Behavior.Escape;
                case EnemyModel.TENDENCY.VULGAR:
                    bool power = player.Power * EnemyBehaviorModel.POWER_CRITERION < enemy.Power;
                    bool hp = player.Hp * EnemyBehaviorModel.HP_CRITERION < enemy.Hp;
                    Behavior state = power && hp ? Behavior.Attack : Behavior.Escape;
                    return state;
                default:
                    return Behavior.StandBy;
            }
        }
    }
}
