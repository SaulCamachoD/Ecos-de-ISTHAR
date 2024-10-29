using UnityEngine;

namespace Enemy
{
    public class IdleEnemy: IEnemyState
    {
        private EnemyAIcontroller _enemyAI;
        
        public void EnterState(EnemyAIcontroller enemyAI)
        {
            _enemyAI = enemyAI;
            Debug.Log("estoy en idle");
        }

        public void UpdateState()
        {
            if (_enemyAI.CanSeePlayer())
            {
                _enemyAI.TransitionToState(_enemyAI.ChaseEnemyState);
            }
        }

        public void ExitState()
        {
            
        }
    }
}