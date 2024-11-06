using UnityEngine;
using UnityEngine.AI;

public class EnemyTypeA : EnemyBase
{
    private NavMeshAgent _navMeshAgent;



    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent != null)
        {
            _navMeshAgent.speed = moveSpeed;
        }
    }
    
    
    protected override void MoveTowardsPlayer(Transform player)
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.SetDestination(player.position);
        }
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

