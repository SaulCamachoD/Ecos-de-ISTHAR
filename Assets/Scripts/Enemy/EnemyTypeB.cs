using UnityEngine;

public class EnemyTypeB : EnemyBase
{
    protected override void MoveTowardsPlayer(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 flyPosition = new Vector3(direction.x, direction.y + 0.5f, direction.z);
        transform.position = Vector3.MoveTowards(transform.position, player.position + flyPosition, moveSpeed * Time.deltaTime);
        
    }

    protected override void Attack(Transform player)
    {
        if (CanAttack())
        {
            Debug.Log("te ataco en B");
        }
    }
}
