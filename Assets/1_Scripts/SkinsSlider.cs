using System;
using System.Collections;
using BRAmongUS.SRCO;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using YG;

namespace BRAmongUS.Skins
{
    public sealed class SkinsSlider : MonoBehaviour
    {
        [SerializeField] private SkinsContainerSCRO skinsContainer;
        [SerializeField] private SkinUI skinUIPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        private float snapSpeed;
        
        public int SelectedSkinIndex => simpleScrollSnap.CenteredPanel;

        public event Action<int> OnSkinSelectedAction;

        private void Awake()
        {
            var skins = skinsContainer.Skins;
            SkinUI skinUI;
            
            for (int i = 0, count = skins.Count; i < count; i++)
            {
                skinUI = Instantiate(skinUIPrefab, content);
                skinUI.Initialize(skins[i]);
            }
            
            snapSpeed = simpleScrollSnap.SnapSpeed;
            simpleScrollSnap.OnPanelCentered.AddListener(OnSkinSelected);
            simpleScrollSnap.enabled = true;
            simpleScrollSnap.OnInitialized += Init;
            OnSkinSelected(YandexGame.savesData.selectedSkinIndex, 0);
        }
        
        private void OnDestroy()
        {
            simpleScrollSnap.OnPanelCentered.RemoveListener(OnSkinSelected);
        }
        
        private void Init()
        {
            simpleScrollSnap.OnInitialized -= Init;

            StartCoroutine(InitCoroutine());
        }

        private IEnumerator InitCoroutine()
        {
            simpleScrollSnap.SnapSpeed = 1000;
            simpleScrollSnap.GoToPanel(YandexGame.savesData.selectedSkinIndex);
            yield return new WaitForSeconds(0.1f);
            simpleScrollSnap.SnapSpeed = snapSpeed;
        }

        private void OnSkinSelected(int page, int previousPage)
        {
            OnSkinSelectedAction?.Invoke(page);
        }
    }
}