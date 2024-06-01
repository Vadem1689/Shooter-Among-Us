using BRAmongUS.Yandex.Ad;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace BRAmongUS.Yandex
{
    public sealed class BootstrapLoader : MonoBehaviour
    {
        private void OnEnable() => YandexGame.GetDataEvent += OnInitialize;
        private void OnDisable() => YandexGame.GetDataEvent -= OnInitialize;

        private void Awake()
        {
            if (YandexGame.SDKEnabled)
            {
                OnInitialize();
            }
        }

        private void OnInitialize()
        {
            AdService.Instance.OnAddClosedAction += Initialize;

            AdService.Instance.SetInterstitialTimer(100);
            AdService.Instance.ShowInterstitialAd();
        }

        private void Initialize()
        {
            AdService.Instance.OnAddClosedAction -= Initialize;

            AdService.Instance.SetInterstitialTimer(0);

            SceneManager.LoadScene(Constants.Scenes.MainMenu);
        }
    }
}