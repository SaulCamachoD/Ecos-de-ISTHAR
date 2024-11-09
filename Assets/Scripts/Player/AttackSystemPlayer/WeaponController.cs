using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData[] weapons;
    private int currentWeaponIndex = 0;
    private float nextFireTime = 0f;
    private bool attacking = false;
    public Camera mainCamera;
    public float directionOffset = 0f;

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
        Debug.Log("Switched to " + weapons[currentWeaponIndex].projectilePrefab.name);
    }

    public void HandleFire()
    {
        if (Time.time >= nextFireTime)
        {
            WeaponData currentWeapon = weapons[currentWeaponIndex];
            nextFireTime = Time.time + 1f / currentWeapon.fireRate;

            GameObject projectile = ProjectilePool.Instance.GetProjectile(currentWeapon.projectilePrefab);
            projectile.transform.position = transform.position;

            Vector3 fireDirection = mainCamera.transform.forward;

            Quaternion offsetRotation = Quaternion.Euler(0, directionOffset, 0);
            fireDirection = offsetRotation * fireDirection;

            projectile.transform.rotation = Quaternion.LookRotation(fireDirection);

            projectile.GetComponent<Projectile>().SetOriginalPrefab(currentWeapon.projectilePrefab);
        }
    }

    public void Fire(bool isAttacknow)
    {
        attacking = isAttacknow;
    }

    public void ReturnProjectile(GameObject projectile)
    {
        ProjectilePool.Instance.ReturnProjectile(projectile, weapons[currentWeaponIndex].projectilePrefab);
    }


    private void OnDrawGizmos()
    {
        if (mainCamera != null)
        {
            // Obtiene la posición del arma o el punto de disparo
            Vector3 firePosition = transform.position;

            // Calcula la dirección de disparo desde el centro de la cámara
            Vector3 fireDirection = mainCamera.transform.forward;

            // Aplica la rotación de offset, si está configurada
            Quaternion offsetRotation = Quaternion.Euler(0, directionOffset, 0);
            fireDirection = offsetRotation * fireDirection;

            // Dibuja un rayo en el Editor para mostrar la dirección de disparo
            Gizmos.color = Color.red;
            Gizmos.DrawRay(firePosition, fireDirection * 10f); // Extiende el rayo para que sea visible
        }
    }
}
