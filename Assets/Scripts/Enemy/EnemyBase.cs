using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    private NavMeshAgent _navMeshAgent;

    [Header("Energy Management")]
    [SerializeField] private float maxEnergy = 50f;
    private float _currentEnergy;
    [SerializeField] private float energyRechargeRate = 5f;
    [SerializeField] private float energyCostPerAttack = 10f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float sightRange = 5f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float _nextAttackTime;

    public float SightRange => sightRange;
    public float CurrentEnergy => _currentEnergy;
    public float AttackRange => attackRange;
    protected NavMeshAgent NavMeshAgent => _navMeshAgent;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = moveSpeed;
    }

    protected virtual void Start()
    {
        InitializeStats();
    }

    protected virtual void Update()
    {
        RechargeEnergy();
    }

    private void InitializeStats()
    {
        _currentHealth = maxHealth;
        _currentEnergy = maxEnergy;
    }

    
    public void PerformAttack(Transform player)
    {
        Attack(player);
        ConsumeEnergyForAttack();
    }

    public void PerformMoveTowardsPlayer(Transform player)
    {
        MoveTowardsPlayer(player);
    }

    
    
    
    protected abstract void MoveTowardsPlayer(Transform player);
    protected abstract void Attack(Transform player);

    private void RechargeEnergy()
    {
        if (_currentEnergy < maxEnergy)
        {
            _currentEnergy += energyRechargeRate * Time.deltaTime;
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0, maxEnergy);
        }
    }

    protected internal bool CanAttack()
    {
        return Time.time >= _nextAttackTime && _currentEnergy >= energyCostPerAttack;
    }

    protected void ConsumeEnergyForAttack()
    {
        _currentEnergy -= energyCostPerAttack;
        _nextAttackTime = Time.time + attackCooldown;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}


