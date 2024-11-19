using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth = 100f;
    public float damage;
    public float moveSpeed = 5f;
    public float currentHealth;
    public GameObject prefabEnemy;

    [Header("Energy Management")]
    [SerializeField] private float maxEnergy = 50f;
    public float currentEnergy;
    [SerializeField] private float energyRechargeRate = 5f;
    [SerializeField] private float energyCostPerAttack = 10f;
    

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float sightRange = 5f;
    [SerializeField] private float attackCooldown = 1.5f;
    
    private float _nextAttackTime;
    public event Action <GameObject> OnEnemyDeath;

    private Animator _animator;
    

    public float SightRange => sightRange;
    public float CurrentEnergy => currentEnergy;
    public float AttackRange => attackRange;
 

    protected virtual void Awake()
    {

        _animator = GetComponentInChildren<Animator>();
       InitializeStats();
        
    }

    private void OnEnable()
    {
        InitializeStats();
    }

    protected virtual void Start()
    {
       
    }

    protected virtual void Update()
    {
        RechargeEnergy();
    }

    private void InitializeStats()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    
    public void PerformAttack(Transform player)
    {
        if (CanAttack())
        {
            _animator.SetTrigger("Attack");
            Attack(player);
            ConsumeEnergyForAttack();
        }
        
    }

    public void PerformMoveTowardsPlayer(Transform player)
    {
        MoveTowardsPlayer(player);
    }

    
    
    
    protected abstract void MoveTowardsPlayer(Transform player);
    protected abstract void Attack(Transform player);

    private void RechargeEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRechargeRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        }
    }

    protected internal bool CanAttack()
    {
        return Time.time >= _nextAttackTime && currentEnergy >= energyCostPerAttack;
    }

    protected void ConsumeEnergyForAttack()
    {
        currentEnergy -= energyCostPerAttack;
        _nextAttackTime = Time.time + attackCooldown;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        OnEnemyDeath?.Invoke(gameObject);
        ProjectilePool.Instance.ReturnProjectile(gameObject,prefabEnemy);
    }
}


