using System;
using Enemy;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAIcontroller : MonoBehaviour
{
    public IEnemyState IdleEnemyState = new IdleEnemy();
    public IEnemyState ChaseEnemyState = new ChaseEnemy();
    public IEnemyState DieEnemyState = new DieEnemy();
    public IEnemyState AttackEnemyState = new AttackEnemy();


    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public Transform player;
    private EnemyBase _enemy;
    private IEnemyState _currentEnemyState;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _enemy = GetComponent<EnemyBase>();
        TransitionToState(IdleEnemyState);
    }

    private void Update()
    {
       _currentEnemyState?.UpdateState();
        
    }
    
    public void TransitionToState(IEnemyState newState)
    {
        _currentEnemyState?.ExitState();
        _currentEnemyState = newState;
        _currentEnemyState.EnterState(this);
    }

    public bool CanSeePlayer()
    {
        return Vector3.Distance(transform.position, player.position) < _enemy.SightRange;
    }


    public bool IsWhithinAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= _enemy.AttackRange;
    }

    public bool CanAttack()
    {
        return _enemy.CanAttack();
    }

    public void PerformAttack()
    {
        _enemy.PerformAttack(player);
    }
    
    public void MoveTowardPlayer()
    {
       _enemy.PerformMoveTowardsPlayer(player);   
    }

    public void SetAnimationTrigger(string trigger)
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Attack");
        animator.SetTrigger(trigger);
    }
   
}
