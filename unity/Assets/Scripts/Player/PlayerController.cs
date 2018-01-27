using UnityEngine;

using UniRx;

using System;
using System.Linq;

using AI.Behavior;
using AI.Unit.Enemy;

namespace AI.Unit.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerView view;

        public PlayerModel Model { get; set; }
        public Animator Anim { get { return GetComponent<Animator>(); } }

        private bool isAction = false;

        private Rigidbody rigid { get { return GetComponent<Rigidbody>(); } }

        void Start()
        {
            Model = new PlayerModel();

            view.OnDirKey
                .Where(_ => !isAction)
                .Subscribe(dir =>
                {
                    if (dir != Vector3.zero)
                        OnMove(dir);
                    else
                        OnStay();
                }).AddTo(this);

            view.OnAttackKey.Subscribe(_ => OnAttack()).AddTo(this);
            view.OnHealKey.Subscribe(_ => OnHeal()).AddTo(this);
            view.OnDashKey
                .Where(_ => Model.Mp > Skill.DashMpCost && view.AttackTrigger.Target.Count > 0)
                .Subscribe(_ => OnDash()).AddTo(this);

            Observable.EveryUpdate()
                .Select(_ => Model.Hp)
                .Where(hp => hp <= 0)
                .First()
                .Subscribe(_ => Anim.SetTrigger("param_idletoko_big"))
                .AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(Model.MpRecoveryTime))
                .Where(hp => hp > 0)
                .Subscribe(_ =>
                {
                    Model.Mp += 1;
                    Model.Mp = Model.Mp > Model.MaxMp ? Model.MaxMp : Model.Mp;
                }).AddTo(this);
        }

        private void OnMove(Vector3 dir)
        {
            rigid.velocity = transform.forward * dir.z * Model.Speed;
            transform.Rotate(new Vector3(0, dir.x, 0) * Model.RotateSpeed * Time.deltaTime);
            Anim.SetBool("param_idletorunning", true);
        }

        private void OnStay()
        {
            Anim.SetBool("param_idletorunning", false);
        }

        private void OnAttack()
        {
            Anim.SetTrigger("param_idletohit01");
        }

        private void OnHeal()
        {
            Anim.SetTrigger("param_idletowinpose");
        }

        private void OnDash()
        {
            transform.position -= transform.forward * 2.7f;
            Anim.SetTrigger("param_idletohit03");
        }

        private void Dash()
        {
            isAction = true;
            view.AttackTrigger.Target
                .ForEach(x =>
                {
                    EnemyController enemy = x.GetComponent<EnemyController>();
                    Model.Mp -= Skill.DashMpCost;
                    if (enemy != null)
                    {
                        enemy.Model.Hp -= Skill.DashPower;
                        enemy.GetComponent<EnemyController>().Anim.SetTrigger("IsDamage");
                    }
                });
        }

        private void Heal()
        {
            isAction = true;
            if (Model.Mp >= Skill.HealMpCost)
            {
                Model.Mp -= Skill.HealMpCost;
                Model.Hp += Skill.HealPower;
                Model.Hp = Model.Hp > Model.MaxHp ? Model.MaxHp : Model.Hp;
            }
        }

        // NOTO: AttackStart,EndはAnimationCommponentで処理
        private void Attack()
        {
            isAction = true;
            view.AttackTrigger.Target
                .ForEach(x =>
                {
                    EnemyController enemy = x.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.Model.Hp -= Model.Power;
                        enemy.GetComponent<EnemyController>().Anim.SetTrigger("IsDamage");
                    }
                });
        }

        private void Death()
        {
            Destroy(gameObject);
        }

        private void ActionEnd()
        {
            isAction = false;
        }
    }
}
