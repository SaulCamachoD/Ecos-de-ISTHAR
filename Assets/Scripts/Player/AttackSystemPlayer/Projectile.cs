using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    private float lifeTimer;
    private GameObject originalPrefab;

    private void OnEnable()
    {
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Impacto en enemigo");
            ReturnToPool();
        }
    }

    public void SetOriginalPrefab(GameObject prefab)
    {
        originalPrefab = prefab;
    }

    private void ReturnToPool()
    {
        if (originalPrefab != null)
        {
            ProjectilePool.Instance.ReturnProjectile(gameObject, originalPrefab);
        }
        else
        {
            Debug.LogError("Original prefab no asignado al proyectil.");
        }
    }
}
