using UnityEngine;
using UnityEngine.AI;

public class EnemyTypeA : EnemyBase
{
    

    protected override void MoveTowardsPlayer(Transform player)
    {
         NavMeshAgent.SetDestination(player.position);
    }

    protected override void Attack(Transform player)
    {
        if (CanAttack())
        {
            RaycastHit hit;
            var direction = (player.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
            {
                Debug.Log("Did Hit!!!!");
            }
        }

    }
}

