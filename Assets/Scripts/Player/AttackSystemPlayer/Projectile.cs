using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float damage = 10f;
    
    
    private float lifeTimer;
    private GameObject originalPrefab;
    public String targetTag;

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
        if (other.CompareTag(targetTag))
        {
            Debug.Log($"Impacto en {targetTag}");

            EnemyBase enemyBase = GetComponent<EnemyBase>();
            enemyBase.TakeDamage(damage);
           
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
