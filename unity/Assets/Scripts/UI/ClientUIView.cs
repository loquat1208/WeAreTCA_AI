using UnityEngine;
using UnityEngine.UI;

using UniRx;

using AI.Unit.Player;

namespace AI.UI
{
    public class ClientUIView : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Text hp;
        [SerializeField] private Text mp;
        [SerializeField] private Image hpGauge;
        [SerializeField] private Image mpGauge;

        void Start()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    hpGauge.transform.localScale = new Vector3(player.Model.Hp / player.Model.MaxHp, 1, 1);
                    mpGauge.transform.localScale = new Vector3(player.Model.Mp / player.Model.MaxMp, 1, 1);
                    hp.text = string.Format("HP : {0} / {1}", player.Model.Hp, player.Model.MaxHp);
                    mp.text = string.Format("MP : {0} / {1}", player.Model.Mp, player.Model.MaxMp);
                });
        }
    }
}