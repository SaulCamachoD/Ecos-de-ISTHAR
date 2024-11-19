using UnityEngine;

namespace Enemy
{
    public class AttackEnemy : IEnemyState
    {
        private EnemyAIcontroller _enemyAI;
        private bool _isAttacking;

        public void EnterState(EnemyAIcontroller enemyAI)
        {
            _enemyAI = enemyAI;
            if (_enemyAI.navMeshAgent != null)
            {
                _enemyAI.navMeshAgent.isStopped = true;
            }
            
            _isAttacking = false;
            Debug.Log("estoy en ataque");
        }

        public void UpdateState()
        {
            if (_enemyAI.IsWhithinAttackRange())
            {
                if (_enemyAI.CanAttack() && !_isAttacking)
                {
                    _isAttacking = true;
                    _enemyAI.SetAnimationTrigger("Attack");
                    _enemyAI.PerformAttack();
                }
                else if (!_enemyAI.CanAttack())
                {
                    _isAttacking = false;
                    _enemyAI.StopAnimation("Attack");
                }
                
            }
            else
            {
                _enemyAI.TransitionToState(_enemyAI.ChaseEnemyState);
            }
        }
    

        public void ExitState()
        {
            if (_enemyAI.navMeshAgent != null)
            {
                _enemyAI.navMeshAgent.isStopped = false;
            }
            _isAttacking = false;
        }
    }
}