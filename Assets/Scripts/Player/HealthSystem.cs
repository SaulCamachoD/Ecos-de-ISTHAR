using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthSystem : MonoBehaviour
{
   public PlayerSettings playerSettings;

   private void Awake()
   {
      playerSettings.health = playerSettings.healthMax;
   }

   public void TakeDamage(float damage)
   {
      playerSettings.health -= damage;
   }
}
