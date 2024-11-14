using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public PlayerSettings variables;

     private void Awake()
    {
        variables.health = variables.healthMax;
    }

    public void TakeDamage(float damage)
    {
        variables.health -= damage;
    }
    
}
