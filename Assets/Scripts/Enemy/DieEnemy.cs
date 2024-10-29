using UnityEngine;

namespace Enemy
{
    public class DieEnemy: IEnemyState
    {
        private EnemyAIcontroller _enemyAI;
        public void EnterState(EnemyAIcontroller enemyAI)
        {
            _enemyAI = enemyAI;
            Debug.Log("estoy en muerte");
            
        }

        public void UpdateState()
        {
            
        }

        public void ExitState()
        {
           
        }
    }
}