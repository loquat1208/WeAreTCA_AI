using System.Collections.Generic;

using UnityEngine;

using UniRx;

namespace AI.System
{
    public class ServerCamera : MonoBehaviour
    {
        private enum STATE
        {
            Enemy1 = 0,
            Enemy2 = 1,
            Enemy3 = 2,
            Enemy4 = 3,
            Enemy5 = 4,
            Fixed = 5,
        }

        [SerializeField] private List<Transform> enemys;
        [SerializeField] private float height = 5f;
        [SerializeField] private float fixedHeight = 40f;
        [SerializeField] private float speed = 10f;

        private int state = 0;

        void Start()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        state += 1;
                        state = state > (int)STATE.Fixed ? 0 : state;
                    }
                    if (Input.GetButtonDown("Fire2"))
                    {
                        state -= 1;
                        state = state < 0 ? (int)STATE.Fixed : state;
                    }

                    OnShooting((STATE)state);
                })
                .AddTo(this);
        }

        private void OnShooting(STATE state)
        {
            switch (state)
            {
                case STATE.Enemy1:
                case STATE.Enemy2:
                case STATE.Enemy3:
                case STATE.Enemy4:
                case STATE.Enemy5:
                    transform.position = Vector3.Lerp(transform.position, enemys[(int)state].position + Vector3.up * height, speed * Time.deltaTime);
                    break;
                case STATE.Fixed:
                    transform.position = Vector3.Lerp(transform.position, Vector3.up * fixedHeight, speed * Time.deltaTime);
                    break;
            }
        }
    }
}