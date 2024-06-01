using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRAmongUS.UI
{
    public sealed class RespawnSplashScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup respawnSplash;
        [SerializeField] private TMP_Text respawnText;
        [SerializeField] private Image deathImage;
        
        [SerializeField] private float fadeDuration = 1.0f;
        [SerializeField] private int respawnTime = 3;

        private void Start()
        {
            deathImage.sprite = GameController.Instance.RealPlayerSkinData.DeathSprite;
        }

        public void ShowSplash()
        {
            respawnSplash.DOFade(1, fadeDuration);
            StartCoroutine(SetRespawnText());
        }

        IEnumerator SetRespawnText()
        {
            for (int i = respawnTime; i != 0; i--)
            {
                respawnText.SetText(i.ToString());
                yield return new WaitForSeconds(1);
            }
            
            respawnSplash.DOFade(0, fadeDuration);
        }
    }
}