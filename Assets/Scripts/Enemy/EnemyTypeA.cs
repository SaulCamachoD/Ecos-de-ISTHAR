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
            Debug.Log("te ataco en A");
            
        }
    }
}
