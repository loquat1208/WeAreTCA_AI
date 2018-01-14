using UnityEngine;

using System;

using UniRx;

using AI.Unit;

namespace AI.Player
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
            view.OnDirKey.Subscribe(OnMove);
            view.OnAttackKey.Subscribe(_ => OnAttack());
        }

        private void OnMove(Vector3 dir)
        {
            rigid.velocity = dir * Model.Speed;
            rigid.rotation = Quaternion.LookRotation(dir);
        }

        private void OnAttack()
        {
            attackTrigger.Target.ForEach(x => Debug.Log(x.name));
        }

        private void OnStay()
        {
        }
    }
}
