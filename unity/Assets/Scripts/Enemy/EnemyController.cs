using UnityEngine;
using UnityEngine.AI;

using UniRx;

using AI.Behavior;
using AI.Unit.Player;

namespace AI.Unit.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private TargetView attackTrigger;

        public EnemyModel Model { get; set; }

        private Transform player;
        private NavMeshAgent nav;
        private AIModel.Behavior behavior;

        private void Start()
        {
            Model = new EnemyModel();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            nav = GetComponent<NavMeshAgent>();
            Observable.EveryUpdate().Subscribe(_ =>
            {
                SetBehavior();
                OnAction();
            });
        }

        // NOTE: 現在順番は後に設定した条件優先
        private void SetBehavior()
        {
            // NOTE: コードリファトリングが必要
            for (int i = 0; i < Model.Behaviors.Count; i++ )
            {
                AIModel ai = Model.Behaviors[i];
                if (ai.GetSubject == AIModel.Subject.Player)
                {
                    PlayerModel model = player.GetComponent<PlayerController>().Model;
                    if (ai.GetCriterion == AIModel.Criterion.Hp)
                    {
                        if (model.Hp < ai.GetFrom || model.Hp > ai.GetTo)
                            continue;
                    }
                    if (ai.GetCriterion == AIModel.Criterion.Mp)
                    {
                        if (model.Mp < ai.GetFrom || model.Mp > ai.GetTo)
                            continue;
                    }
                }
                if (ai.GetSubject == AIModel.Subject.Enemy)
                {
                    if (ai.GetCriterion == AIModel.Criterion.Hp)
                    {
                        if (Model.Hp < ai.GetFrom || Model.Hp > ai.GetTo)
                            continue;
                    }
                    if (ai.GetCriterion == AIModel.Criterion.Mp)
                    {
                        if (Model.Mp < ai.GetFrom || Model.Mp > ai.GetTo)
                            continue;
                    }
                }

                behavior = ai.GetBehavior;
            }
        }

        private void OnAction()
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
            nav.isStopped = false;
            nav.SetDestination(player.position);

            // TODO: animationを追加してanimationが終わったら減るようにする
            if (attackTrigger.Target.Count > 0)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                    playerController.Model.Hp -= Model.Power;

                Debug.Log(playerController.Model.Hp);
            };
        }

        private void OnSkill()
        {
            nav.isStopped = true;
        }
    }
}