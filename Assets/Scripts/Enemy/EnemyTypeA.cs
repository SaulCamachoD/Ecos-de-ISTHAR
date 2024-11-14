using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyTypeA : EnemyBase
{
    private NavMeshAgent _navMeshAgent; 
    public Transform projectileSpawnPoint;
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
            var position = projectileSpawnPoint.position;
            Vector3 direction = (player.position - position).normalized;
            GameObject projectile = ProjectilePool.Instance.GetProjectile(projectilePrefab);
            projectile.transform.position = position;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            var projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.SetOriginalPrefab(projectilePrefab);
                projectileScript.playerTag = "Player";
        }
    }


    public void ShootEnemy()
    {
        
    }
}

