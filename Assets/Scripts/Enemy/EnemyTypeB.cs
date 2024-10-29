using UnityEngine;

public class EnemyTypeB : EnemyBase
{
    protected override void MoveTowardsPlayer(Transform player)
    {
        NavMeshAgent.SetDestination(player.position);
    }

    protected override void Attack(Transform player)
    {
        if (CanAttack())
        {
            Debug.Log("te ataco en B");
        }
    }
}
