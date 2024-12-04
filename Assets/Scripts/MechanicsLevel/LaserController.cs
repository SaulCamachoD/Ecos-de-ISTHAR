using UnityEngine;

public class LaserController : MonoBehaviour
{
    public void DisableLaser()
    {
        Debug.Log($"Láser {gameObject.name} desactivado");
        gameObject.SetActive(false); // Desactiva el láser
    }
}
