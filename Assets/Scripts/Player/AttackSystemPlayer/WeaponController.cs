using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData[] weapons;
    private int currentWeaponIndex = 0;
    private float nextFireTime = 0f;
    private bool attacking = false;
    [SerializeField] bool attackingHeavy = false;
    public Camera mainCamera;
    public float directionOffset = 0f;
    public SounPlayerManager soundplayerManager;

    public EnergyManager energyManager;

    private void Update()
    {
        if (attacking)
        {
            HandleFire();
        }
    }

    public void HandleWeaponSwitch()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
    }

    public void HandleFire()
    {
        if (Time.time >= nextFireTime)
        {
            WeaponData currentWeapon = weapons[currentWeaponIndex];
            nextFireTime = Time.time + 1f / currentWeapon.fireRate;

            if (!attackingHeavy)
            {
                FireProjectile(currentWeapon);
            }
        }
    }


    private void FireProjectile(WeaponData weapon)
    {
        if (energyManager.CurrentEnergy >= weapon.energyCost) 
        {
            
            energyManager.DecreaseEnergy(weapon.energyCost);

            GameObject projectile = ProjectilePool.Instance.GetProjectile(weapon.projectilePrefab);
            projectile.transform.position = transform.position;

            Vector3 fireDirection = mainCamera.transform.forward;

            Quaternion offsetRotation = Quaternion.Euler(0, directionOffset, 0);
            fireDirection = offsetRotation * fireDirection;

            projectile.transform.rotation = Quaternion.LookRotation(fireDirection);

            projectile.GetComponent<Projectile>().SetOriginalPrefab(weapon.projectilePrefab);
        }
    }

    public void Fire(bool isAttacknow)
    {
        attacking = isAttacknow;
    }
    
    public void HeavyAttack(bool HeavyAttack)
    {
        attackingHeavy = HeavyAttack;
        if (currentWeaponIndex == 1)
        {
            FireProjectile(weapons[1]);
            attackingHeavy = false; 
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        ProjectilePool.Instance.ReturnProjectile(projectile, weapons[currentWeaponIndex].projectilePrefab);
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }
}
