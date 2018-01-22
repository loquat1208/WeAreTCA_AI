using UnityEngine;
using UnityEngine.AI;

using UniRx;

using AI.Behavior;
using AI.Unit.Player;

namespace AI.Unit.Enemy
{
    // NOTE: 後リファトリング必要
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private TargetView attackTrigger;

        public EnemyModel Model { get; set; }

        private Transform player;
        private NavMeshAgent nav;
        private AIModel.Behavior behavior;

        private Animator anim { get { return GetComponent<Animator>(); } }

        private void Start()
        {
            Model = new EnemyModel();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            nav = GetComponent<NavMeshAgent>();
            nav.stoppingDistance = 1.5f;
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
                    OnEscape();
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

        // NOTE: 下のメソッドを他のクラスに移動する？
        private void OnChase()
        {
            // NOTE: ずっとtrue, falseすること直したい
            anim.SetBool("IsRun", true);
            nav.SetDestination(player.position);
            nav.speed = Model.Speed;
        }

        private void OnStay()
        {
            // NOTE: ずっとtrue, falseすること直したい
            anim.SetBool("IsRun", false);
        }

        private void OnEscape()
        {
            // NOTE: ずっとtrue, falseすること直したい
            anim.SetBool("IsRun", true);
            Vector3 dir = Vector3.Normalize(transform.position - player.position);
            transform.position += dir * Model.Speed * Time.deltaTime;
            transform.localRotation = Quaternion.LookRotation(dir);
        }

        private void OnAttack()
        {
            OnChase();

            if (attackTrigger.Target.Count > 0)
                anim.SetTrigger("IsAttack");
            PlayerController playerController = player.GetComponent<PlayerController>();
        }

        // NOTE: AttackのAnimationEvent
        private void Attack()
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
                playerController.Model.Hp -= Model.Power;
        }

        private void OnSkill()
        {
            nav.isStopped = true;
            switch(Model.Skill)
            {
                case Skill.Type.Dash:
                    OnChase();

                    // TODO: animationを追加してanimationが終わったら減るようにする
                    // NOTO: Skillクラスに移動する？
                    if (attackTrigger.Target.Count > 0)
                    {
                        PlayerController playerController = player.GetComponent<PlayerController>();
                        if (playerController != null)
                            playerController.Model.Hp -= Skill.DashPower;
                    };
                    break;
                case Skill.Type.Heal:
                    anim.SetTrigger("IsHeal");
                    break;
                case Skill.Type.None:
                    break;
            }
        }

        private void Heal()
        {
            if (Model.Mp > Skill.HealMpCost)
            {
                Model.Mp -= Skill.HealMpCost;
                Model.Hp += Skill.HealPower;
            }
            Debug.Log(Model.Hp + " / " + Model.Mp);
        }
    }
}