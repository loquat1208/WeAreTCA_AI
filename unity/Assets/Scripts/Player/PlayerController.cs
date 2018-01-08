using UnityEngine;
using UniRx;

namespace AI.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerView view;

        public PlayerModel Model { get; set; }

        private Rigidbody rigid { get { return GetComponent<Rigidbody>(); } }

        void Start()
        {
            Model = new PlayerModel();
            view.OnDirKey.Subscribe(OnMove);
        }

        private void OnMove(Vector3 dir)
        {
            rigid.velocity = dir * Model.Speed;
            rigid.rotation = Quaternion.LookRotation(dir);
        }
    }
}