using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTypeA : EnemyBase
{
    private NavMeshAgent _navMeshAgent;
    private Transform _projectileSpawnPoint;
    public GameObject projectilePrefab;
   



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
            if (projectilePrefab == null)
            {
                Debug.LogError("Projectile prefab is not assigned!");
                return;
            }
            if (_projectileSpawnPoint == null)
            {
                Debug.LogError("Projectile spawn point is not assigned!");
                return;
            }
            if (ProjectilePool.Instance == null)
            {
                Debug.LogError("ProjectilePool instance is missing in the scene!");
                return;
            }
            
            var position = _projectileSpawnPoint.position;
            Vector3 direction = (player.position - position).normalized;
            GameObject projectile = ProjectilePool.Instance.GetProjectile(projectilePrefab);
            projectile.transform.position = position;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.GetComponent<Projectile>().SetOriginalPrefab(projectilePrefab);

        }

    }

    public void ShootEnemy()
    {
        
    }
}

