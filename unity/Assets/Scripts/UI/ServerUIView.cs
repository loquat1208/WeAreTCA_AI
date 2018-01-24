using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using AI.System;

using UniRx;

namespace AI.UI
{
    public class ServerUIView : MonoBehaviour
    {
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

        // Use this for initialization
        void Start()
        {
            playerButton.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Player).AddTo(this);
            enemy1Button.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy1).AddTo(this);
            enemy2Button.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy2).AddTo(this);
            enemy3Button.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy3).AddTo(this);
            enemy4Button.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy4).AddTo(this);
            enemy5Button.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Enemy5).AddTo(this);
            fixedButton.OnClickAsObservable().Subscribe(_ => serverCamera.State = ServerCamera.STATE.Fixed).AddTo(this);
            farButton.OnClickAsObservable().Subscribe(_ => serverCamera.Distance = ServerCamera.DISTANCE.Far).AddTo(this);
            nearButton.OnClickAsObservable().Subscribe(_ => serverCamera.Distance = ServerCamera.DISTANCE.Near).AddTo(this);
        }
    }
}