using BRAmongUS.SRCO;
using BRAmongUS.Yandex.Ad;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BRAmongUS.UI
{
    public sealed class FinishSplashScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _jostick;
        [SerializeField] private GameSettingsSCRO gameSettings;
        [SerializeField] private CanvasGroup finishSplash;
        [SerializeField] private TMP_Text killsAmountText;
        [SerializeField] private TMP_Text coinsAmountText;
        
        [SerializeField] private Button multiplyCoinsButton;

        [SerializeField] private float fadeDuration = 1.0f;

        private void OnEnable()
        { 
            SceneManager.sceneUnloaded += OnSceneUnloaded; 
        }
        
        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void ShowSplash(int killsAmount, int coinsAmount)
        {
            killsAmountText.SetText($"{killsAmount.ToString()} {Constants.IconsTags.Kill} = {killsAmount * gameSettings.CoinsForKill} {Constants.IconsTags.Coin}");
            UpdateCoinsAmountText(coinsAmount.ToString());
            
            finishSplash.DOFade(1, fadeDuration).OnComplete(() =>
            {
                finishSplash.blocksRaycasts = true;
                Cursor.visible = true;
            });
        }
        
        public void OpenShop()
        {
            finishSplash.blocksRaycasts = false;
            SceneManager.LoadSceneAsync(Constants.Scenes.Shop, LoadSceneMode.Additive);
        }
        
        public void OpenLeaderboard()
        {
            finishSplash.blocksRaycasts = false;
            SceneManager.LoadSceneAsync(Constants.Scenes.Leaderboard, LoadSceneMode.Additive);
        }
        
        public void ShowMultiplyCoinsAd()
        {
            AdService.Instance.ShowRewardBasedVideo(ERewardVideoType.MultiplyCoins);
            multiplyCoinsButton.interactable = false;
        }

        public void UpdateCoinsAmountText(in string text)
        {
            coinsAmountText.SetText($"{text} {Constants.IconsTags.Coin}"); 
        }

        public void StartNexGame()
        {
            finishSplash.blocksRaycasts = false;
            GameController.Instance.StartNewGame();
            _jostick.gameObject.SetActive(true);
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            finishSplash.blocksRaycasts = true;
            _jostick.gameObject.SetActive(false);
        }
    }
}