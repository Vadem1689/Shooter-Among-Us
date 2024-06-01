using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fireRoot;
    public GameObject bullet;

    [SerializeField] private float bulletSpeed = 100;
    
    private Projectile cachedProjectile;

    public virtual void Fire(float damage, Vector2 target, Player shooter)
    {
        var projectile = Instantiate(bullet, fireRoot.position, transform.rotation);
        projectile.SetActive(true);
        
        cachedProjectile = projectile.GetComponent<Projectile>();
        
        cachedProjectile.SetDamage(damage);
        cachedProjectile.SetShooter(shooter);

        var rigidBody = cachedProjectile.Rigidbody;
        var force = (target - new Vector2(fireRoot.position.x, fireRoot.position.y)).normalized * bulletSpeed;
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }
}
