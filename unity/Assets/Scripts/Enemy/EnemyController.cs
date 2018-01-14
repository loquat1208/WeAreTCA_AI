using UnityEngine;

namespace AI.Unit.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyModel Model { get; set; }

        private void Start()
        {
            Model = new EnemyModel();
        }
    }
}