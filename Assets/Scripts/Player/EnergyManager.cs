using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public float CurrentEnergy;
    public float EnergyRegenAmount;
    public Renderer targetRenderer;

    private void Start()
    {
        CurrentEnergy = playerSettings.energy;
        StartCoroutine(EnergyRegenRoutine());
    }

    private void Update()
    {
        UpdateEnergyLevel();
    }
    public void DecreaseEnergy(float amount)
    {
        CurrentEnergy = Mathf.Max(CurrentEnergy - amount, 0f);
        UpdateEnergyLevel();

    }

    
    public void IncreaseEnergy(float amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, playerSettings.energy);
        UpdateEnergyLevel();
    }

    private void UpdateEnergyLevel()
    {
        float energyLevel = CurrentEnergy / playerSettings.energy;
        targetRenderer.material.SetFloat("_LevelEnergy", energyLevel);
        
    }

    private System.Collections.IEnumerator EnergyRegenRoutine()
    {
        while (true)
        {
            if (CurrentEnergy > 0 && CurrentEnergy < playerSettings.energy)
            {
                IncreaseEnergy(EnergyRegenAmount);
            }
            yield return new WaitForSeconds(1f); // Espera 1 segundo entre regeneraciones.
        }
    }

}
