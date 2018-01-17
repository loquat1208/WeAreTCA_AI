using UnityEngine;

namespace AI.Behavior
{
    public class Skill : MonoBehaviour
    {
        public enum Type
        {
            Heal,
            Dash,
            None,
        }

        public const int DashPower = 2;
        public const int DashMpCost = 2;
        public const int HealPower = 1;
        public const int HealMpCost = 2;
    }
}
