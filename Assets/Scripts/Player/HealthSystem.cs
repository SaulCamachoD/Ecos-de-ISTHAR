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

    public void DecreaseHealth(float amount)
    {
        playerSettings.health = Mathf.Max(playerSettings.health - amount, 0f);
        UpdateHeatlhLevel();

    }

    public void IncreaseHealth(float amount)
    {
        playerSettings.health = Mathf.Min(playerSettings.health + amount, playerSettings.energy);
        UpdateHeatlhLevel();
    }

    private void UpdateHeatlhLevel()
    {
        float energyLevel = playerSettings.health / playerSettings.energy;
    }
}
