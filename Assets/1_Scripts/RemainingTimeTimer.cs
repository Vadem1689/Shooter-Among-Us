using System;
using System.Collections;
using BRAmongUS.SRCO;
using BRAmongUS.Utils.Singleton;
using UnityEngine;

namespace BRAmongUS.Timer
{
    public sealed class RemainingTimeTimer : Singleton<RemainingTimeTimer>
    {
        [SerializeField] private GameSettingsSCRO gameSettings;
        
        private WaitForSeconds cachedWaitForSeconds = new WaitForSeconds(1);
        
        private int startedTime;
        private bool isPaused;

        public int RemainingTime { get; private set; }
        
        public int RemainingMinutes => RemainingTime / 60;
        public int RemainingSeconds => RemainingTime % 60;
        
        public event Action OnTimeChanged = () => { };
        public event Action OnTimerFinished = () => { };

        private void Start()
        {
            startedTime = gameSettings.GameTimeInSeconds;
            RemainingTime = startedTime;
            StartCoroutine(StartTimerLoop());
            StartCoroutine(InvokeTimeFinishedTimer());
        }

        private IEnumerator StartTimerLoop()
        {
            while (true)
            {
                yield return cachedWaitForSeconds;
                if (!isPaused)
                {
                    --RemainingTime;
                    OnTimeChanged.Invoke();
                }
            }
        }

        private IEnumerator InvokeTimeFinishedTimer()
        {
            yield return new WaitForSeconds(++startedTime);
            OnTimerFinished.Invoke();
            
            StopAllCoroutines();
        }
    }
}