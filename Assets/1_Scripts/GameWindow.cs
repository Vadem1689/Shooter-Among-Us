using BRAmongUS.UI;
using BRAmongUS.Utils.Singleton;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using YG;

public class GameWindow : Singleton<GameWindow>
{
    [SerializeField] TMP_Text TotalKillsText;
    
    [SerializeField] TMP_Text TotalCoinsText;

    [SerializeField] CanvasGroup WinSplash;

    [SerializeField] CanvasGroup LooseSplash;
    
    [SerializeField] CanvasGroup interactionButtonCanvasGroup;
    
    [SerializeField] Canvas TouchCanvas;

    [SerializeField] RespawnSplashScreen respawnSplash;
    
    [SerializeField] FinishSplashScreen finishSplashScreen;
    
    [SerializeField] GameObject playerSight;
    
    [SerializeField] float interactButtonFadeDuration = 0.5f;
    
    private Button interactionButton;
    
    protected override void Awake()
    { 
        base.Awake();
        bool isMobile = YandexGame.EnvironmentData.isMobile;
        TouchCanvas.enabled = isMobile;
        playerSight.SetActive(!isMobile);
        
        interactionButton = interactionButtonCanvasGroup.GetComponent<Button>();
    }

    public void SetKillsCount(int value)
    {
        TotalKillsText.text = value.ToString();
    }
    
    public void SetCoinsCount(int value)
    {
        TotalCoinsText.text = value.ToString();
    }
    
    public void SetFinishScreenCoinsAmount(int value)
    {
        finishSplashScreen.UpdateCoinsAmountText(value.ToString());
    }

    public void ShowWinSplash()
    {
        Debug.Log("ShowWinSplash");
        WinSplash.enabled = true;
        WinSplash.alpha = 0;
        WinSplash.DOFade(1,1.0f);
    }

    public void ShowLooseSplash()
    {
        Debug.Log("ShowLooseSplash");
        LooseSplash.enabled = true;
        LooseSplash.alpha = 0;
        LooseSplash.DOFade(1, 1.0f);
    }
    
    public void ShowRespawnSplash()
    {
        respawnSplash.ShowSplash();
    }
    
    public void ShowFinishSplash(int killsAmount, int coinsAmount)
    {
        finishSplashScreen.ShowSplash(killsAmount, coinsAmount);
    }

    public void ShowInteractionButton()
    {
        interactionButtonCanvasGroup.blocksRaycasts = true;
        interactionButtonCanvasGroup.DOFade(1, interactButtonFadeDuration);
    }
    
    public void HideInteractionButton()
    {
        interactionButtonCanvasGroup.blocksRaycasts = false;
        interactionButtonCanvasGroup.DOFade(0, interactButtonFadeDuration);
    }
    
    public void ActivateInteractionButton(bool interactable)
    {
        interactionButton.interactable = interactable;
    }
}
