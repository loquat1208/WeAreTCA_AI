using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using AI.System;
using AI.Behavior;
using AI.Unit.Player;
using AI.Unit.Enemy;

using UniRx;

namespace AI.UI
{
    public class ServerUIView : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private List<EnemyController> enemys;
        [SerializeField] private ServerCamera serverCamera;
        [SerializeField] private Button playerButton;
        [SerializeField] private Button enemy1Button;
        [SerializeField] private Button enemy2Button;
        [SerializeField] private Button enemy3Button;
        [SerializeField] private Button enemy4Button;
        [SerializeField] private Button enemy5Button;
        [SerializeField] private Button fixedButton;
        [SerializeField] private Button farButton;
        [SerializeField] private Button nearButton;
        [SerializeField] private Button enemySearchUIOnButton;
        [SerializeField] private Button enemySearchUIOffButton;
        [SerializeField] private Button resetKey;
        [SerializeField] private Text playerText;
        [SerializeField] private List<Text> enemysTexts;

        // Use this for initialization
        void Start()
        {
            playerButton.OnClickAsObservable().Where(_ => player != null).Subscribe(_ => serverCamera.State = ServerCamera.STATE.Player).AddTo(this);
            enemy1Button.OnClickAsObservable().Where(_ => enemys[0] != null).Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy1).AddTo(this);
            enemy2Button.OnClickAsObservable().Where(_ => enemys[1] != null).Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy2).AddTo(this);
            enemy3Button.OnClickAsObservable().Where(_ => enemys[2] != null).Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy3).AddTo(this);
            enemy4Button.OnClickAsObservable().Where(_ => enemys[3] != null).Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy4).AddTo(this);
            enemy5Button.OnClickAsObservable().Where(_ => enemys[4] != null).Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy5).AddTo(this);
            fixedButton.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Fixed).AddTo(this);
            farButton.OnClickAsObservable().Subscribe(_ => serverCamera.Distance = ServerCamera.DISTANCE.Far).AddTo(this);
            nearButton.OnClickAsObservable().Subscribe(_ => serverCamera.Distance = ServerCamera.DISTANCE.Near).AddTo(this);
<<<<<<< HEAD
            enemySearchUIOnButton.OnClickAsObservable().Subscribe(_ => enemys.ForEach(x => x.SetSearchUI(true))).AddTo(this);
            enemySearchUIOffButton.OnClickAsObservable().Subscribe(_ => enemys.ForEach(x => x.SetSearchUI(false))).AddTo(this);
            resetKey.OnClickAsObservable().Subscribe(_ => SceneManager.LoadScene("Main")).AddTo(this);
=======
            enemySearchUIOnButton.OnClickAsObservable().Subscribe(_ => enemys.ForEach(x =>
            {
                if (x != null) x.SetSearchUI(true);
            })).AddTo(this);
            enemySearchUIOffButton.OnClickAsObservable().Subscribe(_ => enemys.ForEach(x =>
            {
                if (x != null)
                x.SetSearchUI(false);
            })).AddTo(this);
>>>>>>> 19854c89709b171931a8680de64da2e7844d3601

            Observable.EveryUpdate().Subscribe(_ =>
            {
                float playerHpPersent = player.Model.Hp / player.Model.MaxHp * 100f;
                float playerMpPersent = player.Model.Mp / player.Model.MaxMp * 100f;
                playerText.text = string.Format("Player\nHp : {0}%\nMp : {1}%", playerHpPersent, playerMpPersent);
                for (int i = 0; i < enemysTexts.Count; i++)
                {
                    EnemyModel model = enemys[i].Model;
                    float enemyHpPersent = model.Hp / model.MaxHp * 100f;
                    float enemyMpPersent = model.Mp / model.MaxMp * 100f;
                    enemysTexts[i].text = string.Format("Enemy{0}\n{1}\nHp : {2}%\nMp : {3}%\nPower : {4}\nSpeed : {5}\nSkill : {6}",
                        i, BehaviorToString(enemys[i].Behavior), enemyHpPersent, enemyMpPersent, model.Power, model.Speed, model.Skill);
                }
            });
        }

        private string BehaviorToString(AIModel.Behavior behavior)
        {
            switch (behavior)
            {
                case AIModel.Behavior.Attack: return "攻撃中";
                case AIModel.Behavior.Chase: return "追撃中";
                case AIModel.Behavior.Escape15:
                case AIModel.Behavior.Escape30:
                case AIModel.Behavior.Escape45: return "逃走中";
                case AIModel.Behavior.Skill: return "スキル使用中";
                case AIModel.Behavior.None: return "待機中";
                default: return string.Empty;
            }
        }
    }
}