using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float baseDamage = 10f;
    
    
    private float lifeTimer;
    private GameObject originalPrefab;
    public string enemyTag;
    public string playerTag;

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
        
        if (other.CompareTag(enemyTag))
        {
            
            BodyPart bodyPart = other.GetComponent<BodyPart>();
            float damageMultiplier = bodyPart != null ? bodyPart.damageMultiplier : 1f;
            float finalDamage = baseDamage * damageMultiplier;

            EnemyBase enemyBase = other.GetComponentInParent<EnemyBase>();
            if (enemyBase != null) 
            { 
                enemyBase.TakeDamage(finalDamage);
                Debug.Log($"Daño aplicado: {finalDamage}");
            }

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
