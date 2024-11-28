using System;
using UnityEditor;
using UnityEngine;

public class AttackMeleeBoss : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Transform pointAttack;
    [SerializeField] private float attackRadius;
    [SerializeField] private float distanceAttack;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private Transform playerTransform;
    private bool canAttack = true;
    

    


    private void Update()
    {
       Punch();
    }

    private void Punch()
    {
        if (canAttack)
        {
            float distanceToplayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToplayer <= distanceAttack)
            {
                Collider[] hit = Physics.OverlapSphere(pointAttack.position, attackRadius);

                foreach (var hitC in hit)
                    if (hitC.TryGetComponent<HealthSystem>(out var healthSystem))
                    {
                        healthSystem.TakeDamage(damage);
                        Debug.Log($"boss a hecho la cantidad de {damage} de daño");
                    }

                canAttack = false;
                Invoke(nameof(ResetAttack), attackCooldown);

            }

        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointAttack.position,attackRadius);
    }
}
