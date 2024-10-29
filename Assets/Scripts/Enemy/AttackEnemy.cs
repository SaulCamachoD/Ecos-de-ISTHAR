using UnityEngine;

namespace Enemy
{
    public class AttackEnemy : IEnemyState
    {
        private EnemyAIcontroller _enemyAI;

        public void EnterState(EnemyAIcontroller enemyAI)
        {
            _enemyAI = enemyAI;
            _enemyAI.navMeshAgent.isStopped = true;
            Debug.Log("estoy en ataque");
        }

        public void UpdateState()
        {
            if (_enemyAI.IsWhithinAttackRange())
            {
                if (_enemyAI.CanAttack())
                {

                    _enemyAI.PerformAttack();
                }
            }
            else
            {
                _enemyAI.TransitionToState(_enemyAI.ChaseEnemyState);
            }
        }
    

        public void ExitState()
        {
            _enemyAI.navMeshAgent.isStopped = false;
        }
    }
}