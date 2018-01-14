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
                    .Where(dir => dir != Vector3.zero)
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
    }
}