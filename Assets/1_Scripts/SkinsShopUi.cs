using TMPro;
using UnityEngine;
using YG;

namespace BRAmongUS.Skins
{
    public sealed class SkinsShopUi : MonoBehaviour
    {
        [SerializeField] private SkinsShopManager skinsShopManager;
        [SerializeField] private TMP_Text coinsText;

        private void Awake()
        {
            skinsShopManager.OnCoinsCountChangedSkinAction += UpdateCoinsText;
            
            UpdateCoinsText(YandexGame.savesData.totalCoins);
        }
        
        private void OnDestroy()
        {
            skinsShopManager.OnCoinsCountChangedSkinAction -= UpdateCoinsText;
        }

        private void UpdateCoinsText(int coins)
        {
            coinsText.SetText(coins.ToString());
        }
    }
}