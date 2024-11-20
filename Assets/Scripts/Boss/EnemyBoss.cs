using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
  public float bossHealth = 100f;
  public float bossDamage = 30f;
  public float bossSpeed = 10f;



 
  
  
  
  public void TakeDamage(float damage)
  {
    bossHealth -= damage;
  }
  
  
 
}
