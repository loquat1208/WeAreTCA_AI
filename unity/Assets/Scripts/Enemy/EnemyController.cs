using System;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

using UniRx;

using AI.Behavior;
using AI.Unit.Player;

namespace AI.Unit.Enemy
{
    // NOTE: 後リファトリング必要(Viewを作る、Interfaceを使用)
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private GameObject searchUI;
        [SerializeField] private GameObject Panel;
        //サーバーにアクセスできるため、サーバーをシングルトンにしなかった理由は同じクラスで同じコルーチーンを複数同時に展開できないから
        public EnemyModel Model { get; set; }
        public Animator Anim { get { return GetComponent<Animator>(); } }
        public AIModel.Behavior Behavior { get; private set; }
        public AIModel AI { get; private set; }

        private Transform player;
        private NavMeshAgent nav;
        private RaycastHit hit;

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
                AI = ai;
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
                    Panel.GetComponentInChildren<Text>().text = string.Format("{0}の{1}が\n{2}以上{3}以下\n攻撃", AI.GetSubject, AI.GetCriterion, AI.GetFrom, AI.GetTo);
                    OnAttack();
                    break;
                case AIModel.Behavior.Escape15:
                    Panel.GetComponentInChildren<Text>().text = string.Format("{0}の{1}が\n{2}以上{3}以下\n逃走", AI.GetSubject, AI.GetCriterion, AI.GetFrom, AI.GetTo);
                    OnEscape(15f);
                    break;
                case AIModel.Behavior.Escape30:
                    Panel.GetComponentInChildren<Text>().text = string.Format("{0}の{1}が\n{2}以上{3}以下\n逃走", AI.GetSubject, AI.GetCriterion, AI.GetFrom, AI.GetTo);
                    OnEscape(30f);
                    break;
                case AIModel.Behavior.Escape45:
                    Panel.GetComponentInChildren<Text>().text = string.Format("{0}の{1}が\n{2}以上{3}以下\n逃走", AI.GetSubject, AI.GetCriterion, AI.GetFrom, AI.GetTo);
                    OnEscape(45f);
                    break;
                case AIModel.Behavior.Chase:
                    Panel.GetComponentInChildren<Text>().text = "追撃中";
                    OnChase();
                    break;
                case AIModel.Behavior.Skill:
                    Panel.GetComponentInChildren<Text>().text = string.Format("{0}の{1}が\n{2}以上{3}以下\nスキル", AI.GetSubject, AI.GetCriterion, AI.GetFrom, AI.GetTo);
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

            if (Vector3.Distance(transform.position, player.position) < Model.AttackLength && hit.transform != null && hit.transform.tag == "Player")
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
                    if (Vector3.Distance(transform.position, player.position) < Model.AttackLength && hit.transform != null && hit.transform.tag == "Player")
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
            Panel.GetComponentInChildren<Image>().color = Color.red;
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Model.Hp -= Model.Power;
                player.GetComponent<PlayerController>().Anim.SetTrigger("param_idletodamage");
            }
        }


        private void Dash()
        {
            Panel.GetComponentInChildren<Image>().color = Color.red;
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && Vector3.Distance(transform.position, player.position) < Model.AttackLength && hit.transform != null && hit.transform.tag == "Player")
            {
                playerController.Model.Hp -= Skill.DashPower;
                Model.Mp -= Skill.DashMpCost;
                player.GetComponent<PlayerController>().Anim.SetTrigger("param_idletodamage");
            }
        }

        private void Heal()
        {
            Panel.GetComponentInChildren<Image>().color = Color.red;
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

        private void StartAction()
        {
            Panel.GetComponentInChildren<Image>().color = Color.green;
        }
    }
}