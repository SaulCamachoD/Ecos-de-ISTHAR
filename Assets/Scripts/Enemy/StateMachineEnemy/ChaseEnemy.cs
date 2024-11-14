using UnityEngine;

namespace Enemy
{
    public class ChaseEnemy : IEnemyState
    {
        private EnemyAIcontroller _enemyAI;
        public void EnterState(EnemyAIcontroller enemyAI)
        {
            _enemyAI = enemyAI;
            if (_enemyAI.navMeshAgent != null)
            {
                _enemyAI.navMeshAgent.isStopped = false; 
            }
            _enemyAI.SetAnimationTrigger("Run");
            Debug.Log("estoy en caceria");
        }

        public void UpdateState()
        {
            _enemyAI.MoveTowardPlayer();
            if (_enemyAI.IsWhithinAttackRange())
            {
                _enemyAI.TransitionToState(_enemyAI.AttackEnemyState);
            }
            else if (!_enemyAI.CanSeePlayer())
            {
                _enemyAI.TransitionToState(_enemyAI.IdleEnemyState);
            }
        }

        public void ExitState()
        {
                
        }
    }
}