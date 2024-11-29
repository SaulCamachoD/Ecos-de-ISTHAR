using UnityEngine;

public class RechargingHeath : MonoBehaviour
{
    private HealthSystem healthSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (healthSystem == null)
            {
                healthSystem = other.GetComponent<HealthSystem>();
            }

            // Si se encontró HealthSystem, aumentar la salud
            if (healthSystem != null)
            {
                healthSystem.IncreaseHealth(20f);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("HealthSystem no encontrado en el jugador.");
            }
        }
    }
}
