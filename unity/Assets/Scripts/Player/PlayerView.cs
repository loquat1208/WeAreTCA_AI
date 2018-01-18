using UnityEngine;
using UniRx;

namespace AI.Unit.Player
{
    public class PlayerView : MonoBehaviour
    {
        public IObservable<Vector3> OnDirKey
        {
            get
            {
                return Observable.EveryUpdate()
                    .Select(_ => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))
                    .AsObservable();
            }
        }

        public IObservable<bool> OnAttackKey
        {
            get
            {
                return Observable.EveryUpdate()
                    .Select(_ => Input.GetButtonDown("Fire1"))
                    .Where(isAttack => isAttack)
                    .AsObservable();
            }
        }

        public IObservable<bool> OnHealKey
        {
            get
            {
                return Observable.EveryUpdate()
                    .Select(_ => Input.GetButtonDown("Fire2"))
                    .Where(isHeal => isHeal)
                    .AsObservable();
            }
        }

        public IObservable<bool> OnDashKey
        {
            get
            {
                return Observable.EveryUpdate()
                    // NOTE: 後キーを決めないと。。
                    .Select(_ => Input.GetKeyDown(KeyCode.Space))
                    .Where(isDash => isDash)
                    .AsObservable();
            }
        }
    }
}