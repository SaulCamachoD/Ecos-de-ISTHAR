using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float baseDamage = 10f;
    private float _lifeTimer;
    private GameObject _originalPrefab;
    public string enemyTag;
    public string playerTag; 
    public string objectTag;
    private HealthSystem _healthSystem;
    private void OnEnable()
    {
        _lifeTimer = lifeTime;
        ResetProjectile();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(enemyTag) || other.CompareTag(playerTag)|| other.CompareTag(objectTag))
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
           }
           else if (other.CompareTag(playerTag))
           {
               _healthSystem = other.GetComponentInParent<HealthSystem>();
               if (_healthSystem != null)
               {
                   _healthSystem.TakeDamage(baseDamage);
                   Debug.Log($"ataque enemigo {baseDamage}");
               }
               
           }
           else if (other.CompareTag(objectTag))
           {
               var breakeable = other.GetComponentInParent<BreakableObject>();
               if (breakeable != null)
               {
                   breakeable.TakeDamage(baseDamage);
                   Debug.Log($"ataque a objeto {baseDamage}");
               }
           }

           ReturnToPool();
        }
    }


    public void SetOriginalPrefab(GameObject prefab)
    {
        _originalPrefab = prefab;
    }

    private void ResetProjectile()
    {
        
    }
    private void ReturnToPool()
    {
        if (_originalPrefab != null)
        {
            ProjectilePool.Instance.ReturnProjectile(gameObject, _originalPrefab);
        }
        else
        {
            Debug.LogError("Original prefab no asignado al proyectil.");
        }
    }
}
