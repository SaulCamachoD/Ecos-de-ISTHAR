using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform player;   
        [SerializeField] private Transform enemy;    
        [SerializeField] private Vector3 offset;    
    
        private void LateUpdate()
        {
            if (player == null || enemy == null) return;
            
            transform.position = enemy.position + enemy.TransformDirection(offset);
            
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = lookRotation;
        }
}
