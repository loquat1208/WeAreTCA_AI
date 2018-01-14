using UnityEngine;
using UnityEngine.AI;

using System.Collections;

namespace AI.Unit.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyModel Model { get; set; }

        private Transform player;
        private NavMeshAgent nav;

        private void Start()
        {
            Model = new EnemyModel();

            player = GameObject.FindGameObjectWithTag("Player").transform;

            nav = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (nav.enabled)
                nav.SetDestination(player.position);
        }

        public void OnChase()
        {
            nav.isStopped = false;
        }

        public void OnStay()
        {
            nav.isStopped = true;
        }
    }
}