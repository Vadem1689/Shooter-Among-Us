using UnityEngine;
using YG;

namespace BRAmongUS.Yandex
{
    public sealed class YGInitializer : MonoBehaviour
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
            YandexGame.GameReadyAPI();
        }
    }
}