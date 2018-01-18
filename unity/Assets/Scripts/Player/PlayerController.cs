using UnityEngine;

using UniRx;

using System.Linq;

using AI.Behavior;
using AI.Unit.Enemy;

namespace AI.Unit.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerView view;
        [SerializeField] private TargetView attackTrigger;

        public PlayerModel Model { get; set; }

        private bool isAction = false;
        private Animator anim { get { return GetComponent<Animator>(); } }

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
                .Where(_ => Model.Mp > Skill.DashMpCost && attackTrigger.Target.Count > 0)
                .Subscribe(_ => OnDash()).AddTo(this);
        }

        private void OnMove(Vector3 dir)
        {
            transform.position += dir * Model.Speed * Time.deltaTime;
            transform.localRotation = Quaternion.LookRotation(dir);
            anim.SetBool("param_idletorunning", true);
        }

        private void OnStay()
        {
            anim.SetBool("param_idletorunning", false);
        }

        private void OnAttack()
        {
            anim.SetTrigger("param_idletohit01");
        }

        private void OnHeal()
        {
            anim.SetTrigger("param_idletowinpose");
        }

        private void OnDash()
        {
            transform.position -= transform.forward * 2.7f;
            anim.SetTrigger("param_idletohit03");
        }

        private void Dash()
        {
            isAction = true;
            attackTrigger.Target
                .ForEach(x =>
                {
                    EnemyController enemy = x.GetComponent<EnemyController>();
                    Model.Mp -= Skill.DashMpCost;
                    if (enemy != null)
                        enemy.Model.Hp -= Skill.DashPower;
                    Debug.Log(enemy.Model.Hp + "/" + Model.Mp);
                });
        }

        private void Heal()
        {
            isAction = true;
            if (Model.Mp >= Skill.HealMpCost)
            {
                Model.Mp -= Skill.HealMpCost;
                Model.Hp += Skill.HealPower;
            }
        }

        // NOTO: AttackStart,EndはAnimationCommponentで処理
        private void Attack()
        {
            isAction = true;
            attackTrigger.Target
                .ForEach(x =>
                {
                    EnemyController enemy = x.GetComponent<EnemyController>();
                    if (enemy != null)
                        enemy.Model.Hp -= Model.Power;
                });
        }

        private void ActionEnd()
        {
            isAction = false;
        }
    }
}
