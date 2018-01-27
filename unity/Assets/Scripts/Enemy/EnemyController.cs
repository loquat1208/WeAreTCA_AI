using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using UniRx;

using AI.Behavior;
using AI.Unit.Player;

namespace AI.Unit.Enemy
{
    // NOTE: 後リファトリング必要(Viewを作る、Interfaceを使用)
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private TargetView attackTrigger;
        [SerializeField] private GameObject searchUI;
        //サーバーにアクセスできるため、サーバーをシングルトンにしなかった理由は同じクラスで同じコルーチーンを複数同時に展開できないから
        public EnemyModel Model { get; set; }
        public Animator Anim { get { return GetComponent<Animator>(); } }
        public AIModel.Behavior Behavior { get; private set; }

        private Transform player;
        private NavMeshAgent nav;

        private Rigidbody rigid { get { return GetComponent<Rigidbody>(); } }


        private void Start()
        {
            Model = new EnemyModel();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            nav = GetComponent<NavMeshAgent>();
            nav.stoppingDistance = 1.5f;

            Observable.Interval(TimeSpan.FromSeconds(Model.MpRecoveryTime)).Subscribe(_ =>
            {
                Model.Mp += 1;
                Model.Mp = Model.Mp > Model.MaxMp ? Model.MaxMp : Model.Mp;
            }).AddTo(this);

            Observable.EveryUpdate().Subscribe(_ =>
            {
                SetBehavior();
                OnAction();
                CheckDeath();
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
                    float hpPersent = model.Hp / model.MaxHp * 100f;
                    float mpPersent = model.Mp / model.MaxMp * 100f;
                    if (ai.GetCriterion == AIModel.Criterion.Hp)
                    {
                        if (hpPersent < ai.GetFrom || hpPersent > ai.GetTo)
                            continue;
                    }
                    if (ai.GetCriterion == AIModel.Criterion.Mp)
                    {
                        if (mpPersent < ai.GetFrom || mpPersent > ai.GetTo)
                            continue;
                    }
                }
                if (ai.GetSubject == AIModel.Subject.Enemy)
                {
                    float hpPersent = Model.Hp / Model.MaxHp * 100f;
                    float mpPersent = Model.Mp / Model.MaxMp * 100f;
                    if (ai.GetCriterion == AIModel.Criterion.Hp)
                    {
                        if (hpPersent < ai.GetFrom || hpPersent > ai.GetTo)
                            continue;
                    }
                    if (ai.GetCriterion == AIModel.Criterion.Mp)
                    {
                        if (mpPersent < ai.GetFrom || mpPersent > ai.GetTo)
                            continue;
                    }
                }

                Behavior = ai.GetBehavior;
            }
        }

        // NOTE: ここで書くのもじゃね！
        public void SetSearchUI(bool trigger)
        {
            searchUI.SetActive(trigger);
        }

        private void CheckDeath()
        {
            if (Model.Hp > 0)
                return;

            Anim.SetTrigger("IsDeath");
        }

        private void OnAction()
        {
            switch(Behavior)
            {
                case AIModel.Behavior.Attack:
                    OnAttack();
                    break;
                case AIModel.Behavior.Escape15:
                    OnEscape(15f);
                    break;
                case AIModel.Behavior.Escape30:
                    OnEscape(30f);
                    break;
                case AIModel.Behavior.Escape45:
                    OnEscape(45f);
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
            Anim.SetBool("IsRun", true);
            nav.isStopped = false;
            nav.SetDestination(player.position);
            nav.speed = Model.Speed;
        }

        private void OnStay()
        {
            // NOTE: ずっとtrue, falseすること直したい
            nav.isStopped = true;
            Anim.SetBool("IsRun", false);
        }

        private void OnEscape(float length)
        {
            if (Vector3.Distance(transform.position, player.position) > length)
            {
                OnStay();
                return;
            }

            // NOTE: ずっとtrue, falseすること直したい
            Anim.SetBool("IsRun", true);
            Vector3 dir = Vector3.Normalize(transform.position - player.position);
            rigid.velocity = dir * Model.Speed;
            transform.localRotation = Quaternion.LookRotation(dir);
        }

        private void OnAttack()
        {
            OnChase();

            if (attackTrigger.Target.Count > 0)
                Anim.SetTrigger("IsAttack");
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
                        Anim.SetTrigger("IsDash");
                    break;
                case Skill.Type.Heal:
                    Anim.SetTrigger("IsHeal");
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
            {
                playerController.Model.Hp -= Model.Power;
                player.GetComponent<PlayerController>().Anim.SetTrigger("param_idletodamage");
            }
        }


        private void Dash()
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && attackTrigger.Target.Count > 0)
            {

                playerController.Model.Hp -= Skill.DashPower;
                Model.Mp -= Skill.DashMpCost;
                player.GetComponent<PlayerController>().Anim.SetTrigger("param_idletodamage");
            }
        }

        private void Heal()
        {
            if (Model.Mp > Skill.HealMpCost)
            {
                Model.Mp -= Skill.HealMpCost;
                Model.Hp += Skill.HealPower;
                Model.Hp = Model.Hp > Model.MaxHp ? Model.MaxHp : Model.Hp;
            }
        }

        private void Rotate(float angle)
        {
            transform.Rotate(Vector3.up * angle);
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}