using System;
using BRAmongUS.Utils.Singleton;
using YG;

namespace BRAmongUS.Yandex.Ad
{
    public enum ERewardVideoType
    {
        MultiplyCoins = 0,
        AddCoins = 1,
    }
    
    public sealed class AdService : SingletonDontDestroy<AdService>
    {
        public event Action OnAddOpenedAction = () => { };
        public event Action OnAddClosedAction = () => { };

        public event Action<ERewardVideoType> OnRewardVideoCompletedAction;

        protected override void Awake()
        {
            base.Awake();
            
            YandexGame.RewardVideoEvent += OnRewardVideoCompleted;
            YandexGame.OpenFullAdEvent += OnAdOpened;
            YandexGame.CloseFullAdEvent += OnAdClosed;
            YandexGame.OpenVideoEvent += OnAdOpened;
            YandexGame.CloseVideoEvent += OnAdClosed;
        }

        private void OnDestroy()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoCompleted;
            YandexGame.OpenFullAdEvent -= OnAdOpened;
            YandexGame.CloseFullAdEvent -= OnAdClosed;
            YandexGame.OpenVideoEvent += OnAdOpened;
            YandexGame.CloseVideoEvent += OnAdClosed;
        }

        public void ShowRewardBasedVideo(ERewardVideoType rewardType)
        {
            YandexGame.RewVideoShow((int)rewardType);
        }

        public void ShowInterstitialAd()
        { 
            YandexGame.FullscreenShow();
        }

        public void SetInterstitialTimer(int value)
        {
            YandexGame.timerShowAd = value;
        }

        private void OnRewardVideoCompleted(int rewardIndex)
        {
            OnRewardVideoCompletedAction?.Invoke((ERewardVideoType)rewardIndex);
        }

        private void OnAdOpened() => OnAddOpenedAction.Invoke();
        private void OnAdClosed() => OnAddClosedAction.Invoke();
    }
}