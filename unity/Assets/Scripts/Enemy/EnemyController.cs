using UnityEngine;
using UnityEngine.AI;

using UniRx;

using AI.Behavior;

namespace AI.Unit.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyModel Model { get; set; }

        private Transform player;
        private NavMeshAgent nav;
        private AIModel.Behavior behavior;

        private void Start()
        {
            Model = new EnemyModel();

            player = GameObject.FindGameObjectWithTag("Player").transform;

            nav = GetComponent<NavMeshAgent>();

            Observable.EveryUpdate().Select(x => Model.Behaviors[0].GetBehavior).Subscribe(OnAction).AddTo(this);
        }
        
        private void OnAction(AIModel.Behavior behavior)
        {
            switch(behavior)
            {
                case AIModel.Behavior.Attack:
                    OnAttack();
                    break;
                case AIModel.Behavior.Escape:
                    OnAttack();
                    break;
                case AIModel.Behavior.Chase:
                    OnChase();
                    break;
                case AIModel.Behavior.Skill:
                    OnSkill();
                    break;
                case AIModel.Behavior.None:
                    OnStay();
                    break;
            }
        }

        private void OnChase()
        {
            nav.isStopped = false;
            nav.SetDestination(player.position);
        }

        private void OnStay()
        {
            nav.isStopped = true;
        }

        private void OnAttack()
        {
            nav.isStopped = true;
        }

        private void OnSkill()
        {
            nav.isStopped = true;
        }
    }
}