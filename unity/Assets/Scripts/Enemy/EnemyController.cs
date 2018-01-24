using System;

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

            Observable.Interval(TimeSpan.FromSeconds(Model.MpRecoveryTime)).Subscribe(_ =>
            {
                Model.Mp += 1;
                Debug.Log(Model.Mp);
            }).AddTo(this);

            Observable.EveryUpdate().Subscribe(_ =>
            {
                SetBehavior();
                OnAction();
            }).AddTo(this);
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
            if (!IsDetected)
            {
                OnStay();
                return;
            }

            // NOTE: ずっとtrue, falseすること直したい
            anim.SetBool("IsRun", true);
            nav.isStopped = false;
            nav.SetDestination(player.position);
            nav.speed = Model.Speed;
        }

        private void OnStay()
        {
            // NOTE: ずっとtrue, falseすること直したい
            nav.isStopped = true;
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

        private void OnSkill()
        {
            switch(Model.Skill)
            {
                case Skill.Type.Dash:
                    OnChase();

                    // NOTO: Skillクラスに移動する？
                    if (attackTrigger.Target.Count > 0)
                        anim.SetTrigger("IsDash");
                    break;
                case Skill.Type.Heal:
                    anim.SetTrigger("IsHeal");
                    break;
                case Skill.Type.None:
                    break;
            }
        }

        private bool IsDetected 
        {
            get
            {
                Vector3 dirToPlayer = player.position - transform.position;
                float angle = Vector3.Angle(transform.forward, dirToPlayer);
                if (angle > Model.SearchAngle * 0.5f)
                    return false;

                RaycastHit hit;
                Vector3 startPosition = transform.position + transform.up + transform.forward;
                bool isHit = Physics.Raycast(startPosition, dirToPlayer, out hit, Model.SearchLength);
                if (isHit && hit.transform.tag == "Player")
                    return true;

                return false;
            }
        }

        // NOTE: AnimationEvent
        private void Attack()
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
                playerController.Model.Hp -= Model.Power;
        }


        private void Dash()
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Model.Hp -= Skill.DashPower;
                Model.Mp -= Skill.DashMpCost;
            }
        }

        private void Heal()
        {
            if (Model.Mp > Skill.HealMpCost)
            {
                Model.Mp -= Skill.HealMpCost;
                Model.Hp += Skill.HealPower;
            }
        }

        private void Rotate(float angle)
        {
            transform.Rotate(Vector3.up * angle);
        }
    }
}