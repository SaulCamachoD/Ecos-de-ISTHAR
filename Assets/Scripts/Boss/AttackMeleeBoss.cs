using System;
using UnityEditor;
using UnityEngine;

public class AttackMeleeBoss : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Transform pointAttack;
    [SerializeField] private float attackRadius;
    [SerializeField] private float distanceAttack;
    private LayerMask _layerMaskPlayer;
    private Animator _animator;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    private void Attack()
    {

        if (distanceAttack < 15 )
        {
            Punch();
        }
        
        
    }
    
    private void Punch()
    { 
        Collider[] hit = Physics.OverlapSphere(pointAttack.position, attackRadius,_layerMaskPlayer);

        foreach (var hitC in hit)
        {
            hitC.GetComponent<HealthSystem>().TakeDamage(damage);
            Debug.Log($"boss a hecho la cantidad de {damage} de daño");
        }
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointAttack.position,attackRadius);
    }
}
