using UnityEngine;

public class DamagePrube : MonoBehaviour
{
    public HealthSystem healthSystem;
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            healthSystem.DecreaseHealth(damage);
        }
    }
}
