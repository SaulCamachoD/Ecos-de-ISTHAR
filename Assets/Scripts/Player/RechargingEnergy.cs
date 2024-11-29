using UnityEngine;

public class RechargingEnergy : MonoBehaviour
{
    private EnergyManager energyManager;

    private void OnTriggerEnter(Collider other)
    {
     
        if(other.CompareTag("Player"))
        {
            if (energyManager == null)
            {
                energyManager = other.GetComponent<EnergyManager>();
            }

            if (energyManager != null)
            {
                energyManager.IncreaseEnergy(20f);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("HealthSystem no encontrado en el jugador.");
            }
        }
    }
}
