using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Collider obstacleCollider;

    private void Awake()
    {
        obstacleCollider = GetComponent<Collider>();
    }

    public void SetTrigger(bool isTrigger)
    {
        obstacleCollider.isTrigger = isTrigger;
    }

    public void ResetTrigger()
    {
        obstacleCollider.isTrigger = false;
    }
}
