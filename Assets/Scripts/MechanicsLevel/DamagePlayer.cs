using System;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
   [SerializeField] private float damage = 5f;
   [SerializeField] private string playerTag = "Player";
   


   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag(playerTag))
      {
         HealthSystem healthSystem = other.GetComponent<HealthSystem>();
         if (healthSystem !=null)
         {
                healthSystem.DecreaseHealth(damage);
                Debug.Log($"Trigger activado por: {other.name}");
         }
         
      }
   }
}
