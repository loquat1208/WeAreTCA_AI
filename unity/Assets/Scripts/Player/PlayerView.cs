using UnityEngine;
using UniRx;

namespace AI.Unit.Player
{
    public class PlayerView : MonoBehaviour
    {
        public TargetView AttackTrigger;

        [SerializeField] private KeyCode attack = KeyCode.Z;
        [SerializeField] private KeyCode heal = KeyCode.X;
        [SerializeField] private KeyCode dash = KeyCode.C;

        // NOTE: ループが多すぎるコードリファトリング必要
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
                    .Select(_ => Input.GetKeyDown(attack))
                    .Where(isAttack => isAttack)
                    .AsObservable();
            }
        }

        public IObservable<bool> OnHealKey
        {
            get
            {
                return Observable.EveryUpdate()
                    .Select(_ => Input.GetKeyDown(heal))
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
                    .Select(_ => Input.GetKeyDown(dash))
                    .Where(isDash => isDash)
                    .AsObservable();
            }
        }
    }
}