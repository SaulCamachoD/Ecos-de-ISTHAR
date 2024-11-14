using UnityEngine;

public interface IEnemyState
{
    public void EnterState(EnemyAIcontroller enemyAI);
    public void UpdateState();
    public void ExitState();

}
