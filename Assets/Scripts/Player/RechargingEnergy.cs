using UnityEngine;

public class RechargingEnergy : MonoBehaviour
{
    public EnergyManager energyManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            energyManager.IncreaseEnergy(20f);
            Destroy(gameObject);
        }
    }
}
