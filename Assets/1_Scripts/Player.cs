using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;
using BRAmongUS.Loot;
using BRAmongUS.Skins;
using BRAmongUS.SRCO;
using BRAmongUS.Utils;

public sealed class Player : MonoBehaviour
{
    [SerializeField] private TextMeshPro nickText;

    [SerializeField] private GunController gunController;

    [SerializeField] private Health health;

    [SerializeField] private Slider healthSlider, reloadingSlider;

    [SerializeField] private PlayerGraphics playerBodyGraphics;

    [SerializeField] private InteractionController interactionController;

    [SerializeField] private MovementController movementController;

    [SerializeField] private PathfindingController pathfindingController;

    [SerializeField] private BehaviorTree enemyBehaviorTree;
    [SerializeField] private BehaviorTree playerBehaviorTree;

    [SerializeField] private Collider2D bodyCollider;
    
    [SerializeField] private Shield shield;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameSettingsSCRO gameSettings;
    
    private float pathfindingSpeed;
    private bool isDead;
    
    public bool IsRealPlayer { get; set; }
    public bool HasSpecialSkinColor { get; set; }
    
    public bool IsActiveShield => shield.IsActive;
    
    public int MaxHealth { get; private set; }
    
    public float GunFirePeriod => gunController.GetFirePeriod();
    
    public event Action OnDie = () => { };

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        health.Init();
        UpdateHealthSlider();
        UpdateReloadingSlider();
        
        MaxHealth = (int)health.GetStartHealth();
        pathfindingSpeed = gameSettings.BotSpeed;
        
        if (IsRealPlayer)
        {
            playerBehaviorTree.enabled = true; 
        }
    }
    
    public void SetSkin(in SkinData skin)
    {
        playerBodyGraphics.SetPlayerSkin(skin);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void SetPlayerName(string name)
    {
        nickText.text = name;
    }

    public string GetPlayerName()
    {
        return nickText.text;
    }
    
    public void Fire(in Vector2 target)
    {
        //Debug.Log("PlayerFire:"+target);
        gunController.Fire(target, this);
    }

    public void Look(in Vector2 target)
    {
        gunController.Look(target);
        playerBodyGraphics.Look(target);
    }

    public void Move(in Vector2 movementAmount)
    {
        movementController.Move(movementAmount);
    }

    public void TakeDamage(float amount, in Player shooter)
    {
        if(!IsRealPlayer)
            amount *= gameSettings.PlayerWeaponDamageMultiplier;
        
        isDead = health.TakeDamage(amount);
        UpdateHealthSlider();
        if (isDead)
        {
            playerBodyGraphics.Die();
            bodyCollider.enabled = false;
            DisableAI();
            gunController.OnDie();
            GameController.Instance.SetPlayerDead(this, shooter);
        }
    }

    public void Heal(int amount)
    {
        health.Heal(amount);
        UpdateHealthSlider();
    }
    
    public void ActivateShield(in float duration)
    {
        shield.ActivateShield(duration);
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = health.GetCurrentHealth() / health.GetStartHealth();
    }

    private void UpdateReloadingSlider()
    {
        if (gunController.GetFirePeriod() >= 0)
        {
            reloadingSlider.value = gunController.GetTimeToNextShot() / gunController.GetFirePeriod();
        }
        else
        {
            Debug.LogError("Incorrect fire period!");
        }
    }

    public bool CanOpenAmmoBox()
    {
        return interactionController.CanOpenAmmoBox();
    }

    public void SwitchGunFromBox()
    {
        var gun = interactionController.OpenBoxInReach();
        if(gun != null)
            gunController.SwitchGun(gun);
    }

    public void SetDestination(Vector2 destination) {
        pathfindingController.SetDestination(destination);
    }

    public void SetMovementTarget(Transform target)
    {
        pathfindingController.SetTarget(target);
    }

    
    public void DisableAI() {
        enemyBehaviorTree.DisableBehavior();
        playerBehaviorTree.DisableBehavior();
        //Debug.Log("AI Disabled on " + nickText.text);
        enemyBehaviorTree.enabled = false;
        playerBehaviorTree.enabled = false;
    }
    
    public void EnablePlayerAI() 
    {
        playerBehaviorTree.enabled = true;
        playerBehaviorTree.EnableBehavior();
    }

    private void Update()
    {
        UpdateReloadingSlider();
    }

    private void FixedUpdate()
    {
        if (pathfindingController.GetDesiredMovement() != Vector2.zero) {
            Move(pathfindingController.GetDesiredMovement() * pathfindingSpeed); 
        }
    }
}
