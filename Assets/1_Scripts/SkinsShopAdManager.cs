using BRAmongUS.SRCO;
using BRAmongUS.Yandex.Ad;
using UnityEngine;

namespace BRAmongUS.Skins
{
    public sealed class SkinsShopAdManager : MonoBehaviour
    {
        [SerializeField] private SkinsShopManager skinsShopManager;
        [SerializeField] private GameSettingsSCRO gameSettings;

        private void Awake()
        {
            AdService.Instance.OnRewardVideoCompletedAction += OnRewardVideoCompleted;
        }
        
        private void OnDestroy()
        {
            AdService.Instance.OnRewardVideoCompletedAction -= OnRewardVideoCompleted;
        }

        private void OnRewardVideoCompleted(ERewardVideoType rewardType)
        {
            if(rewardType == ERewardVideoType.AddCoins)
                skinsShopManager.AddCoins(gameSettings.CoinsForShowedAd);
            
        }
        
        public void ShowRewardVideoForCoins()
        {
            AdService.Instance.ShowRewardBasedVideo(ERewardVideoType.AddCoins);
        }
    }
}