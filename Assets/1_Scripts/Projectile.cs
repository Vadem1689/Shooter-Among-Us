using System;
using System.Collections;
using System.Threading.Tasks;
using BRAmongUS.Gun;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impact;
    
    [SerializeField] private ProjectileSfx soundEffects;
    
    [SerializeField] private GameObject impactPlayer;
    
    private Player playerWhoShot;

    private float damage;

    public Rigidbody2D Rigidbody;

    public void SetDamage(float ammount)
    {
        damage = ammount;
    }

    public void SetShooter(in Player shooter)
    {
        playerWhoShot = shooter;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            if (player == playerWhoShot)
            {
                return;
            }
            if(!player.IsActiveShield)
            {
                player.TakeDamage(damage, playerWhoShot);
                impactPlayer.SetActive(true);
                impactPlayer.transform.SetParent(null);
                soundEffects.PlayHitSoundEffect();
            }
        }
        else
        {
            impact.SetActive(true);
            impact.transform.SetParent(null);
        }
        
        Destroy(gameObject);
    }
}
