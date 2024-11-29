using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public float CurrentEnergy;
    public Renderer targetRenderer;

    private void Start()
    {
        CurrentEnergy = playerSettings.energy;
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

}
