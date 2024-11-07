using UnityEngine;
using UnityEngine.Serialization;

public class EnemyTypeB : EnemyBase
{
    
    [SerializeField] private float minAltitude = 3.0f;
    [SerializeField] private float maxAltitude = 15.0f;
    
    protected override void MoveTowardsPlayer(Transform player)
    {
      float targetY = Mathf.Clamp(player.position.y + minAltitude, minAltitude, maxAltitude);
        Vector3 direction = (player.position - transform.position).normalized;
       
        Vector3 targetPosition = new Vector3(player.position.x, targetY, player.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
    }

    protected override void Attack(Transform player)
    {
        if (CanAttack())
        {
            Debug.Log("te ataco en B");
        }
    }
}
