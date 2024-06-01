using BRAmongUS.Skins;
using BRAmongUS.SRCO;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace BRAmongUS.MainMenu
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private SkinsContainerSCRO skinsContainer;
        [SerializeField] private SkinUI playerSkinUi;
        [SerializeField] private CanvasGroup menuPanel;
        
        private void Start()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            SetPlayerSkinUi();
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            menuPanel.blocksRaycasts = true;
            SetPlayerSkinUi();
        }
        
        public void OnStartGame()
        {
            menuPanel.blocksRaycasts = false;
            SceneManager.LoadSceneAsync(Constants.Scenes.Game);
        }

        public void OnOpenShop()
        {
            menuPanel.blocksRaycasts = false;
            SceneManager.LoadSceneAsync(Constants.Scenes.Shop, LoadSceneMode.Additive);
        }

        public void OnOpenLeaderboard()
        {
            menuPanel.blocksRaycasts = false;
            SceneManager.LoadSceneAsync(Constants.Scenes.Leaderboard, LoadSceneMode.Additive);
        }

        public void OnMuteAudio()
        {
            
        }

        private void SetPlayerSkinUi()
        {
            playerSkinUi.Initialize(skinsContainer.Skins[YandexGame.savesData.selectedSkinIndex]);
        }
    }
}