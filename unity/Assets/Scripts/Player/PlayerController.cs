using UnityEngine;

using UniRx;

using System.Linq;

using AI.Unit.Enemy;

namespace AI.Unit.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerView view;
        [SerializeField] private TargetView attackTrigger;

        public PlayerModel Model { get; set; }

        private Rigidbody rigid { get { return GetComponent<Rigidbody>(); } }

        void Start()
        {
            Model = new PlayerModel();
            view.OnDirKey.Subscribe(OnMove).AddTo(this);
            view.OnAttackKey.Subscribe(_ => OnAttack()).AddTo(this);
        }

        private void OnMove(Vector3 dir)
        {
            rigid.velocity = dir * Model.Speed;
            rigid.rotation = Quaternion.LookRotation(dir);
        }

        private void OnAttack()
        {
            attackTrigger.Target
                .ForEach(x =>
                {
                    EnemyController enemy = x.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.Model.Hp -= Model.Power;
                        Debug.Log(enemy.Model.Hp);
                    }
                });
        }
    }
}
