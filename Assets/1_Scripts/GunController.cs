using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class GunController : MonoBehaviour
{
    [SerializeField]
    private GunTypeSCRO currentGunType;

    [SerializeField]
    private Gun currentGun;

    [SerializeField]
    private GameObject gunRoot;

    [SerializeField]
    private GameObject LookAtRoot;


    private float timeToNextShot = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Fire(in Vector2 target, in Player shooter)
    {
        if (timeToNextShot <= 0) {
            currentGun.Fire(currentGunType.damage, target, shooter);
            timeToNextShot = GetFirePeriod();
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float GetFirePeriod()
    {
        return 60/currentGunType.fireRate;
    }

    public float GetTimeToNextShot()
    {
        return timeToNextShot;
    }

    public void Look(Vector2 target)
    {
        LookAtRoot.transform.LookAt(target);
        //Debug.Log("Looking");
    }

    public void Update()
    {
        if (timeToNextShot > 0)
        {
            timeToNextShot -= Time.deltaTime;
        }
        else timeToNextShot = 0;

    }

    public void SwitchGun(GunTypeSCRO newGun)
    {
        //Debug.Log("switching gun");
        currentGunType = newGun;
        Destroy(currentGun.gameObject);
        currentGun = Instantiate(currentGunType.gunPrototype, gunRoot.transform).GetComponent<Gun>();
        //Debug.Log(currentGun);
        timeToNextShot = 0;
    }

    public void OnDie() {
        Destroy(LookAtRoot);
        Destroy(this);
    }
}
