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

        private bool isAttack = false;
        private Animator anim { get { return GetComponent<Animator>(); } }

        void Start()
        {
            Model = new PlayerModel();

            view.OnDirKey
                .Where(_ => !isAttack)
                .Subscribe(dir =>
                {
                    if (dir != Vector3.zero)
                        OnMove(dir);
                    else
                        OnStay();
                }).AddTo(this);

            view.OnAttackKey.Subscribe(_ => OnAttack()).AddTo(this);
            view.OnHealKey.Subscribe(_ => OnHeal()).AddTo(this);
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

        private void Heal()
        {
            if (Model.Mp > 0)
            {
                Model.Mp -= Skill.HealMpCost;
                Model.Hp += Skill.HealPower;
                Debug.Log(Model.Hp + "/" + Model.Mp);
            }
        }

        // NOTO: AttackStart,EndはAnimationCommponentで処理
        private void AttackStart()
        {
            isAttack = true;
            attackTrigger.Target
                .ForEach(x =>
                {
                    EnemyController enemy = x.GetComponent<EnemyController>();
                    if (enemy != null)
                        enemy.Model.Hp -= Model.Power;
                });
        }

        private void AttackEnd()
        {
            isAttack = false;
        }
    }
}
