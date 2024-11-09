using UnityEngine;
using UnityEngine.Serialization;

public class EnemyTypeB : EnemyBase
{
    [SerializeField] private float minAltitude = 3.0f;
    [SerializeField] private float maxAltitude = 15.0f;
    [SerializeField] private float detectionDistance = 5.0f;
    [SerializeField] private float avoidanceStrength = 3.0f;
    [SerializeField] private float reattemptInterval = 1.0f;

    private Vector3 _currentTarget;
    private float _reattemptTimer;
    private int _lastAvoidanceDirection = 1;

    protected override void MoveTowardsPlayer(Transform player)
    {
        float targetY = Mathf.Clamp(player.position.y + minAltitude, minAltitude, maxAltitude);
        Vector3 direction = (player.position - transform.position).normalized;
        
        Vector3 targetPosition = new Vector3(player.position.x, targetY, player.position.z);

       
        Debug.DrawRay(transform.position, direction * detectionDistance, Color.red);

        
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, detectionDistance))
        {
            _lastAvoidanceDirection *= -1;
            Vector3 horizontalAvoidanceDirection = Vector3.Cross(hit.normal, Vector3.up).normalized;
            horizontalAvoidanceDirection.y = 0; 
            
            _currentTarget = transform.position + horizontalAvoidanceDirection * avoidanceStrength * _lastAvoidanceDirection;
            _reattemptTimer = reattemptInterval;
        }
        else if (_reattemptTimer <= 0)
        {
            _currentTarget = targetPosition;
        }
        else
        {
            _reattemptTimer -= Time.deltaTime;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, moveSpeed * Time.deltaTime);
    }

    protected override void Attack(Transform player)
    {
        if (CanAttack())
        {
            Debug.Log("te ataco en B");
        }
    }
}
