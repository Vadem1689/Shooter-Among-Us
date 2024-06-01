using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BRAmongUS;
using BRAmongUS.Audio;
using BRAmongUS.Loot;
using BRAmongUS.Skins;
using BRAmongUS.SRCO;
using BRAmongUS.Timer;
using BRAmongUS.Utils;
using BRAmongUS.Yandex.Ad;
using Cinemachine;
using UnityEngine.SceneManagement;
using YG;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private SpawnManager spawnManager;
    
    [SerializeField] private PlayerInputControllerLegacy playerInputControllerLegacy;

    [SerializeField] private RandomNamePicker randomNamePicker;

    [SerializeField] private CinemachineVirtualCamera playerCamera;

    [SerializeField] private List<Player> players;

    [SerializeField] private Player realPlayer;

    [SerializeField] private DeathLine deathLine;
    
    [SerializeField] private SkinsContainerSCRO skinsContainerSCRO;

    [SerializeField] private int fragReward = 50;

    [SerializeField] private float playerShieldDuration = 2;

    [SerializeField] private float reviveTime = 3.5f;

    [SerializeField] private GameWindow gameWindow;
    
    [SerializeField] private GameSettingsSCRO gameSettings;

    private WaitForSeconds cachedRespawnWaitForSeconds;
    private IEnumerator playerRespawnCoroutine;

    private AudioController audioController;

    private int killsCount;
    private int coinsCount;
    private int playersLeft;
    private float secondsLeft = 0;
    private bool isGameFinished;
    
    private SkinData specialPlayerSkinData;
    private SkinData defaultPlayerSkinData;

    public SkinData RealPlayerSkinData { get; private set; }

    public event Action OnRoundEnded = () => { };
    public event Action OnRealPlayerDied = () => { };
    public event Action<Player> OnRealPlayerSpawned;
    
    private void Awake()
    {
        cachedRespawnWaitForSeconds = new WaitForSeconds(reviveTime);
    }
    
    private void Start()
    {
        audioController = AudioController.Instance;
        
        RemainingTimeTimer.Instance.OnTimerFinished += OnGameEnded;
        AdService.Instance.OnRewardVideoCompletedAction += OnRewardVideoCompleted;
        
        Cursor.visible = false;
    }

    private void OnDestroy()
    { 
        RemainingTimeTimer.Instance.OnTimerFinished -= OnGameEnded;
        AdService.Instance.OnRewardVideoCompletedAction -= OnRewardVideoCompleted;
    }

    public void Init()
    {
        Instance = this;
        randomNamePicker.Init();
        players = spawnManager.Spawn();
        playersLeft = players.Count;
        realPlayer = players[Random.Range(0, players.Count)];
        SetRealPlayer(realPlayer);
        SetPlayersSkin();
    }

    public void SetPlayerDead(Player deadPlayer, Player shooter)
    {
        if (isGameFinished) return;
        
        if(deadPlayer.IsRealPlayer)
            OnRealPlayerDied.Invoke();
        
        if (deadPlayer == null)
        {
            Debug.Log("Player was dead");
            return;
        }

        if (shooter.IsRealPlayer) 
            AddKillsCount(1);
        
        audioController.PlaySound(deadPlayer.IsRealPlayer ? ESoundType.PlayerDeath : ESoundType.EnemyDeath);
        bool hasSpecialSkinColor = deadPlayer.HasSpecialSkinColor;
        GameObject deadPlayerGameObject = deadPlayer.gameObject;
        players.Remove(deadPlayer);
        Destroy(deadPlayer);

       playerRespawnCoroutine = RespawnPlayer(deadPlayerGameObject, deadPlayer.IsRealPlayer, hasSpecialSkinColor);
       StartCoroutine(playerRespawnCoroutine);
    }

    IEnumerator RespawnPlayer(GameObject deadPlayer, bool isRealPlayer, bool hasSpecialSkinColor)
    {
        if (isRealPlayer)
        {
            EnablePlayerInput(false);
            gameWindow.ShowRespawnSplash();
        }

        yield return cachedRespawnWaitForSeconds;

        Player newPlayer = spawnManager.SpawnPlayer();
        players.Add(newPlayer);

        if (isRealPlayer && !isGameFinished)
        {
            SetRealPlayer(newPlayer);
            realPlayer.ActivateShield(playerShieldDuration);
        }
        
        if (isRealPlayer)
        {
            newPlayer.SetSkin(RealPlayerSkinData);
        }
        else if (hasSpecialSkinColor)
        {
            newPlayer.SetSkin(specialPlayerSkinData);
            newPlayer.HasSpecialSkinColor = true;
        }
        else
        {
            newPlayer.SetSkin(defaultPlayerSkinData);
        }
        
        Destroy(deadPlayer);
    }

    private void SetRealPlayer(in Player player)
    {
        realPlayer = player;
        realPlayer.IsRealPlayer = true;
        string playerName = YandexGame.playerName;

        if (!YandexGame.auth || playerName == String.Empty)
        {
            if(YandexGame.savesData.language == "ru")
                playerName = "Аноним";
            else
                playerName = "Anonymous";
        }
        realPlayer.SetPlayerName(playerName);
        realPlayer.tag = Constants.Tags.Player;
        playerCamera.Follow = realPlayer.transform;
        OnRealPlayerSpawned?.Invoke(realPlayer);
        realPlayer.DisableAI();
        if(YandexGame.EnvironmentData.isMobile)
            realPlayer.EnablePlayerAI();
        EnablePlayerInput(true);
    }

    private void EnablePlayerInput(bool value)
    {
        playerInputControllerLegacy.enabled = value;
    }

    private void SetPlayersSkin()
    {
        var skins = skinsContainerSCRO.Skins;
        RealPlayerSkinData = skins[YandexGame.savesData.selectedSkinIndex];

        defaultPlayerSkinData = skins[skinsContainerSCRO.BotSkinIndex];
        
        realPlayer.SetSkin(RealPlayerSkinData);
       
        players.Remove(realPlayer);
        for (var i = 0; i < players.Count; i++)
        {
            players[i].SetSkin(defaultPlayerSkinData);
        }
        
        Player specialPlayer = players[Random.Range(0, players.Count)];
        specialPlayerSkinData = skins.GetRandomExcept(skins[skinsContainerSCRO.BotSkinIndex], RealPlayerSkinData);
        
        specialPlayer.SetSkin(specialPlayerSkinData);
        specialPlayer.HasSpecialSkinColor = true;
        
        players.Add(realPlayer);
    }

    private void AddKillsCount(in int value)
    {
        killsCount += value;
        SavingUtils.SetInt(ESaveType.TotalKills, value + YandexGame.savesData.totalKills);
        gameWindow.SetKillsCount(killsCount);
    }
    
    public void AddCoinsCount(in int value)
    {
        coinsCount += value;
        SavingUtils.SetInt(ESaveType.TotalCoins, value + YandexGame.savesData.totalCoins);
        gameWindow.SetCoinsCount(coinsCount);
    }

    private void Loose()
    {
        Debug.Log("You Loose");
        StartCoroutine(ResetGame());
        gameWindow.ShowLooseSplash();
    }

    private void WinCheck()
    {
        if (playersLeft == 1)
        {
            Debug.Log("You Won");
            StartCoroutine(ResetGame());
            gameWindow.ShowWinSplash();
        }
    }

    private void OnGameEnded()
    {
        isGameFinished = true;
        
        if(playerRespawnCoroutine != null)
            StopCoroutine(playerRespawnCoroutine);
        
        EnablePlayerInput(false);

        foreach (var player in players)
        {
            Destroy(player);
        }
        
        SavingUtils.SetInt(ESaveType.TotalCoins, (killsCount * gameSettings.CoinsForKill) + YandexGame.savesData.totalCoins);
        
        YandexGame.NewLeaderboardScores(Constants.ID.LeaderboardID, YandexGame.savesData.totalKills);
        StartCoroutine(OpenFinishSplash());
        
        OnRoundEnded.Invoke();
    }
    
    IEnumerator OpenFinishSplash()
    {
        gameWindow.ShowFinishSplash(killsCount, coinsCount);
        
        yield return new WaitForSeconds(1);
        AdService.Instance.ShowInterstitialAd();
    }

    IEnumerator ResetGame()
    {
        Debug.Log("Reseting Scene...");
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    
    private void OnRewardVideoCompleted(ERewardVideoType rewardType)
    {
        switch (rewardType)
        {
            case ERewardVideoType.MultiplyCoins:
                int tempCoinsCount = coinsCount * gameSettings.CoinsMultiplierOnShowedAd;
                gameWindow.SetFinishScreenCoinsAmount(tempCoinsCount);
                tempCoinsCount -= coinsCount;
                SavingUtils.SetInt(ESaveType.TotalCoins, tempCoinsCount + YandexGame.savesData.totalCoins);
                break;
        }
    }

    public string GetRandomPlayerName()
    {
        return randomNamePicker.GetRandomPlayerName();
    }

    public void StartNewGame()
    {
        StartCoroutine(ResetGame());
    }

    public Player GetRealPlayer()
    {
        return realPlayer;
    }

    public AmmoBox GetRandomLootBox()
    {
        return spawnManager.GetRandomLootBox();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Player GetClosestPlayer(in Transform target)
    {
        float distance = Mathf.Infinity;
        float curdistance;
        int minimalDistanceIterator = 0;
        Player targetPlayer = target.GetComponent<Player>();

        for (var i = 0; i < players.Count; i++)
        {
            if (players[i] == null) continue;
            curdistance = (players[i].transform.position - target.position).magnitude;
            if (curdistance < distance && players[i] != targetPlayer)
            {
                distance = curdistance;
                minimalDistanceIterator = i;
            }
        }

        return players[minimalDistanceIterator];
    }

    public void CheckDeathLine()
    {
        for (var i = 0; i < players.Count; i++)
        {
            if (players[i] == null) continue;
            if ((players[i].transform.position - deathLine.transform.position).magnitude >= deathLine.GetDamageRadius())
            {
                players[i].TakeDamage(deathLine.GetDamageAmount(), null);
            }
        }
    }
}