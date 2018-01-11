using UnityEngine;
using System;
using UniRx;

namespace AI.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerView view;
        // NOTO: Prefab化してResourceフォルダーから呼ぶか決めないと
        [SerializeField] private GameObject attackTrigger;

        public PlayerModel Model { get; set; }

        private Rigidbody rigid { get { return GetComponent<Rigidbody>(); } }

        void Start()
        {
            Model = new PlayerModel();
            view.OnDirKey.Subscribe(OnMove);
            view.OnAttackKey.Subscribe(isAttack =>
            {
                OnAttack();
            });
        }

        private void OnMove(Vector3 dir)
        {
            rigid.velocity = dir * Model.Speed;
            rigid.rotation = Quaternion.LookRotation(dir);
        }

        private void OnAttack()
        {
            attackTrigger.SetActive(true);
            Delay(1, OnStay);
        }

        private void OnStay()
        {
            attackTrigger.SetActive(false);
        }

        private void Delay(float time, Action onComplete = null)
        {
            Observable.Timer(TimeSpan.FromSeconds(time)).First().Subscribe(_ => onComplete.Invoke());
        }
    }
}
