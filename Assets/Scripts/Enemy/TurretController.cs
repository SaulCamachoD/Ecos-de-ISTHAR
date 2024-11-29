using UnityEngine;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private Transform player;        // Referencia al jugador
    [SerializeField] private float detectionRange = 10f; // Rango de detección
    [SerializeField] private float fireRate = 2f;     // Tiempo entre disparos
    [SerializeField] private Transform firePoint;     // Punto de disparo
    [SerializeField] private GameObject projectilePrefab; // Prefab del proyectil
    [SerializeField] private float health = 100f;     // Vida de la torreta

    private float _fireCooldown;

    private void Start()
    {
       
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Si el jugador está dentro del rango de detección
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            AimAtPlayer(); // Apuntar hacia el jugador
            ShootAtPlayer(); // Disparar al jugador
        }
    }

    private void AimAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0); // Girar solo en el eje Y
    }

    private void ShootAtPlayer()
    {
        if (_fireCooldown > 0)
        {
            _fireCooldown -= Time.deltaTime;
            return;
        }

        // Crear el proyectil y lanzarlo
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * 10f; // Ajusta la velocidad del proyectil
            }
        }

        _fireCooldown = fireRate; // Reiniciar cooldown de disparo
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        Debug.Log("¡Torreta destruida!");
        Destroy(gameObject); 
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar el rango de detección en la escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

