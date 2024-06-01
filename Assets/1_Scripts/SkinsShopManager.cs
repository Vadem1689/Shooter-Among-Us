using System;
using BRAmongUS.SRCO;
using BRAmongUS.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace BRAmongUS.Skins
{
    public sealed class SkinsShopManager : MonoBehaviour
    {
        [SerializeField] private SkinsContainerSCRO skinsContainer;
        [SerializeField] private SkinsSlider skinsSlider;
        
        [SerializeField] private Button buyButton;
        [SerializeField] private Button selectButton;
        
        [SerializeField] private TMP_Text selectButtonText;
        [SerializeField] private TMP_Text buyButtonText;

        [SerializeField] private bool autoSelectOnBuy = true;
        
        [Space(10)]
        [SerializeField] private LocalizedText selectText;
        [SerializeField] private LocalizedText selectedText;
        
        private SkinData Skin(in int index) => skinsContainer.Skins[index];

        public event Action<int> OnCoinsCountChangedSkinAction;

        private bool isRussianLanguage;
        
        private void Awake()
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
            selectButton.onClick.AddListener(OnSelectButtonClicked);
            skinsSlider.OnSkinSelectedAction += OnSkinUISelected;
            
            isRussianLanguage = YandexGame.savesData.language == "ru";
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveListener(OnBuyButtonClicked);
            selectButton.onClick.RemoveListener(OnSelectButtonClicked);
            skinsSlider.OnSkinSelectedAction -= OnSkinUISelected;
        }

        private void OnBuyButtonClicked()
        {
            Buy(skinsSlider.SelectedSkinIndex);

            if (autoSelectOnBuy)
            {
                SelectSkin(skinsSlider.SelectedSkinIndex);
                OnSelectButtonClicked();
            }
            else
            {
                selectButtonText.SetText(selectedText.GetText(isRussianLanguage));
                selectButton.interactable = true;
                SetActiveButtons(false);
            }
        }
        
        private void OnSelectButtonClicked()
        {
            SelectSkin(skinsSlider.SelectedSkinIndex);
            
            SetActiveButtons(false);
            selectButtonText.SetText(selectedText.GetText(isRussianLanguage));
            selectButton.interactable = false;
        }
        
        private void OnSkinUISelected(int skinIndex)
        {
            bool isSkinBought = YandexGame.savesData.boughtSkins[skinIndex];
            
            if (isSkinBought)
            {
                bool isSelected = YandexGame.savesData.selectedSkinIndex == skinIndex;
                selectButtonText.SetText(isSelected ? selectedText.GetText(isRussianLanguage) : selectText.GetText(isRussianLanguage));
                selectButton.interactable = !isSelected;
            }
            else
            {
                bool canBuy = YandexGame.savesData.totalCoins >= Skin(skinIndex).Price;
                buyButton.interactable = canBuy;
            
                buyButtonText.SetText($"{Skin(skinIndex).Price} {Constants.IconsTags.Coin}");
            }
            
            SetActiveButtons(!isSkinBought);
        }

        private void Buy(int skinIndex)
        {
            SavingUtils.SetBoolArrayElement(ESaveType.BoughtSkins, skinIndex, true);
            AddCoins(-Skin(skinIndex).Price);
        }
        
        private void SelectSkin(int skinIndex)
        {
            SavingUtils.SetInt(ESaveType.SelectedSkinIndex, skinIndex);
        }
        
        private void SetActiveButtons(bool enableBuyButton)
        {
            buyButton.gameObject.SetActive(enableBuyButton);
            selectButton.gameObject.SetActive(!enableBuyButton);
        }

        public void UnloadShopScene()
        {
            SceneManager.UnloadSceneAsync(Constants.Scenes.Shop);
        }
        
        public void AddCoins(int value)
        {
            SavingUtils.SetInt(ESaveType.TotalCoins, YandexGame.savesData.totalCoins + value);
            OnCoinsCountChangedSkinAction?.Invoke(YandexGame.savesData.totalCoins);
        }
    }
}