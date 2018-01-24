using UnityEngine;

using UniRx;

namespace AI.System
{
    public class ClientCamera : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float distance = -2f;
        [SerializeField] private float height = 5f;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float downAngle = -1f;

        void Start()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    Vector3 position = player.forward * distance + Vector3.up * height;
                    Quaternion angle = Quaternion.LookRotation(player.forward + new Vector3(0, downAngle, 0), player.up);
                    transform.position = Vector3.Lerp(transform.position, player.position + position, speed * Time.deltaTime);
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, angle, 10f * Time.deltaTime);
                })
                .AddTo(this);
        }
    }
}