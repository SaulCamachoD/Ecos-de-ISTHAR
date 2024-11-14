using UnityEngine;
using UnityEngine.Serialization;

public class BreakableObject : MonoBehaviour
{
  [SerializeField] private float health;
  [SerializeField] private float currentHealth;
  [SerializeField] private GameObject normalObject;
  [SerializeField] private GameObject breakObject;
  private bool _isBroken = false;

  private void Start()
  {
    currentHealth = health;
    normalObject.SetActive(true);
    breakObject.SetActive(false);
  }

  public void TakeDamage(float damage)
  {
    if(_isBroken) return;
    currentHealth -= damage;
    if (currentHealth <=0)
    {
      BreakObject();
    }
  }

  private void BreakObject()
  {
    _isBroken = true;
    normalObject.SetActive(false);
    breakObject.SetActive(true);
    Debug.Log("Objeto roto activado");
  }
  
  
  
  
  
  
  
}
