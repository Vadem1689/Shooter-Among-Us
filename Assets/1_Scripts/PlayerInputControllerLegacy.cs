using System;
using System.Collections;
using System.Runtime.CompilerServices;
using BRAmongUS.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using YG;

[DefaultExecutionOrder(-1)]
public sealed class PlayerInputControllerLegacy : MonoBehaviour
{
    [SerializeField] private Transform targetIcon;
    [SerializeField] private GameController gameController;
    
    [SerializeField] private float targetImagePaddingX = 0.015f;
    [SerializeField] private float targetImagePaddingY = 0.015f;
    [SerializeField] private float targetImageMoveSpeed = 0.5f;
    
    private Vector2 MovementAmount;
    private Vector2 lookingDirection;
    private Vector3 mousePosition;
    private Vector2 playerPositionVector2 = Vector2.zero;
    private bool isFiring;
    
    private Coroutine fireCoroutine;
    
    private ProjectInput input;
    private Player player;
    
    private DesktopPlayerSight desktopPlayerSight;
    
    public Transform PlayerTransform { get; private set; }
    
    private void Awake()
    {
        input = new ProjectInput(); 
        input.Enable();
        gameController.OnRealPlayerSpawned += Init;
        gameController.OnRealPlayerDied += OnPlayerDied;
        gameController.OnRoundEnded += BlockAllInputs;

        if (!YandexGame.EnvironmentData.isMobile)
            desktopPlayerSight = new DesktopPlayerSight();
    }

    private void Start()
    {
        StartCoroutine(StartMoveSightCoroutine());
    }

    private void OnDestroy()
    {
        input.Disable();
        UnsubscribeEvents();
        
        GameController.Instance.OnRealPlayerSpawned -= Init;
        GameController.Instance.OnRealPlayerDied -= OnPlayerDied;
        gameController.OnRoundEnded -= BlockAllInputs;
    }
    
    private void FixedUpdate()
    {
        playerPositionVector2 = PlayerTransform.position;
        player.Move(Time.fixedDeltaTime * input.Player.Move.ReadValue<Vector2>());
    }
    
    private void OnLook(InputAction.CallbackContext ctx)
    {
        player.Look(playerPositionVector2 + lookingDirection * 10);
    }
    
    private void Init(Player realPlayer)
    {
        player = realPlayer;
        PlayerTransform = player.transform;
        
        Vector3 newTargetIconPosition = Vector3.zero;
        newTargetIconPosition.z = 10;
        targetIcon.localPosition = newTargetIconPosition;
        SubscribeEvents();
    }

    public void Interact()
    {
        if (player.CanOpenAmmoBox())
        {
            player.SwitchGunFromBox();
        }
    }
    
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        Interact();
    }

    private void OnStartFire(InputAction.CallbackContext ctx)
    { 
        fireCoroutine = StartCoroutine(StartFireCoroutine());
    }
    
    private void OnStopFire(InputAction.CallbackContext ctx)
    {
        StopFire();
    }

    private void StopFire()
    {
        isFiring = false;
    }
    
    private IEnumerator StartMoveSightCoroutine()
    {
        while (true)
        {
            desktopPlayerSight.SetPosition(ref lookingDirection, input.Player.Look.ReadValue<Vector2>(), targetIcon, PlayerTransform);
            yield return null;
        }
    }
    
    private IEnumerator StartFireCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(player.GunFirePeriod);
        
        isFiring = true;
        while (isFiring)
        {
            player.Fire(playerPositionVector2 + lookingDirection * 10);
            yield return wait;
        }
    }
    
    private void OnPlayerDied()
    {
        UnsubscribeEvents();
        if(fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }
    
    private void BlockAllInputs()
    {
        UnsubscribeEvents();
        StopAllCoroutines();
    }
    
    private void SubscribeEvents()
    {
        input.Player.Interact.performed += OnInteract;
        input.Player.Look.performed += OnLook;
        input.Player.Fire.started += OnStartFire;
        input.Player.Fire.canceled += OnStopFire;
    }
    
    private void UnsubscribeEvents()
    {
        input.Player.Interact.performed -= OnInteract;
        input.Player.Look.performed -= OnLook;
        input.Player.Fire.started -= OnStartFire;
        input.Player.Fire.canceled -= OnStopFire;
    }
}

