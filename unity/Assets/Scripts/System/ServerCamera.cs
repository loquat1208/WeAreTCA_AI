using System.Collections.Generic;

using UnityEngine;

using UniRx;

namespace AI.System
{
    public class ServerCamera : MonoBehaviour
    {
        public enum STATE
        {
            Enemy1 = 0,
            Enemy2 = 1,
            Enemy3 = 2,
            Enemy4 = 3,
            Enemy5 = 4,
            Fixed = 5,
            Player = 6,
        }

        public enum DISTANCE
        {
            Far,
            Near,
        }

        [SerializeField] private Transform player;
        [SerializeField] private List<Transform> enemys;
        [SerializeField] private float nearHeight = 10f;
        [SerializeField] private float farHeight = 80f;
        [SerializeField] private float speed = 10f;

        public STATE State { get; set; }
        public DISTANCE Distance { get; set; }

        private float height;

        void Start()
        {
            State = STATE.Fixed;
            Distance = DISTANCE.Near;

            Observable.EveryUpdate()
                .Subscribe(_ => OnShooting(State, Distance))
                .AddTo(this);
        }

        private void OnShooting(STATE state, DISTANCE distance)
        {
            switch (distance)
            {
                case DISTANCE.Far:
                    height = farHeight;
                    break;
                case DISTANCE.Near:
                    height = nearHeight;
                    break;
            }

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
                    transform.position = Vector3.Lerp(transform.position, Vector3.up * farHeight, speed * Time.deltaTime);
                    break;
                case STATE.Player:
                    transform.position = Vector3.Lerp(transform.position, player.position + Vector3.up * height, speed * Time.deltaTime);
                    break;
            }
        }
    }
}