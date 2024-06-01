using System.Collections;
using System.Collections.Generic;
using BRAmongUS.SRCO;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameSettingsSCRO gameSettings;
    
    private float startAmmount = 100;

    private float currentAmmount;

    private bool isDead = false;


    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        startAmmount = gameSettings.PlayerHealth;
        currentAmmount = startAmmount;
    }

    public bool IsDead() {
        return isDead;
    }

    public void Heal(int ammount)
    {
        currentAmmount += ammount;
        if (currentAmmount > startAmmount) currentAmmount = startAmmount;
    }

    public bool TakeDamage(float ammount) {
        currentAmmount -= ammount;
        if (currentAmmount <= 0) isDead = true;
        return isDead;
    }

    public float GetCurrentHealth()
    {
        return currentAmmount;
    }

    public float GetStartHealth()
    {
        return startAmmount;
    }
}
