using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRAmongUS.Timer
{
    public sealed class RemainingTimeTimerUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text minutesText;
        [SerializeField] private TMP_Text secondsText;
        
        [Space(10)]
        [SerializeField] public int maxMinutes = 5;
        [SerializeField] public int maxSeconds = 60;
        
        private Dictionary<int, string> cachedMinutesStrings = new ();
        private Dictionary<int, string> cachedSecondsStrings = new ();

        private RemainingTimeTimer timer;
        
        private int previousMinutes = -1;
        private int cachedMinutes;
        
        private void Start()
        {
            SetCachedTime();
            
            timer = RemainingTimeTimer.Instance;
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UpdateTimer()
        {
            cachedMinutes = timer.RemainingMinutes;
            if(previousMinutes != cachedMinutes)
            {
                minutesText.SetText(cachedMinutesStrings[cachedMinutes]);
                previousMinutes = cachedMinutes;
            }
            
            secondsText.SetText(cachedSecondsStrings[timer.RemainingSeconds]);
        }
        
        private void SubscribeEvents()
        {
            timer.OnTimeChanged += UpdateTimer;
        }
        
        private void UnsubscribeEvents()
        {
            timer.OnTimeChanged -= UpdateTimer;
        }
        
        private void SetCachedTime()
        {
            for (int i = 0; i < maxMinutes; i++)
            {
                cachedMinutesStrings.Add(i, i.ToString());
            }
            
            for (int i = 0; i < maxSeconds; i++)
            {
                if(i < 10)
                    cachedSecondsStrings.Add(i, $"0{i}");
                else
                    cachedSecondsStrings.Add(i, i.ToString());
            }
        }
    }
}